namespace RSLib.Data
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Data Easing Curve", menuName = "RSLib/Data/Easing Curve")]
    public class EasingCurve : ScriptableObject
    {
        public const RSLib.Maths.Curve DEFAULT = RSLib.Maths.Curve.Linear;

        [SerializeField] private RSLib.Maths.Curve _curve = DEFAULT;

        public RSLib.Maths.Curve Curve => _curve;
    }
}