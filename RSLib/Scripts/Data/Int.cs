namespace RSLib.Data
{
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    [CreateAssetMenu(fileName = "New Data Int", menuName = "RSLib/Data/Int", order = -100)]
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
    
    [System.Serializable]
    public class IntField
    {
        [SerializeField] private Int _dataInt = null;
        [SerializeField] private int _valueInt = 0;
        [SerializeField] private bool _useDataInt = true;

        public int Value => _useDataInt ? _dataInt : _valueInt;
        
        public void Set(int value)
        {
            if (_useDataInt) _dataInt.Value = value;
            else _valueInt = value;
        }
        public void Set(Int dataInt)
        {
            if (_useDataInt) _dataInt.Value = dataInt;
            else _valueInt = dataInt;
        }
        public void Set(IntField intField)
        {
            if (_useDataInt) _dataInt.Value = intField;
            else _valueInt = intField;
        }
        
        #region ARITHMETIC OPERATORS
        
        public static IntField operator +(IntField a, IntField b)
        {
            if (a._useDataInt) a._dataInt += b.Value;
            else a._valueInt += b.Value;
            return a;
        }
        public static IntField operator +(IntField a, int b)
        {
            if (a._useDataInt) a._dataInt += b;
            else a._valueInt += b;
            return a;
        }
        public static int operator +(int a, IntField b)
        {
            a += b.Value;
            return a;
        }

        public static IntField operator -(IntField a, IntField b)
        {
            if (a._useDataInt) a._dataInt -= b.Value;
            else a._valueInt -= b.Value;
            return a;
        }
        public static IntField operator -(IntField a, int b)
        {
            if (a._useDataInt) a._dataInt -= b;
            else a._valueInt -= b;
            return a;
        }
        public static int operator -(int a, IntField b)
        {
            a -= b.Value;
            return a;
        }

        public static IntField operator *(IntField a, IntField b)
        {
            if (a._useDataInt) a._dataInt *= b.Value;
            else a._valueInt *= b.Value;
            return a;
        }
        public static IntField operator *(IntField a, int b)
        {
            if (a._useDataInt) a._dataInt *= b;
            else a._valueInt *= b;
            return a;
        }
        public static int operator *(int a, IntField b)
        {
            a *= b.Value;
            return a;
        }

        public static IntField operator /(IntField a, IntField b)
        {
            if (a._useDataInt) a._dataInt /= b.Value;
            else a._valueInt /= b.Value;
            return a;
        }
        public static IntField operator /(IntField a, int b)
        {
            if (a._useDataInt) a._dataInt /= b;
            else a._valueInt /= b;
            return a;
        }
        public static int operator /(int a, IntField b)
        {
            a /= b.Value;
            return a;
        }

        public static IntField operator %(IntField a, IntField b)
        {
            if (a._useDataInt) a._dataInt %= b.Value;
            else a._valueInt %= b.Value;
            return a;
        }
        public static IntField operator %(IntField a, int b)
        {
            if (a._useDataInt) a._dataInt %= b;
            else a._valueInt %= b;
            return a;
        }
        public static int operator %(int a, IntField b)
        {
            a %= b.Value;
            return a;
        }

        public static bool operator >(IntField a, IntField b)
        {
            return a.Value > b.Value;
        }
        public static bool operator >(IntField a, int b)
        {
            return a.Value > b;
        }
        public static bool operator >(int a, IntField b)
        {
            return a > b.Value;
        }
        
        public static bool operator <(IntField a, IntField b)
        {
            return a.Value < b.Value;
        }
        public static bool operator <(IntField a, int b)
        {
            return a.Value < b;
        }
        public static bool operator <(int a, IntField b)
        {
            return a < b.Value;
        }
        
        public static bool operator >=(IntField a, IntField b)
        {
            return a.Value >= b.Value;
        }
        public static bool operator >=(IntField a, int b)
        {
            return a.Value >= b;
        }
        public static bool operator >=(int a, IntField b)
        {
            return a >= b.Value;
        }
        
        public static bool operator <=(IntField a, IntField b)
        {
            return a.Value <= b.Value;
        }
        public static bool operator <=(IntField a, int b)
        {
            return a.Value <= b;
        }
        public static bool operator <=(int a, IntField b)
        {
            return a <= b.Value;
        }
        
        #endregion // ARITHMETIC OPERATORS
        
        #region CONVERSION OPERATORS
        
        public static implicit operator int(IntField intField)
        {
            return intField.Value;
        }
        
        #endregion // CONVERSION OPERATORS
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(IntField))]
    public class IntFieldPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("_dataInt");
            return EditorGUI.GetPropertyHeight(valueProperty);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty useDataIntProperty = property.FindPropertyRelative("_useDataInt");
            SerializedProperty valueIntProperty = property.FindPropertyRelative("_valueInt");
            SerializedProperty dataIntProperty = property.FindPropertyRelative("_dataInt");

            position.width -= 24;

            EditorGUI.PropertyField(position,
                useDataIntProperty.boolValue ? dataIntProperty : valueIntProperty,
                label, 
                true);
            
            position.x += position.width + 24;
            position.width = EditorGUI.GetPropertyHeight(useDataIntProperty);
            position.height = position.width;
            position.x -= position.width;
            
            EditorGUI.PropertyField(position, useDataIntProperty, GUIContent.none);
        }
    }
#endif
}