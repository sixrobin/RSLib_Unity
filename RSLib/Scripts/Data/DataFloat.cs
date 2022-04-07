namespace RSLib
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Data Float", menuName = "RSLib/Data/Float")]
    public class DataFloat : ScriptableObject
    {
        [SerializeField] private float _value = 0f;

        public float Value => _value;
        
        #region OPERATORS

        public static bool operator >(DataFloat a, DataFloat b)
        {
            return a.Value > b.Value;
        }
        public static bool operator >(DataFloat a, float b)
        {
            return a.Value > b;
        }
        public static bool operator >(float a, DataFloat b)
        {
            return a > b.Value;
        }
        
        public static bool operator <(DataFloat a, DataFloat b)
        {
            return a.Value < b.Value;
        }
        public static bool operator <(DataFloat a, float b)
        {
            return a.Value < b;
        }
        public static bool operator <(float a, DataFloat b)
        {
            return a < b.Value;
        }
        
        public static bool operator >=(DataFloat a, DataFloat b)
        {
            return a.Value >= b.Value;
        }
        public static bool operator >=(DataFloat a, float b)
        {
            return a.Value >= b;
        }
        public static bool operator >=(float a, DataFloat b)
        {
            return a >= b.Value;
        }
        
        public static bool operator <=(DataFloat a, DataFloat b)
        {
            return a.Value <= b.Value;
        }
        public static bool operator <=(DataFloat a, float b)
        {
            return a.Value <= b;
        }
        public static bool operator <=(float a, DataFloat b)
        {
            return a <= b.Value;
        }
        
        public static bool operator !=(DataFloat a, DataFloat b)
        {
            return a.Value != b.Value;
        }
        public static bool operator !=(DataFloat a, float b)
        {
            return a.Value != b;
        }
        public static bool operator !=(float a, DataFloat b)
        {
            return a != b.Value;
        }
        
        public static bool operator ==(DataFloat a, DataFloat b)
        {
            return a.Value == b.Value;
        }
        public static bool operator ==(DataFloat a, float b)
        {
            return a.Value == b;
        }
        public static bool operator ==(float a, DataFloat b)
        {
            return a == b.Value;
        }
        
        #endregion // OPERATORS
        
        protected bool Equals(DataFloat other)
        {
            return base.Equals(other) && _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            
            if (ReferenceEquals(this, obj))
                return true;
            
            if (obj.GetType() != GetType())
                return false;
            
            return Equals((DataFloat) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ _value.GetHashCode();
            }
        }
    }
}