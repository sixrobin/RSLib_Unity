namespace RSLib.Data
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Data Int", menuName = "RSLib/Data/Int")]
    public class Int : ScriptableObject
    {
        public struct ValueChangedEventArgs
        {
            public int Previous;
            public int New;
        }

        [SerializeField] private int _value = 0;
        [SerializeField] private Vector2Int _range = new Vector2Int(int.MinValue, int.MaxValue);

        public delegate void ValueChangedEventHandler(ValueChangedEventArgs args);
        public event ValueChangedEventHandler ValueChanged;

        public int Value
        {
            get => _value;
            set
            {
                int newValue = Mathf.Clamp(value, _range.x, _range.y);

                ValueChangedEventArgs valueChangedEventArgs = new ValueChangedEventArgs
                {
                    Previous = _value,
                    New = newValue
                };

                _value = newValue;
                ValueChanged?.Invoke(valueChangedEventArgs);
            }
        }

        public Vector2Int Range => _range;
        public int Min => Range.x;
        public int Max => Range.y;
        
        private void OnValidate()
        {
            Value = Mathf.Clamp(_value, _range.x, _range.y);
        }

        #region ARITHMETIC OPERATORS
        
        public static Int operator +(Int a, Int b)
        {
            a.Value += b.Value;
            return a;
        }
        public static Int operator +(Int a, int b)
        {
            a.Value += b;
            return a;
        }
        public static int operator +(int a, Int b)
        {
            a += b.Value;
            return a;
        }

        public static Int operator -(Int a, Int b)
        {
            a.Value -= b.Value;
            return a;
        }
        public static Int operator -(Int a, int b)
        {
            a.Value -= b;
            return a;
        }
        public static int operator -(int a, Int b)
        {
            a -= b.Value;
            return a;
        }

        public static Int operator *(Int a, Int b)
        {
            a.Value *= b.Value;
            return a;
        }
        public static Int operator *(Int a, int b)
        {
            a.Value *= b;
            return a;
        }
        public static int operator *(int a, Int b)
        {
            a *= b.Value;
            return a;
        }

        public static Int operator /(Int a, Int b)
        {
            a.Value /= b.Value;
            return a;
        }
        public static Int operator /(Int a, int b)
        {
            a.Value /= b;
            return a;
        }
        public static int operator /(int a, Int b)
        {
            a /= b.Value;
            return a;
        }

        public static Int operator %(Int a, Int b)
        {
            a.Value %= b.Value;
            return a;
        }
        public static Int operator %(Int a, int b)
        {
            a.Value %= b;
            return a;
        }
        public static int operator %(int a, Int b)
        {
            a %= b.Value;
            return a;
        }

        public static bool operator >(Int a, Int b)
        {
            return a.Value > b.Value;
        }
        public static bool operator >(Int a, int b)
        {
            return a.Value > b;
        }
        public static bool operator >(int a, Int b)
        {
            return a > b.Value;
        }
        
        public static bool operator <(Int a, Int b)
        {
            return a.Value < b.Value;
        }
        public static bool operator <(Int a, int b)
        {
            return a.Value < b;
        }
        public static bool operator <(int a, Int b)
        {
            return a < b.Value;
        }
        
        public static bool operator >=(Int a, Int b)
        {
            return a.Value >= b.Value;
        }
        public static bool operator >=(Int a, int b)
        {
            return a.Value >= b;
        }
        public static bool operator >=(int a, Int b)
        {
            return a >= b.Value;
        }
        
        public static bool operator <=(Int a, Int b)
        {
            return a.Value <= b.Value;
        }
        public static bool operator <=(Int a, int b)
        {
            return a.Value <= b;
        }
        public static bool operator <=(int a, Int b)
        {
            return a <= b.Value;
        }
        
        public static bool operator !=(Int a, Int b)
        {
            return a.Value != b.Value;
        }
        public static bool operator !=(Int a, int b)
        {
            return a.Value != b;
        }
        public static bool operator !=(int a, Int b)
        {
            return a != b.Value;
        }
        
        public static bool operator ==(Int a, Int b)
        {
            return a.Value == b.Value;
        }
        public static bool operator ==(Int a, int b)
        {
            return a.Value == b;
        }
        public static bool operator ==(int a, Int b)
        {
            return a == b.Value;
        }
        
        #endregion // ARITHMETIC OPERATORS
        
        #region CONVERSION OPERATORS
        
        public static implicit operator int(Int dataInt)
        {
            return dataInt.Value;
        }
        
        #endregion // CONVERSION OPERATORS

        protected bool Equals(Int other)
        {
            return base.Equals(other) && _value == other._value && _range.Equals(other._range);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            
            if (ReferenceEquals(this, obj))
                return true;
            
            if (obj.GetType() != GetType())
                return false;
            
            return Equals((Int) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ _value;
                hashCode = (hashCode * 397) ^ _range.GetHashCode();
                return hashCode;
            }
        }
    }
}