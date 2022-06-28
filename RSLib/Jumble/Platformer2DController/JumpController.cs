namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    public class JumpController
    {
        public JumpController(Platformer2DController controller)
        {
            _controller = controller;
            ResetJumpsLeft();

            _controller.ControllerData.ValuesValidated += OnDataValuesChanged;
        }

        ~JumpController()
        {
            _controller.ControllerData.ValuesValidated -= OnDataValuesChanged;
        }

        protected Platformer2DController _controller;

        private System.Collections.IEnumerator _jumpAnticipationCoroutine;
        private System.Collections.IEnumerator _landImpactCoroutine;

        public delegate void JumpedEventHandler(bool airborne);
        public delegate void LandedEventHandler(bool impact);
        public delegate void LandImpactEventHandler();
        public event JumpedEventHandler Jumped;
        public event LandedEventHandler Landed;
        public event LandImpactEventHandler LandedImpactOver;

        private UnitJumpData _lastJumpData;
        
        #region PROPERTIES

        protected UnitJumpData JumpData
        {
            get
            {
                // Refresh jump data.
                if (_lastJumpData != _controller.ControllerData.Jump)
                {
                    if (_lastJumpData != null)
                        _lastJumpData.ValuesValidated -= OnDataValuesChanged;

                    _lastJumpData = _controller.ControllerData.Jump;
                    if (_lastJumpData != null)
                        _lastJumpData.ValuesValidated += OnDataValuesChanged;
                }

                return _lastJumpData;
            }
        }

        public float Gravity { get; private set; }
        public float JumpVelocityMax { get; private set; }
        public float JumpVelocityMin { get; private set; }
        public int JumpsLeft { get; private set; }
        public float LandImpactSpeedMultiplier { get; private set; }

        public bool IsAnticipatingJump => _jumpAnticipationCoroutine != null;
        public bool IsInLandImpact => _landImpactCoroutine != null;

        public bool JumpAllowedThisFrame { get; set; }
        
        #endregion // PROPERTIES

        /// <summary>
        /// Computes all jump values based on maximum jump height and apex reach duration.
        /// Computed values are gravity and jump velocity min and max.
        /// </summary>
        public void ComputeJumpPhysics()
        {
            if (JumpData == null)
            {
                Gravity = UnitJumpData.DEFAULT_GRAVITY;
                return;
            }
            
            Gravity = -(2f * JumpData.JumpHeightMax) / JumpData.JumpApexDurationSqr;
            JumpVelocityMax = Mathf.Abs(Gravity) * JumpData.JumpApexDuration;
            JumpVelocityMin = Mathf.Sqrt(2f * Mathf.Abs(Gravity) * JumpData.JumpHeightMin);
        }
        
        /// <summary>
        /// Instantly applies maximum jump speed to current velocity.
        /// Calling this method will invoke jump event.
        /// </summary>
        /// <param name="velocity">Velocity to add maximum jump force to.</param>
        /// <param name="multiplier">Multiplier applied to force.</param>
        public void ApplyMaxVelocity(ref Vector3 velocity, float multiplier = 1f)
        {
            velocity.y = JumpVelocityMax * multiplier;
            RaiseJumpEvent();
        }
        
        /// <summary>
        /// Instantly applies minimum jump speed to current velocity.
        /// Calling this method will invoke jump event.
        /// </summary>
        /// <param name="velocity">Velocity to add minimum jump force to.</param>
        /// <param name="multiplier">Multiplier applied to force.</param>
        public void ApplyMinVelocity(ref Vector3 velocity, float multiplier = 1f)
        {
            velocity.y = JumpVelocityMin * multiplier;
            RaiseJumpEvent();
        }
        
        /// <summary>
        /// Instantly applies a various jump speed to current velocity, depending on player input for instance.
        /// Can be overriden, but applies max velocity by default and invoke jump event.
        /// </summary>
        /// <param name="velocity">Velocity to add jump force to.</param>
        /// <param name="multiplier">Multiplier applied to force.</param>
        public virtual void ApplyVariousVelocity(ref Vector3 velocity, float multiplier = 1f)
        {
            ApplyMaxVelocity(ref velocity, multiplier);
        }
        
        /// <summary>
        /// Raises jumped event.
        /// </summary>
        public void RaiseJumpEvent()
        {
            Jumped?.Invoke(JumpsLeft < JumpData.MaxFollowingJumps);
        }

        /// <summary>
        /// Spends a jump use.
        /// </summary>
        public void SpendOneJumpLeft()
        {
            JumpsLeft--;
        }
        
        /// <summary>
        /// Determines whether the controller can jump or not.
        /// Can be overriden to add specific conditions to suit each game requirements.
        /// </summary>
        /// <returns>True if jump is allowed, else false.</returns>
        public virtual bool CanJump()
        {
            return JumpsLeft > 0 && !IsInLandImpact && !IsAnticipatingJump;
        }

        /// <summary>
        /// Instantly resets the allowed jumps left.
        /// </summary>
        public void ResetJumpsLeft()
        {
            JumpsLeft = JumpData != null ? JumpData.MaxFollowingJumps : 0;
        }

        /// <summary>
        /// Starts jump anticipation coroutine that will trigger actual jump once complete.
        /// </summary>
        /// <param name="airborne">Is the jump anticipation being triggered airborne.</param>
        public void JumpAfterAnticipation(bool airborne = false)
        {
            float anticipationDuration = airborne ? JumpData.AirborneJumpAnticipationDuration : JumpData.JumpAnticipationDuration;
            _controller.StartCoroutine(_jumpAnticipationCoroutine = JumpAfterAnticipationCoroutine(anticipationDuration));
        }

        /// <summary>
        /// Triggers or not the landing impact behaviour based on fall velocity.
        /// </summary>
        /// <param name="landingSpeed">Landing speed (method will use the absolute value itself).</param>
        /// <returns>True if impact has been triggered, else false.</returns>
        public bool TriggerLandImpact(float landingSpeed)
        {
            float landingSpeedAbs = Mathf.Abs(landingSpeed);
            
            bool triggerImpact = _controller.CanTriggerLandingImpact()
                                 && JumpData != null
                                 && JumpData.MinVelocityForLandingImpact > 0
                                 && Mathf.Abs(landingSpeedAbs) > JumpData.MinVelocityForLandingImpact;
            
            if (triggerImpact)
                _controller.StartCoroutine(_landImpactCoroutine = WaitForLandImpactCoroutine(landingSpeedAbs));
            
            Landed?.Invoke(triggerImpact);
            return triggerImpact;
        }

        /// <summary>
        /// Jump anticipation sequence.
        /// </summary>
        /// <param name="duration">Anticipation duration.</param>
        private System.Collections.IEnumerator JumpAfterAnticipationCoroutine(float duration)
        {
            yield return RSLib.Yield.SharedYields.WaitForSeconds(duration);
            _controller.JumpWithVariousVelocity();
            yield return RSLib.Yield.SharedYields.WaitForFrames(2);
            _jumpAnticipationCoroutine = null;
        }

        /// <summary>
        /// Land impact sequence, with automatically computed values based on jump data and land speed.
        /// </summary>
        /// <param name="landingSpeedAbs">Absolute value of landing speed.</param>
        private System.Collections.IEnumerator WaitForLandImpactCoroutine(float landingSpeedAbs)
        {
            float impactDur = Mathf.Clamp(landingSpeedAbs * JumpData.LandingImpactDurationFactor, JumpData.LandingImpactDurationMin, JumpData.LandingImpactDurationMax);
            LandImpactSpeedMultiplier = RSLib.Maths.Maths.Normalize01(impactDur, JumpData.LandingImpactDurationMin, JumpData.LandingImpactDurationMax);
            LandImpactSpeedMultiplier = 1f - LandImpactSpeedMultiplier;
            LandImpactSpeedMultiplier = Mathf.Max(LandImpactSpeedMultiplier, JumpData.LandingImpactSpeedMultiplierMin);
            // CProLogger.Log(this, $"Landing at a speed of {fallSpeedAbs.ToString("f2")} => Computed impact duration of {impactDur.ToString("f2")}s and a speed mult of {LandImpactSpeedMult.ToString("f2")}.", _playerCtrl.gameObject);

            yield return RSLib.Yield.SharedYields.WaitForSeconds(impactDur);
            
            _landImpactCoroutine = null;
            LandedImpactOver?.Invoke();
        }
        
        private void OnDataValuesChanged()
        {
            ComputeJumpPhysics();
        }
    }
}