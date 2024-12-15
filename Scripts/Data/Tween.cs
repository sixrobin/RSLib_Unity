namespace RSLib.Unity.Data
{
    using RSLib.CSharp.Maths;
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Data Tween", menuName = "RSLib/Data/Tween", order = -50)]
    public class Tween : ScriptableObject
    {
        [SerializeField]
        private float _duration = 0f;
        [SerializeField]
        private Curve _curve = Curve.InOutSine;

        public float Duration => Mathf.Max(0f, _duration);
        public Curve Curve => _curve;

        private void OnValidate()
        {
            _duration = Mathf.Max(0f, _duration);
        }
    }
}