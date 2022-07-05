namespace RSLib.Jumble.Platformer2D
{
    using RSLib.Extensions;
    using UnityEngine;

    public class PlayerController : Platformer2DController
    {
        [Header("PLAYER")]
        [SerializeField] private LayerMask _dashCollisionMask = 0;
        [SerializeField] private PlayerInputData _inputData = null;
        [SerializeField] private bool _inputsAllowedOnInit = true;
        
        private bool _inputsAllowed;

        public delegate void MovedEventHandler(Vector3 newPosition);
        public MovedEventHandler Moved;

        public InputController InputController { get; private set; }

        public override void Init()
        {
            if (Initialized)
                return;
            
            base.Init();

            InputController = new InputController(_inputData, this);

            CollisionsController = new CollisionsController(BoxCollider2D, this, CollisionMask, _dashCollisionMask);
            CollisionsController.CollisionDetected += OnCollisionDetected;
            ControllerData.ValuesValidated += OnDataValuesChanged;

            JumpController.ComputeJumpPhysics();
            if (ControllerData.GroundOnAwake)
                CollisionsController.Ground(transform);

            AllowInputs(_inputsAllowedOnInit);

            RSLib.Debug.Console.DebugConsole.OverrideCommand<float, float>("PositionAdd", "Adds a vector to the player position.", (x, y) => { transform.position += new Vector3(x, y); });
            RSLib.Debug.Console.DebugConsole.OverrideCommand<float, float>("PositionSet", "Sets the player position.", (x, y) => { transform.position = new Vector3(x, y); });
            RSLib.Debug.Console.DebugConsole.OverrideCommand<float>("MultiplySpeed", "Multiplies player speed.", x => _debugSpeedMultiplier = x);
            RSLib.Debug.Console.DebugConsole.OverrideCommand<float>("MultiplyJump", "Multiplies player jump.", x => _debugSpeedMultiplier = x);
            
            Initialized = true;
        }

        public void AllowInputs(bool state)
        {
            _inputsAllowed = state;
        }

        public override void Translate(Vector3 velocity, CollisionsController.CollisionComputationInfo collisionComputationInfo)
        {
            // We don't want to use the base method because it computes both direction and then translates, which can result in glitched corners collisions.
            // This is fine for enemies, but for the player we want to check any direction first, translate if needed, refresh the raycast origins, then check the other direction, and translate if needed.

            CollisionsController.ComputeRaycastOrigins();
            
            if (!collisionComputationInfo.DontResetCurrentCollisionState)
                CollisionsController.CurrentState.Reset();

            if (collisionComputationInfo.OnMovingPlatform)
                CollisionsController.CurrentState.Set(CollisionsController.CollisionOrigin.BELOW);

            velocity *= Time.deltaTime;

            if (CollisionsController.PreviousState.SlopeClimb)
            {
                velocity = CollisionsController.ComputeCollisions(velocity, collisionComputationInfo);
                transform.Translate(velocity);
            }
            else
            {
                if (velocity.y < 0f)
                    CollisionsController.DescendSlope(ref velocity);
                
                if (velocity.x != 0f)
                {
                    CollisionsController.ComputeHorizontalCollisions(ref velocity, collisionComputationInfo);
                    transform.Translate(new Vector3(velocity.x, 0f));
                }
                
                if (velocity.y != 0f)
                {
                    CollisionsController.ComputeRaycastOrigins(); // Recompute raycast origins before computing collisions.
                    CollisionsController.ComputeVerticalCollisions(ref velocity, collisionComputationInfo);
                    transform.Translate(new Vector3(0f, velocity.y));
                }
            }
        }

        public override bool CanTriggerLandingImpact()
        {
            return base.CanTriggerLandingImpact() && !InputController.CheckInput(InputController.ButtonCategory.DASH);
        }

        protected override bool TryDash()
        {
            if (!DashController.CanDash())
                return false;

            InputController.ResetDelayedInput(InputController.ButtonCategory.DASH);
            _currentVelocity = Vector3.zero;
            DashController.Dash(InputController.Horizontal != 0f ? Mathf.Sign(InputController.Horizontal) : CurrentDirection, DashOverCallback);
            return true;
        }

        private void Move()
        {
            if (DashController.IsDashing)
                return;

            if (CollisionsController.CurrentState.Vertical && !JumpController.IsAnticipatingJump)
            {
                if (CollisionsController.CurrentState.Grounded)
                    JumpController.ResetJumpsLeft();

                if (InputController.CheckInput(InputController.ButtonCategory.JUMP) && InputController.CurrentVerticalDirection < 0f)
                    StartCoroutine(_effectorDownCoroutine = EffectorDownCoroutine());

                if (!EffectorDown && !CollisionsController.CurrentState.Slope)
                    _currentVelocity.y = 0f;
            }

            if (InputController.Horizontal != 0f)
                SetDirection(InputController.CurrentHorizontalDirection);

            // Jump.
            if (JumpController.CanJump() && (!EffectorDown || !CollisionsController.AboveEffector))
            {
                JumpController.JumpAllowedThisFrame = true;
                InputController.ResetDelayedInput(InputController.ButtonCategory.JUMP);

                if (CollisionsController.CurrentState.Grounded)
                {
                    if (ControllerData.Jump.JumpAnticipationDuration > 0)
                        JumpController.JumpAfterAnticipation();
                    else
                        JumpController.ApplyMaxVelocity(ref _currentVelocity, _debugJumpMultiplier);
                }
                else
                {
                    JumpController.SpendOneJumpLeft();

                    // Airborne jump.
                    if (ControllerData.Jump.AirborneJumpAnticipationDuration > 0)
                        JumpController.JumpAfterAnticipation(true);
                    else
                        JumpController.ApplyMaxVelocity(ref _currentVelocity, _debugJumpMultiplier);
                }
            }

            // Jump lower if jump input is released.
            if (!CollisionsController.CurrentState.Grounded && !JumpController.IsAnticipatingJump && !InputController.CheckJumpInput() && _currentVelocity.y > JumpController.JumpVelocityMin)
                _currentVelocity.y = JumpController.JumpVelocityMin;

            if (EffectorDown && CollisionsController.AboveEffector)
                StartCoroutine(ResetJumpInputAfterDownEffector());

            float targetVelocityX = 0f;
            if (CanMove())
            {
                targetVelocityX = ControllerData.RunSpeed * _debugSpeedMultiplier;
                targetVelocityX *= InputController.Horizontal;

                // Clamp value to a minimum, to avoid players settings their dead zone to a low value being too slow.
                if (InputController.Horizontal > 0f)
                    targetVelocityX = Mathf.Max(targetVelocityX, ControllerData.MinRunSpeed);
                else if (InputController.Horizontal < 0f)
                    targetVelocityX = Mathf.Min(targetVelocityX, -ControllerData.MinRunSpeed);

                if (JumpController.IsAnticipatingJump)
                    targetVelocityX *= ControllerData.Jump.JumpAnticipationSpeedMultiplier;
                
                if (JumpController.IsInLandImpact)
                    targetVelocityX *= JumpController.LandImpactSpeedMultiplier;
            }

            float gravity = JumpController.Gravity * Time.deltaTime;
            if (_currentVelocity.y < 0f && ControllerData.Jump != null)
                gravity *= ControllerData.Jump.FallMultiplier;

            _currentVelocity.x = Mathf.SmoothDamp(_currentVelocity.x, targetVelocityX, ref _refVelocityX, CollisionsController.CurrentState.Grounded || ControllerData.Jump == null
                                                                                                                         ? ControllerData.GroundedDamping
                                                                                                                         : ControllerData.Jump.AirborneDamping);
            
            _currentVelocity.y += gravity;
            _currentVelocity.y = Mathf.Max(_currentVelocity.y, -ControllerData.MaxFallVelocity);

            // Doing this here makes events being well triggered but causes the tennis ball bug.
            //_currVel += GetCurrentRecoil();
            
            Translate(_currentVelocity, new CollisionsController.CollisionComputationInfo { TriggerEvents = true,
                                                                                            EffectorDown = EffectorDown });
            
            if (targetVelocityX != 0f)
                Moved?.Invoke(transform.position);
            
            // Doing a grounded jump or falling will trigger this condition and remove one jump left. Need to do this after the ComputeCollisions call.
            if (!CollisionsController.CurrentState.Grounded && CollisionsController.PreviousState.Grounded)
                JumpController.SpendOneJumpLeft();
        }

        /// <summary>
        /// Backups the current controller state.
        /// Should be called at the beginning of each frame before doing movement computations.
        /// </summary>
        private void BackupCurrentState()
        {
            CollisionsController.BackupCurrentState();
            _previousVelocity = _currentVelocity;
        }

        /// <summary>
        /// Resets the controller state.
        /// Should be called at the beginning of each frame before doing movement computations and after doing some state backup if needed.
        /// </summary>
        private void ResetCurrentState()
        {
            InputController.Reset();
            JumpController.JumpAllowedThisFrame = false;
        }

        /// <summary>
        /// Used to avoid player to jump after falling down from an effector too close to the ground.
        /// </summary>
        private System.Collections.IEnumerator ResetJumpInputAfterDownEffector()
        {
            const float disallowedJumpDuration = 0.02f;
            yield return RSLib.Yield.SharedYields.WaitForSeconds(disallowedJumpDuration);
            InputController.ResetDelayedInput(InputController.ButtonCategory.JUMP);
        }

        /// <summary>
        /// Allows player to fall down even if on a downward moving platform.
        /// Wait duration should be based on platform vertical velocity but a constant should be working for most cases.
        /// </summary>
        private System.Collections.IEnumerator EffectorDownCoroutine()
        {
            const float effectorDownDuration = 0.1f;
            yield return RSLib.Yield.SharedYields.WaitForSeconds(effectorDownDuration);
            _effectorDownCoroutine = null;
        }

        protected override void Update()
        {
            if (!Initialized)
                return;

            base.Update();

            BackupCurrentState();
            ResetCurrentState();

            if (_inputsAllowed)
                InputController.Update();

            TryDash();
            Move();
            ApplyCurrentRecoil();

            CollisionsController.ProcessDetectedCollisions();
        }

        private void OnDestroy()
        {
            CollisionsController.CollisionDetected -= OnCollisionDetected;
            ControllerData.ValuesValidated -= OnDataValuesChanged;
        }

        private void OnDataValuesChanged()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                if (InputController.InputData != _inputData)
                    InputController = new InputController(_inputData, this);
            }
#endif
        }
    }
}