namespace RSLib.Framework
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private bool dontDestroy = false;

        [SerializeField]
        private bool verbose = false;

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        Debug.LogError($"No {typeof(T).Name} instance found in the scene to make a singleton.");
                    }
                }

                return instance;
            }

            set => instance = value;
        }

        public bool Verbose => this.verbose;

        public static bool Exists()
        {
            return Instance != null;
        }

        public static void Kill()
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
                instance = null;
            }
        }

        #region LOG

        public void Log(string message)
        {
            if (this.verbose)
            {
                Debug.Log($"{typeof(T).Name}: {message}");
            }
        }

        public void Log(string message, Object context)
        {
            if (this.verbose)
            {
                Debug.Log($"{typeof(T).Name}: {message}", context);
            }
        }

        public void LogError(string message)
        {
            Debug.LogError($"{typeof(T).Name}: {message}");
        }

        public void LogError(string message, Object context)
        {
            Debug.LogError($"{typeof(T).Name}: {message}", context);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning($"{typeof(T).Name}: {message}");
        }

        public void LogWarning(string message, Object context)
        {
            Debug.LogWarning($"{typeof(T).Name}: {message}", context);
        }

        #endregion LOG

        protected virtual void Awake()
        {
            if (instance == null)
            {
                Instance = this as T;

                if (this.dontDestroy)
                {
                    this.transform.SetParent(null);
                    DontDestroyOnLoad(this.gameObject);
                }
            }

            if (instance != this)
            {
                if (instance.gameObject == this.gameObject)
                {
                    DestroyImmediate(this);
                }
                else
                {
                    DestroyImmediate(this.gameObject);
                }
            }
        }
    }
}