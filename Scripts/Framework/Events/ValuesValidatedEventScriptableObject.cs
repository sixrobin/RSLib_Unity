namespace RSLib.Unity.Framework.Events
{
    using UnityEngine;

    /// <summary>
    /// ScriptableObject subclass triggering an event in OnValidate event function.
    /// </summary>
    public abstract class ValuesValidatedEventScriptableObject : ScriptableObject
    {
        public delegate void ValuesValidatedEventHandler();
        public event ValuesValidatedEventHandler ValuesValidated;

        protected virtual void OnValidate()
        {
            ValuesValidated?.Invoke();
        }
    }
}