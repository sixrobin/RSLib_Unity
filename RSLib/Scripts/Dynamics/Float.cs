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

        public static bool operator >(Float a, Float b)
        {
            return a.Value > b.Value;
        }
        public static bool operator >(Float a, float b)
        {
            return a.Value > b;
        }
        public static bool operator >(float a, Float b)
        {
            return a > b.Value;
        }
        
        public static bool operator <(Float a, Float b)
        {
            return a.Value < b.Value;
        }
        public static bool operator <(Float a, float b)
        {
            return a.Value < b;
        }
        public static bool operator <(float a, Float b)
        {
            return a < b.Value;
        }
        
        public static bool operator >=(Float a, Float b)
        {
            return a.Value >= b.Value;
        }
        public static bool operator >=(Float a, float b)
        {
            return a.Value >= b;
        }
        public static bool operator >=(float a, Float b)
        {
            return a >= b.Value;
        }
        
        public static bool operator <=(Float a, Float b)
        {
            return a.Value <= b.Value;
        }
        public static bool operator <=(Float a, float b)
        {
            return a.Value <= b;
        }
        public static bool operator <=(float a, Float b)
        {
            return a <= b.Value;
        }
        
        public static bool operator !=(Float a, Float b)
        {
            return a.Value != b.Value;
        }
        public static bool operator !=(Float a, float b)
        {
            return a.Value != b;
        }
        public static bool operator !=(float a, Float b)
        {
            return a != b.Value;
        }
        
        public static bool operator ==(Float a, Float b)
        {
            return a.Value == b.Value;
        }
        public static bool operator ==(Float a, float b)
        {
            return a.Value == b;
        }
        public static bool operator ==(float a, Float b)
        {
            return a == b.Value;
        }
        
        #endregion // OPERATORS
        
        protected bool Equals(Float other)
        {
            return base.Equals(other) && _value.Equals(other._value) && _range.Equals(other._range);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            
            if (ReferenceEquals(this, obj))
                return true;
            
            if (obj.GetType() != GetType())
                return false;
            
            return Equals((Float) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ _value.GetHashCode();
                hashCode = (hashCode * 397) ^ _range.GetHashCode();
                return hashCode;
            }
        }
    }
}