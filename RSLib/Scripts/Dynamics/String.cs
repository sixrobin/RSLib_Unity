namespace RSLib.Dynamics
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Dynamic String", menuName = "RSLib/Dynamics/String")]
    public class String : ScriptableObject
    {
        public struct ValueChangedEventArgs
        {
            public string Previous;
            public string New;
        }

        [SerializeField] private string _value = string.Empty;

        public delegate void ValueChangedEventHandler(ValueChangedEventArgs args);
        public event ValueChangedEventHandler ValueChanged;

        public string Value
        {
            get => _value;
            set
            {
                ValueChangedEventArgs valueChangedEventArgs = new ValueChangedEventArgs()
                {
                    Previous = _value,
                    New = value
                };

                _value = value;
                ValueChanged?.Invoke(valueChangedEventArgs);
            }
        }
    }
}