#if UNITY_EDITOR
namespace RSLib.EditorUtilities
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Simple abstract class used to help adding buttons in inspectors, for debug purpose.
    /// Child class still needs to specify the CustomEditor(typeof(T)) attribute.
    /// </summary>
    /// <typeparam name="T">Type of the object to draw a custom editor of.</typeparam>
    public abstract class ButtonProviderEditor<T> : Editor where T : Object
    {
        protected T Obj { get; private set; }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15f);

            GUILayout.Label("EDITOR UTILITIES", EditorStyles.boldLabel);
            DrawButtons();
        }

        protected virtual void OnEnable()
        {
            Obj = (T)target;
        }

        protected abstract void DrawButtons();

        protected virtual void DrawButton(string label, System.Action onClick)
        {
            if (onClick == null)
                throw new System.ArgumentNullException("onClick", "Inspector button must have an onClick action.");

            if (GUILayout.Button(label))
                onClick();
        }
    }
}
#endif