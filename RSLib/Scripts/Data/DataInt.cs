namespace RSLib
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Data Int", menuName = "RSLib/Data/Int")]
    public class DataInt : ScriptableObject
    {
        [SerializeField] private int _value = 0;

        public int Value => _value;
        
        #region OPERATORS

        public static bool operator >(DataInt a, DataInt b)
        {
            return a.Value > b.Value;
        }
        public static bool operator >(DataInt a, int b)
        {
            return a.Value > b;
        }
        public static bool operator >(int a, DataInt b)
        {
            return a > b.Value;
        }
        
        public static bool operator <(DataInt a, DataInt b)
        {
            return a.Value < b.Value;
        }
        public static bool operator <(DataInt a, int b)
        {
            return a.Value < b;
        }
        public static bool operator <(int a, DataInt b)
        {
            return a < b.Value;
        }
        
        public static bool operator >=(DataInt a, DataInt b)
        {
            return a.Value >= b.Value;
        }
        public static bool operator >=(DataInt a, int b)
        {
            return a.Value >= b;
        }
        public static bool operator >=(int a, DataInt b)
        {
            return a >= b.Value;
        }
        
        public static bool operator <=(DataInt a, DataInt b)
        {
            return a.Value <= b.Value;
        }
        public static bool operator <=(DataInt a, int b)
        {
            return a.Value <= b;
        }
        public static bool operator <=(int a, DataInt b)
        {
            return a <= b.Value;
        }
        
        public static bool operator !=(DataInt a, DataInt b)
        {
            return a.Value != b.Value;
        }
        public static bool operator !=(DataInt a, int b)
        {
            return a.Value != b;
        }
        public static bool operator !=(int a, DataInt b)
        {
            return a != b.Value;
        }
        
        public static bool operator ==(DataInt a, DataInt b)
        {
            return a.Value == b.Value;
        }
        public static bool operator ==(DataInt a, int b)
        {
            return a.Value == b;
        }
        public static bool operator ==(int a, DataInt b)
        {
            return a == b.Value;
        }
        
        #endregion // OPERATORS
        
        protected bool Equals(DataInt other)
        {
            return base.Equals(other) && _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            
            if (ReferenceEquals(this, obj))
                return true;
            
            if (obj.GetType() != GetType())
                return false;
            
            return Equals((DataInt) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ _value;
            }
        }
    }
}