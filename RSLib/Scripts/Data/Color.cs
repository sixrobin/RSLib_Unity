namespace RSLib.Data
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Data Color", menuName = "RSLib/Data/Color")]
    public class Color : ScriptableObject
    {
        [SerializeField] private UnityEngine.Color _color = UnityEngine.Color.white;

        public static UnityEngine.Color Default => UnityEngine.Color.magenta;
        
        public string HexCode => ColorUtility.ToHtmlStringRGBA(_color);
        
        public static implicit operator UnityEngine.Color(Color color)
        {
            return color._color;
        }
    }
}