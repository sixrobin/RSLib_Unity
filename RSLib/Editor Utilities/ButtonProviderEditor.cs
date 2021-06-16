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
            GUILayout.Space(10f);
            DrawButtons();
        }

        protected virtual void OnEnable()
        {
            Obj = (T)target;
        }

        protected abstract void DrawButtons();
    }
}
#endif