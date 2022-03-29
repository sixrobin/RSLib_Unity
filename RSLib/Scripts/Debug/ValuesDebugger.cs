namespace RSLib.Debug
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class AnchorExtensions
    {
        public static bool IsRight(this ValuesDebugger.Anchor anchor)
        {
            return anchor == ValuesDebugger.Anchor.LOWER_RIGHT || anchor == ValuesDebugger.Anchor.UPPER_RIGHT;
        }

        public static bool IsUp(this ValuesDebugger.Anchor anchor)
        {
            return anchor == ValuesDebugger.Anchor.UPPER_LEFT || anchor == ValuesDebugger.Anchor.UPPER_RIGHT;
        }
    }

    [DisallowMultipleComponent]
    public sealed class ValuesDebugger : Framework.Singleton<ValuesDebugger>
    {
        private const float LINE_HEIGHT = 20f;
        private const string VALUE_DEBUG_FORMAT = "{0}:{1}";

        [Header("GENERAL")]
        [SerializeField] private KeyCode _toggleKey = KeyCode.F1;
#pragma warning disable CS0414
        [SerializeField] private bool _editorOnly = true;
#pragma warning restore CS0414

        [Header("STYLE")]
        [SerializeField, Min(0f)] private float _margin = 0f;
        [SerializeField, Min(0f)] private float _linesHeight = 15f;
        [SerializeField] private Color _textsColor = Color.yellow;
        [SerializeField] private bool _boldFont = true;

        private Dictionary<Anchor, Dictionary<string, ValueGetter>> _values = new Dictionary<Anchor, Dictionary<string, ValueGetter>>();
        private Dictionary<Anchor, GUIStyle> _styles = new Dictionary<Anchor, GUIStyle>();

        private bool _enabled;

        public delegate object ValueGetter();

        public enum Anchor
        {
            UPPER_RIGHT,
            UPPER_LEFT,
            LOWER_RIGHT,
            LOWER_LEFT
        }

        public static void DebugValue(string key, ValueGetter valueGetter, Anchor anchor = Anchor.UPPER_RIGHT)
        {
            if (!Instance._values.ContainsKey(anchor))
                Instance._values.Add(anchor, new Dictionary<string, ValueGetter>());

            if (Instance._values[anchor].ContainsKey(key))
                Instance._values[anchor][key] = valueGetter;
            else
                Instance._values[anchor].Add(key, valueGetter);
        }

        private static void ClearValues()
        {
            foreach (KeyValuePair<Anchor, Dictionary<string, ValueGetter>> values in Instance._values)
            {
                string[] keys = values.Value.Keys.ToArray();
                for (int i = keys.Length - 1; i >= 0; --i)
                {
                    object target = values.Value[keys[i]].Target;
                    if (target == null || (target is Object && target.Equals(null)))
                        values.Value.Remove(keys[i]);
                }
            }
        }

        private void InitGUIStyles()
        {
            GUIStyle upperLeftStyle = new GUIStyle()
            {
                fontStyle = _boldFont ? FontStyle.Bold : FontStyle.Normal,
                alignment = TextAnchor.UpperLeft
            };

            GUIStyle upperRightStyle = new GUIStyle()
            {
                fontStyle = _boldFont ? FontStyle.Bold : FontStyle.Normal,
                alignment = TextAnchor.UpperRight
            };

            GUIStyle lowerLeftStyle = new GUIStyle()
            {
                fontStyle = _boldFont ? FontStyle.Bold : FontStyle.Normal,
                alignment = TextAnchor.LowerLeft
            };

            GUIStyle lowerRightStyle = new GUIStyle()
            {
                fontStyle = _boldFont ? FontStyle.Bold : FontStyle.Normal,
                alignment = TextAnchor.LowerRight
            };

            upperLeftStyle.normal.textColor = _textsColor;
            upperRightStyle.normal.textColor = _textsColor;
            lowerLeftStyle.normal.textColor = _textsColor;
            lowerRightStyle.normal.textColor = _textsColor;

            _styles = new Dictionary<Anchor, GUIStyle>()
            {
                { Anchor.UPPER_LEFT, upperLeftStyle },
                { Anchor.UPPER_RIGHT, upperRightStyle },
                { Anchor.LOWER_LEFT, lowerLeftStyle },
                { Anchor.LOWER_RIGHT, lowerRightStyle }
            };
        }

        protected override void Awake()
        {
            base.Awake();
            InitGUIStyles();
        }

        private void Update()
        {
#if !UNITY_EDITOR
            if (_editorOnly)
                return;
#endif
            if (Input.GetKeyDown(_toggleKey))
                _enabled = !_enabled;
        }

        private void OnGUI()
        {
            if (!_enabled)
                return;

            ClearValues();

            foreach (KeyValuePair<Anchor, Dictionary<string, ValueGetter>> values in _values)
            {
                Anchor anchor = values.Key;
                int i = 0;
                
                foreach (KeyValuePair<string, ValueGetter> entry in values.Value)
                {
                    Vector2 rectPos = new Vector2()
                    {
                        x = _margin,
                        y = anchor.IsUp() ? (_linesHeight * i + _margin) : (Screen.height - _linesHeight * i - _margin - LINE_HEIGHT)
                    };

                    Vector2 rectSize = new Vector2()
                    {
                        x = Screen.width - (_margin * 2),
                        y = LINE_HEIGHT
                    };

                    Rect rect = new Rect(rectPos, rectSize);
                    GUI.TextField(rect, string.Format(VALUE_DEBUG_FORMAT, entry.Key, entry.Value().ToString()), _styles[values.Key]);

                    i++;
                }
            }
        }

        private void OnValidate()
        {
            InitGUIStyles();
        }
    }
}