namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    [DisallowMultipleComponent]
    public abstract class Platformer2DController : MonoBehaviour
    {
        [Header("UNIT BASE")]
        [SerializeField] private ControllerData _controllerData = null;
        [SerializeField] private BoxCollider2D _boxCollider2D = null;
        [SerializeField] private LayerMask _collisionMask = 0;
        [SerializeField] private bool _initOnStart = true;
#if UNITY_EDITOR
        [SerializeField] private RSLib.Framework.DisabledString _debugCollisionsState = new RSLib.Framework.DisabledString();
        [SerializeField] private RSLib.Framework.DisabledString _debugSlopeAngle = new RSLib.Framework.DisabledString();
#endif
        
        protected System.Collections.IEnumerator _effectorDownCoroutine;

        protected Vector3 _currentVelocity;
        protected Vector3 _previousVelocity;
        protected float _refVelocityX;
        
        protected Recoil _currentRecoil;
        
        protected float _debugSpeedMultiplier = 1f;
        protected float _debugJumpMultiplier = 1f;

        #region PROPERTIES
        
        public bool Initialized { get; protected set; }
        
        public ControllerData ControllerData => _controllerData;
        public BoxCollider2D BoxCollider2D => _boxCollider2D;
        protected LayerMask CollisionMask => _collisionMask;

        public CollisionsController CollisionsController { get; protected set; }
        public JumpController JumpController { get; protected set; }
        public DashController DashController { get; protected set; }
        
        public float CurrentDirection { get; protected set; }
        public float NoGroundTimer { get; private set; }

        public Vector3 CurrentVelocity => _currentVelocity;
        public Vector3 PreviousVelocity => _previousVelocity;

        public bool EffectorDown => _effectorDownCoroutine != null;
        
        public bool WasFallingLastFrame => _currentVelocity.y < 0f && !CollisionsController.PreviousState.Grounded;
        
        // public bool IsOnMovingPlatform { get; set; }

        #endregion // PROPERTIES

        // void Templar.Physics.MovingPlatform.IMovingPlatformPassenger.OnPlatformMoved(Vector3 vel, bool standingOnPlatform)
        // {
        //     if (float.IsNaN(vel.x) || float.IsNaN(vel.y))
        //     {
        //         CProLogger.LogWarning(this, $"Translating {transform.name} on MovingPlatform by a NaN vector.", gameObject);
        //         return;
        //     }
        //
        //     IsOnMovingPlatform = true;
        //     Translate(vel, triggerEvents: false, standingOnPlatform: standingOnPlatform);
        // }

        public virtual void Init()
        {
            if (Initialized)
                return;

            JumpController = new JumpControllerPlayer(this);
            DashController = new DashControllerPlayer(this);
            
            Initialized = true;
        }

        #region DIRECTION METHODS

        /// <summary>
        /// Sets the controller current horizontal direction.
        /// </summary>
        /// <param name="direction">Direction sign (-1 is left, 1 is right).</param>
        public void SetDirection(float direction)
        {
            CurrentDirection = direction;
            // TODO: Event directionChanged;
        }

        /// <summary>
        /// Sets current direction based on relative direction to target.
        /// </summary>
        /// <param name="target">World position to look at.</param>
        public void LookAt(Vector3 target)
        {
            SetDirection(Mathf.Sign(target.x - transform.position.x));
        }
        
        #endregion // DIRECTION METHODS

        #region MOVEMENT METHODS

        /// <summary>
        /// Translates the controller by a given velocity that's modified by collisions computation.
        /// Adjusted velocity is applied directly to transform.position.
        /// </summary>
        /// <param name="velocity">Movement velocity (NOT scaled by Time.deltaTime).</param>
        /// <param name="collisionComputationInfo">Custom collision computation parameters.</param>
        public virtual void Translate(Vector3 velocity, CollisionsController.CollisionComputationInfo collisionComputationInfo)
        {
            velocity = CollisionsController.ComputeCollisions(velocity * Time.deltaTime, collisionComputationInfo);
            transform.Translate(velocity);
        }

        /// <summary>
        /// Instantly applies a various jump speed to current velocity, depending on player input for instance.
        /// Can be overriden, but applies max velocity by default and invoke jump event.
        /// </summary>
        public virtual void JumpWithVariousVelocity()
        {
            JumpController.ApplyVariousVelocity(ref _currentVelocity, _debugJumpMultiplier);
        }

        /// <summary>
        /// Instantly nullifies controller velocity.
        /// </summary>
        public virtual void ResetVelocity()
        {
            _currentVelocity = Vector3.zero;
        }

        #endregion // MOVEMENT METHODS

        #region RECOIL

        /// <summary>
        /// Instantly sets the current recoil.
        /// </summary>
        /// <param name="direction">Direction in which the controller is recoiled.</param>
        /// <param name="recoilData">Recoil data.</param>
        /// <param name="forceOverride">Forces the recoil to be set in case controller is already being recoiled.</param>
        public void SetCurrentRecoil(float direction, RecoilData recoilData, bool forceOverride = false)
        {
            if (_currentRecoil == null || forceOverride)
                _currentRecoil = new Recoil(direction, recoilData);
        }
        
        /// <summary>
        /// Applies current recoil force to controller velocity.
        /// </summary>
        protected void ApplyCurrentRecoil()
        {
            if (_currentRecoil == null)
                return;
        
            float recoilX = _currentRecoil.Direction * _currentRecoil.Data.Force;
            if (!CollisionsController.CurrentState.Grounded)
                recoilX *= _currentRecoil.Data.AirborneMultiplier;

            CollisionsController.CollisionComputationInfo recoilCollisionComputationInfo = new CollisionsController.CollisionComputationInfo
            {
                DontResetCurrentCollisionState = true,
                CheckEdge = _currentRecoil.Data.CheckEdge
            };

            Translate(new Vector3(recoilX, 0f), recoilCollisionComputationInfo);
        
            _currentRecoil.Update();
            if (_currentRecoil.IsComplete)
                _currentRecoil = null;
        }

        #endregion // RECOIL
        
        /// <summary>
        /// Determines if controller can move or not.
        /// Returns true by default, but can be overriden to suit any specific game requirements.
        /// </summary>
        /// <returns>True if controller movement should be allowed, else false.</returns>
        protected virtual bool CanMove()
        {
            return true;
        }
        
        /// <summary>
        /// Determines if landing impact can be triggered.
        /// Can be overriden to add specific conditions to suit each game requirements.
        /// </summary>
        /// <returns>True if landing impact should be allowed, else false.</returns>
        public virtual bool CanTriggerLandingImpact()
        {
            return !DashController.IsDashing;
        }

        /// <summary>
        /// Starts dash sequence if dash is allowed.
        /// Can be overriden to add specific conditions to suit each game requirements.
        /// </summary>
        /// <returns>True if dash has been triggered, else false.</returns>
        protected virtual bool TryDash()
        {
            if (!DashController.CanDash())
                return false;

            ResetVelocity();
            DashController.Dash(CurrentDirection, DashOverCallback);
            return true;
        }
        
        /// <summary>
        /// Methods called when dash ends.
        /// Can be overriden to add specific behaviour to suit each game requirements.
        /// </summary>
        /// <param name="args">Dash end data.</param>
        protected virtual void DashOverCallback(DashController.DashOverEventArgs args)
        {
            _currentVelocity = args.Velocity * ControllerData.Dash.DashOverSpeedMultiplier;
            if (!CollisionsController.CurrentState.Grounded && ControllerData.Dash.RemoveOneJumpLeftOnDashEnd)
                JumpController.SpendOneJumpLeft();
        }
        
        /// <summary>
        /// Listener method called when a collision has been processed and detected.
        /// Can be overriden but check the early return of the virtual method before doing so.
        /// </summary>
        /// <param name="args">Collision info.</param>
        protected virtual void OnCollisionDetected(CollisionsController.CollisionDetectedEventArgs args)
        {
            // Avoid triggering event if there was a collision from the same origin at the previous frame.
            if (CollisionsController.PreviousState.Get(args.Origin))
                return;

            switch (args.Origin)
            {
                case CollisionsController.CollisionOrigin.BELOW:
                    UnityEngine.Assertions.Assert.IsTrue(_currentVelocity.y < 0f, $"Detected a landing with a positive y velocity ({_currentVelocity.y})!");
                    JumpController.TriggerLandImpact(-_currentVelocity.y);
                    break;
                case CollisionsController.CollisionOrigin.ABOVE:
                case CollisionsController.CollisionOrigin.LEFT:
                case CollisionsController.CollisionOrigin.RIGHT:
                case CollisionsController.CollisionOrigin.EDGE:
                    break;
                default:
                    CProLogger.LogError(this, $"Unhandled collision origin {args.Origin}!");
                    break;
            }
        }

        #region UNITY EVENTS FUNCTIONS
        
        protected virtual void Start()
        {
            if (_initOnStart)
                Init();
        }

        protected virtual void Update()
        {
            if (CollisionsController.CurrentState.Grounded)
                NoGroundTimer = 0f;
            else
                NoGroundTimer += Time.deltaTime;
            
#if UNITY_EDITOR
            _debugCollisionsState = new RSLib.Framework.DisabledString(CollisionsController.CurrentState.ToString());
            _debugSlopeAngle = new RSLib.Framework.DisabledString(CollisionsController.CurrentState.SlopeAngle.ToString("f2"));
#endif
        }
        
        #endregion // UNITY EVENTS FUNCTIONS
    }
}