namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    public class DashController
    {
        public class DashStartedEventArgs : System.EventArgs
        {
            public DashStartedEventArgs(bool airborne)
            {
                Airborne = airborne;
            }

            public bool Airborne { get; }
        }
        
        public class DashOverEventArgs : System.EventArgs
        {
            public DashOverEventArgs(Vector3 velocity, bool interrupted)
            {
                Velocity = velocity;
                Interrupted = interrupted;
            }

            public Vector3 Velocity { get; }
            public bool Interrupted { get; }
        }

        protected Platformer2DController _controller;

        private System.Collections.IEnumerator _dashCoroutine;
        private System.Collections.IEnumerator _dashCooldownCoroutine;

        private Vector3 _dashVelocity;
        
        public DashController(Platformer2DController controller)
        {
            _controller = controller;
            _controller.ControllerData.ValuesValidated += OnDataValuesChanged;
        }

        ~DashController()
        {
            _controller.ControllerData.ValuesValidated -= OnDataValuesChanged;
        }

        public delegate void DashOverEventHandler(DashOverEventArgs args);
        public delegate void DashStartedEventHandler(DashStartedEventArgs args);

        public event DashStartedEventHandler DashStarted;
        public event DashOverEventHandler DashOver;

        public UnitDashData DashData => _controller.ControllerData.Dash;

        public bool IsDashing => _dashCoroutine != null;
        public bool IsDashingOrInCooldown => IsDashing || _dashCooldownCoroutine != null;

        public virtual bool CanDash()
        {
            return DashData != null 
                   && !IsDashingOrInCooldown
                   && (_controller.CollisionsController.CurrentState.Grounded || DashData.CanDashAirborne)
                   && !_controller.JumpController.IsInLandImpact;
        }

        public void Interrupt()
        {
            if (!IsDashing)
                return;

            _controller.StopCoroutine(_dashCoroutine);
            _dashCoroutine = null;
            DashOver?.Invoke(new DashOverEventArgs(_dashVelocity, true));
        }

        public void Dash(float direction, DashOverEventHandler dashOverCallback = null)
        {
            _controller.StartCoroutine(_dashCoroutine = DashCoroutine(direction, dashOverCallback));
        }

        protected virtual float ComputeCooldown()
        {
            return DashData.Cooldown;
        }
        
        private void OnDataValuesChanged()
        {
        }

        private System.Collections.IEnumerator DashCoroutine(float direction, DashOverEventHandler dashOverCallback = null)
        {
            DashStarted?.Invoke(new DashStartedEventArgs(!_controller.CollisionsController.CurrentState.Grounded));

            for (float t = 0f; t < 1f; t += Time.deltaTime / DashData.Duration)
            {
                CollisionsController.CollisionComputationInfo dashCollisionComputationInfo = new CollisionsController.CollisionComputationInfo()
                {
                    DontResetCurrentCollisionState = true,
                    TriggerEvents = true,
                    CheckEdge = t * DashData.Duration >= DashData.EdgeDetectionThreshold
                };
                
                _dashVelocity.x = DashData.SpeedCurve.Evaluate(t) * DashData.Speed * direction;
                _dashVelocity.y += _controller.JumpController.Gravity * Time.deltaTime * DashData.GravityMultiplierCurve.Evaluate(t);
                _controller.Translate(_dashVelocity, dashCollisionComputationInfo);
                yield return null;
            }

            if (DashData.HasCooldown)
            {
                UnityEngine.Assertions.Assert.IsNull(_dashCooldownCoroutine, "Dash is about to end and cooldown coroutine is already running.");
                _controller.StartCoroutine(_dashCooldownCoroutine = DashCooldownCoroutine());
            }

            DashOverEventArgs dashOverEventArgs = new DashOverEventArgs(_dashVelocity, false);
            dashOverCallback?.Invoke(dashOverEventArgs);
            DashOver?.Invoke(dashOverEventArgs);
            
            _dashCoroutine = null;
        }

        private System.Collections.IEnumerator DashCooldownCoroutine()
        {
            yield return RSLib.Yield.SharedYields.WaitForSeconds(ComputeCooldown());
            _dashCooldownCoroutine = null;
        }
    }
}