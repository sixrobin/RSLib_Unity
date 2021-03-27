namespace RSLib.Framework
{
    using UnityEngine;

    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool _dontDestroy = false;
        [SerializeField] private bool _verbose = false;

        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();
                    if (s_instance == null)
                        Debug.LogError($"No {typeof(T).Name} instance found in the scene to make a singleton.");
                }

                return s_instance;
            }

            set => s_instance = value;
        }

        protected bool IsValid => s_instance == this;

        public bool Verbose => _verbose;

        public static bool Exists()
        {
            return s_instance != null;
        }

        public static void Kill()
        {
            if (s_instance != null)
            {
                Destroy(s_instance.gameObject);
                s_instance = null;
            }
        }

        #region LOG

        public virtual void Log(string msg)
        {
            if (_verbose)
                Debug.Log($"{typeof(T).Name}: {msg}", gameObject);
        }

        public virtual void Log(string msg, Object context)
        {
            if (_verbose)
                Debug.Log($"{typeof(T).Name}: {msg}", context);
        }

        public virtual void LogError(string msg)
        {
            Debug.LogError($"{typeof(T).Name}: {msg}", gameObject);
        }

        public virtual void LogError(string msg, Object context)
        {
            Debug.LogError($"{typeof(T).Name}: {msg}", context);
        }

        public virtual void LogWarning(string msg)
        {
            Debug.LogWarning($"{typeof(T).Name}: {msg}", gameObject);
        }

        public virtual void LogWarning(string msg, Object context)
        {
            Debug.LogWarning($"{typeof(T).Name}: {msg}", context);
        }

        #endregion LOG

        protected virtual void Awake()
        {
            if (s_instance == null)
                s_instance = this as T;

            if (s_instance != this)
            {
                if (s_instance.gameObject == gameObject)
                    DestroyImmediate(this);
                else
                    DestroyImmediate(gameObject);
            }
            else
            {
                if (_dontDestroy)
                {
                    transform.SetParent(null);
                    DontDestroyOnLoad(gameObject);
                }
            }
        }
    }
}