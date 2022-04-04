namespace RSLib.Dynamics
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Dynamic Float", menuName = "RSLib/Dynamics/Float")]
    public class Float : ScriptableObject
    {
        public struct ValueChangedEventArgs
        {
            public float Previous;
            public float New;
        }

        [SerializeField] private float _value = 0;
        [SerializeField] private Vector2 _range = new Vector2(Mathf.NegativeInfinity, Mathf.Infinity);

        public delegate void ValueChangedEventHandler(ValueChangedEventArgs args);
        public event ValueChangedEventHandler ValueChanged;

        public float Value
        {
            get => _value;
            set
            {
                float newValue = Mathf.Clamp(value, _range.x, _range.y);

                ValueChangedEventArgs valueChangedEventArgs = new ValueChangedEventArgs()
                {
                    Previous = _value,
                    New = newValue
                };

                _value = newValue;
                ValueChanged?.Invoke(valueChangedEventArgs);
            }
        }

        public Vector2 Range => _range;
        public float Min => Range.x;
        public float Max => Range.y;
        
        private void OnValidate()
        {
            _value = Mathf.Clamp(_value, _range.x, _range.y);
        }

        #region OPERATORS

        public static Float operator +(Float a, Float b)
        {
            a.Value += b.Value;
            return a;
        }
        public static Float operator +(Float a, float b)
        {
            a.Value += b;
            return a;
        }
        public static float operator +(float a, Float b)
        {
            a += b.Value;
            return a;
        }

        public static Float operator -(Float a, Float b)
        {
            a.Value -= b.Value;
            return a;
        }
        public static Float operator -(Float a, float b)
        {
            a.Value -= b;
            return a;
        }
        public static float operator -(float a, Float b)
        {
            a -= b.Value;
            return a;
        }

        public static Float operator *(Float a, Float b)
        {
            a.Value *= b.Value;
            return a;
        }
        public static Float operator *(Float a, float b)
        {
            a.Value *= b;
            return a;
        }
        public static float operator *(float a, Float b)
        {
            a *= b.Value;
            return a;
        }

        public static Float operator /(Float a, Float b)
        {
            a.Value /= b.Value;
            return a;
        }
        public static Float operator /(Float a, float b)
        {
            a.Value /= b;
            return a;
        }
        public static float operator /(float a, Float b)
        {
            a /= b.Value;
            return a;
        }

        public static Float operator %(Float a, Float b)
        {
            a.Value %= b.Value;
            return a;
        }
        public static Float operator %(Float a, float b)
        {
            a.Value %= b;
            return a;
        }
        public static float operator %(float a, Float b)
        {
            a %= b.Value;
            return a;
        }

        #endregion OPERATORS
    }
}