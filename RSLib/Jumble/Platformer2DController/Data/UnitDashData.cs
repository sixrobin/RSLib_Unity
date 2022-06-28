namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Dash Data", menuName = "RSLib/2D Platformer/Dash Data")]
    public class UnitDashData : RSLib.Framework.Events.ValuesValidatedEventScriptableObject
    {
        [Header("DASH (MOTION)")]
        [Tooltip("Full dash motion duration.")]
        [SerializeField, Min(0f)] private float _duration = 1f;

        [Space(5f)]
        [Tooltip("Speed that will be multiplied by the dash curve evaluation.")]
        [SerializeField, Min(0f)] private float _speed = 3f;
        
        [Tooltip("Curve that will be applied to dash speed over the dash duration. Values should be between 0 and 1.")]
        [SerializeField] private AnimationCurve _speedCurve = null;

        [Tooltip("Multiplier applied to gravity while controller is dashing airborne.")]
        [SerializeField, Min(0f)] private AnimationCurve _gravityMultiplierCurve = RSLib.AnimationCurves.One;

        [Space(5f)]
        [Tooltip("Multiplier applied to unit speed on dash end.")]
        [SerializeField, Range(0f, 1f)] private float _dashOverSpeedMultiplier = 1f;
        
        [Tooltip("Percentage of the dash duration from which edges are detected not to fall.")]
        [SerializeField, Range(0f, 1f)] private float _edgeDetectionThreshold = 0f;

        [Header("DASH (MISC)")]
        [Tooltip("Allows the unit to dash airborne.")]
        [SerializeField] private bool _canDashAirborne = true;

        [Tooltip("Removes a jump left on dash end if controller is not on ground.")]
        [SerializeField] private bool _removeOneJumpLeftOnDashEnd = true;
        
        [Tooltip("Dash cooldown in seconds. Use 0 for no cooldown.")]
        [SerializeField, Min(0f)] private float _cooldown = 0.1f;

        [Tooltip("Multiplier applied to dash animation speed.")]
        [SerializeField] private float _animationSpeedMultiplier = 1f;

        public float Duration => _duration;
        public float Speed => _speed;
        public float EdgeDetectionThreshold => _edgeDetectionThreshold;
        public float DashOverSpeedMultiplier => _dashOverSpeedMultiplier;
        public bool CanDashAirborne => _canDashAirborne;
        public bool RemoveOneJumpLeftOnDashEnd => _removeOneJumpLeftOnDashEnd;
        public AnimationCurve SpeedCurve => _speedCurve;
        public float Cooldown => _cooldown;
        public bool HasCooldown => Cooldown > 0;
        public AnimationCurve GravityMultiplierCurve => _gravityMultiplierCurve;
        public float AnimationSpeedMultiplier => _animationSpeedMultiplier;
    }
}