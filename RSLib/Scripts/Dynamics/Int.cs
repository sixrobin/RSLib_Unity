namespace RSLib.Dynamics
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Dynamic Int", menuName = "RSLib/Dynamics/Int")]
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

                ValueChangedEventArgs valueChangedEventArgs = new ValueChangedEventArgs()
                {
                    Previous = _value,
                    New = newValue
                };

                _value = newValue;
                ValueChanged?.Invoke(valueChangedEventArgs);
            }
        }

        private void OnValidate()
        {
            _value = Mathf.Clamp(_value, _range.x, _range.y);
        }

        #region OPERATORS

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

        #endregion OPERATORS
    }
}