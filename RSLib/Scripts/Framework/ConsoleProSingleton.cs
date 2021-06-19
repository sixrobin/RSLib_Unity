namespace RSLib.Framework
{
    using UnityEngine;

    /// <summary>
    /// Child class of Singleton to handle Console Pro plugin prefix system.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DisallowMultipleComponent]
    public class ConsoleProSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        [SerializeField] private string _overrideLogPrefix = string.Empty;

        private string Prefix => string.IsNullOrEmpty(_overrideLogPrefix) ? GetType().Name : _overrideLogPrefix;

        public override void Log(string msg, bool forceVerbose = false)
        {
            if (Verbose || forceVerbose)
                Debug.Log($"#{Prefix}#{msg}", gameObject);
        }

        public override void Log(string msg, Object context, bool forceVerbose = false)
        {
            if (Verbose || forceVerbose)
                Debug.Log($"#{Prefix}#{msg}", context);
        }

        public override void LogWarning(string msg)
        {
            Debug.LogWarning($"#{Prefix}#{msg}", gameObject);
        }

        public override void LogWarning(string msg, Object context)
        {
            Debug.LogWarning($"#{Prefix}#{msg}", context);
        }

        public override void LogError(string msg)
        {
            Debug.LogWarning($"#{Prefix}#{msg}", gameObject);
        }

        public override void LogError(string msg, Object context)
        {
            Debug.LogWarning($"#{Prefix}#{msg}", context);
        }
    }
}