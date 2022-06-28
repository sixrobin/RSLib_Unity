namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Player Jump Data", menuName = "RSLib/2D Platformer/Jump Data")]
    public class UnitJumpData : RSLib.Framework.Events.ValuesValidatedEventScriptableObject
    {
        public const float DEFAULT_GRAVITY = -40f;
        
        [Header("JUMP")]
        [Tooltip("Height reached by the controller's pivot when at his jump maximum apex (input kept down for a long time).")]
        [SerializeField, Min(0f)] private float _jumpHeightMax = 2f;

        [Tooltip("Height reached by the controller's pivot when at his minimum jump apex (quick input down).")]
        [SerializeField, Min(0f)] private float _jumpHeightMin = 0.5f;

        [Tooltip("Duration in seconds that the controller takes to reach his jump apex.")]
        [SerializeField, Min(0f)] private float _jumpApexDuration = 0.5f;

        [Tooltip("Multiplier applied to Y velocity when controller is falling. Use 1 for no multiplying.")]
        [SerializeField, Min(0f)] private float _fallMultiplier = 1f;

        [Tooltip("Duration in seconds to reach target speed when controller is airborne. Use 0 for no damping.")]
        [SerializeField, Range(0f, 1f)] private float _airborneDamping = 0.2f;

        [Tooltip("Multiplier applied to X velocity when controller is preparing his jump. Use 1 for no multiplying.")]
        [SerializeField, Range(0f, 1f)] private float _jumpAnticipationSpeedMultiplier = 0.6f;

        [Tooltip("Jump anticipation duration. Use 0 for an instantaneous jump.")]
        [SerializeField, Min(0f)] private float _jumpAnticipationDuration = 0.1f;

        [Tooltip("Airborne jump anticipation duration. Use 0 for an instantaneous jump, which should be wanted for an airborne jump.")]
        [SerializeField, Min(0f)] private float _airborneJumpAnticipationDuration = 0.1f;

        [Tooltip("How many times can the player jump before he touches the ground and resets the counter.")]
        [SerializeField, Min(1)] private int _maxFollowingJumps = 1;

        [Header("LAND")]
        [Tooltip("Minimum Y velocity required to apply a landing impact. Use -1 to never apply an impact.")]
        [SerializeField, Min(-1f)] private float _minVelocityForLandingImpact = 8f;

        [Tooltip("Minimum multiplier that can be applied to X velocity on landing impact (meaning that 0 will nullify velocity on highest impact).")]
        [SerializeField, Min(0f)] private float _landingImpactSpeedMultiplierMin = 0.05f;

        [Tooltip("Multiplier applied to Y velocity to compute landing impact values (meaning that 0 will result in no impact).")]
        [SerializeField, Range(0f, 1f)] private float _landingImpactDurationFactor = 0.03f;

        [Tooltip("Minimum and maximum values for landing impact duration, also used as range for impact speed multiplier normalization.")]
        [SerializeField] private Vector2 _landingImpactDurationMinMax = Vector2.zero;

        // Jump.
        public float JumpHeightMax => _jumpHeightMax;
        public float JumpHeightMin => _jumpHeightMin;
        public float JumpApexDuration => _jumpApexDuration;
        public float JumpApexDurationSqr => JumpApexDuration * JumpApexDuration;
        public float FallMultiplier => _fallMultiplier;
        public float AirborneDamping => _airborneDamping;
        public float JumpAnticipationSpeedMultiplier => _jumpAnticipationSpeedMultiplier;
        public float JumpAnticipationDuration => _jumpAnticipationDuration;
        public float AirborneJumpAnticipationDuration => _airborneJumpAnticipationDuration;
        public int MaxFollowingJumps => _maxFollowingJumps;
        
        // Land.
        public float MinVelocityForLandingImpact => _minVelocityForLandingImpact;
        public float LandingImpactSpeedMultiplierMin => _landingImpactSpeedMultiplierMin;
        public float LandingImpactDurationFactor => _landingImpactDurationFactor;
        public float LandingImpactDurationMin => _landingImpactDurationMinMax.x;
        public float LandingImpactDurationMax => _landingImpactDurationMinMax.y;

        protected override void OnValidate()
        {
            _landingImpactDurationMinMax.x = Mathf.Max(_landingImpactDurationMinMax.x, 0f);
            _landingImpactDurationMinMax.y = Mathf.Max(_landingImpactDurationMinMax.x, _landingImpactDurationMinMax.y);

            base.OnValidate();
        }
    }
}