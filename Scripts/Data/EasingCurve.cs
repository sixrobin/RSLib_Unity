namespace RSLib.Data
{
    using RSLib.CSharp.Maths;
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Data Easing Curve", menuName = "RSLib/Data/Easing Curve", order = -50)]
    public class EasingCurve : ScriptableObject
    {
        public const Curve DEFAULT = Curve.Linear;

        [SerializeField]
        private Curve _curve = DEFAULT;

        public Curve Curve => _curve;
        
        #region CONVERSION OPERATORS
        
        public static implicit operator Curve(EasingCurve dataEasingCurve)
        {
            return dataEasingCurve.Curve;
        }

        #endregion // CONVERSION OPERATORS

        public override string ToString()
        {
            return Curve.ToString();
        }
    }
}