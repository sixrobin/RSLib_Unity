namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Player Input Data", menuName = "RSLib/2D Platformer/Player Input Data")]
    public class PlayerInputData : RSLib.Framework.Events.ValuesValidatedEventScriptableObject
    {
        [Header("INPUT")]
        [Tooltip("Duration during which the jump input is stored and can be applied.")]
        [SerializeField, Min(0f)] private float _jumpInputDelay = 0.15f;

        [Tooltip("Duration during which the roll input is stored and can be applied.")]
        [SerializeField, Min(0f)] private float _rollInputDelay = 0.15f;

        [Tooltip("Default joystick dead zone.")]
        [SerializeField, Range(0f, 1f)] private float _defaultDeadZone = 0.2f;
        
        public float JumpInputDelay => _jumpInputDelay;
        public float RollInputDelay => _rollInputDelay;
        public float DefaultDeadZone => _defaultDeadZone;
    }
}