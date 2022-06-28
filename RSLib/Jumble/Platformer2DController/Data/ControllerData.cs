namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Controller Data", menuName = "RSLib/2D Platformer/Controller Data")]
    public class ControllerData : RSLib.Framework.Events.ValuesValidatedEventScriptableObject
    {
        [Header("JUMP (Can be null)")]
        [SerializeField] private UnitJumpData _jumpData = null;

        [Header("DASH (Can be null)")]
        [SerializeField] private UnitDashData _dashData = null;

        [Header("MOVEMENT (BASE)")]
        [Tooltip("Base run speed of the controller.")]
        [SerializeField, Min(0f)] private float _runSpeed = 1f;

        [Tooltip("Minimum run speed, despite axis dead zone being possibly very low.")]
        [SerializeField, Min(0f)] private float _minRunSpeed = 0.5f;

        [Tooltip("Duration in seconds to reach target speed when controller is on the ground. Use 0 for no damping.")]
        [SerializeField, Range(0f, 1f)] private float _groundedDamping = 0f;
        
        [Tooltip("Maximum fall velocity.")]
        [SerializeField, Min(0f)] private float _maxFallVelocity = 1000f;

        [Header("MOVEMENT (SLOPES)")]
        [SerializeField, Range(0f, 90f)] private float _maxSlopeClimbAngle = 45f;
        
        [SerializeField, Range(0f, 90f)] private float _maxSlopeDescendAngle = 35f;

        [Header("MISC")]
        [Tooltip("Instantly grounds the controller on awake without triggering any event, just for visual purpose.")]
        [SerializeField] private bool _groundOnAwake = true;

        public UnitJumpData Jump => _jumpData;
        public UnitDashData Dash => _dashData;
        
        public float RunSpeed => _runSpeed;
        public float MinRunSpeed => _minRunSpeed;
        public float GroundedDamping => _groundedDamping;
        public bool GroundOnAwake => _groundOnAwake;
        public float MaxFallVelocity => _maxFallVelocity;
        public float MaxSlopeClimbAngle => _maxSlopeClimbAngle;
        public float MaxSlopeDescendAngle => _maxSlopeDescendAngle;

        protected override void OnValidate()
        {
            _minRunSpeed = Mathf.Min(MinRunSpeed, RunSpeed);
            base.OnValidate();
        }
    }
}