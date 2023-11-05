namespace RSLib.Framework.Pooling
{
    public static class ResourcesPool<T> where T : UnityEngine.Object
    {
        private static readonly System.Collections.Generic.Dictionary<string, T> RESOURCES = new System.Collections.Generic.Dictionary<string, T>();
        private static readonly System.Collections.Generic.Dictionary<string, T[]> RESOURCES_ALL = new System.Collections.Generic.Dictionary<string, T[]>();

        /// <summary>
        /// Caches asset at a given path starting in Resources folder.
        /// </summary>
        /// <param name="path">Asset path.</param>
        public static void Cache(string path)
        {
            if (!RESOURCES.ContainsKey(path))
                RESOURCES.Add(path, UnityEngine.Resources.Load<T>(path));
        }

        /// <summary>
        /// Caches all assets in a folder at a given path starting in Resources folder.
        /// </summary>
        /// <param name="path">Asset path.</param>
        public static void CacheAll(string path)
        {
            if (!RESOURCES_ALL.ContainsKey(path))
                RESOURCES_ALL.Add(path, UnityEngine.Resources.LoadAll<T>(path));
        }

        /// <summary>
        /// Clears loaded assets in pool, without unloading them.
        /// </summary>
        public static void Clear()
        {
            RESOURCES.Clear();
            RESOURCES_ALL.Clear();
        }

        /// <summary>
        /// Loads asset at a given path starting in Resources folder and returns it.
        /// </summary>
        /// <param name="path">Asset path.</param>
        /// <returns>Loaded asset if it has been found.</returns>
        public static T Load(string path)
        {
            if (RESOURCES.TryGetValue(path, out T resource))
                return resource;

            RESOURCES.Add(path, UnityEngine.Resources.Load<T>(path));
            return RESOURCES[path];
        }

        /// <summary>
        /// Loads all assets in a folder at a given path starting in Resources folder and returns them.
        /// </summary>
        /// <param name="path">Assets folder path.</param>
        /// <returns>Loaded assets if folder has been found.</returns>
        public static T[] LoadAll(string path)
        {
            if (RESOURCES_ALL.TryGetValue(path, out T[] resources))
                return resources;

            RESOURCES_ALL.Add(path, UnityEngine.Resources.LoadAll<T>(path));
            return RESOURCES_ALL[path];
        }

        /// <summary>
        /// Removes already loaded path from pool, reloads it at path starting in Resources folder, and returns it.
        /// </summary>
        /// <param name="path">Asset path.</param>
        /// <returns>Loaded asset if it has been found.</returns>
        public static T Reload(string path)
        {
            if (RESOURCES.ContainsKey(path))
                RESOURCES.Remove(path);

            return Load(path);
        }

        /// <summary>
        /// Removes already loaded assets path from pool, reloads them at path starting in Resources folder, and returns them.
        /// </summary>
        /// <param name="path">Assets folder path.</param>
        /// <returns>Loaded assets if folder has been found.</returns>
        public static T[] ReloadAll(string path)
        {
            if (RESOURCES_ALL.ContainsKey(path))
                RESOURCES_ALL.Remove(path);

            return LoadAll(path);
        }
    }

    public static class ResourcesPool
    {
        /// <summary>
        /// Caches asset at a given path starting in Resources folder.
        /// </summary>
        /// <param name="path">Asset path.</param>
        public static void Cache<T>(string path) where T : UnityEngine.Object
        {
            ResourcesPool<T>.Cache(path);
        }

        /// <summary>
        /// Caches all assets in a folder at a given path starting in Resources folder.
        /// </summary>
        /// <param name="path">Asset path.</param>
        public static void CacheAll<T>(string path) where T : UnityEngine.Object
        {
            ResourcesPool<T>.CacheAll(path);
        }

        /// <summary>
        /// Clears loaded assets in pool, without unloading them.
        /// </summary>
        public static void Clear<T>() where T : UnityEngine.Object
        {
            ResourcesPool<T>.Clear();
        }

        /// <summary>
        /// Loads asset at a given path starting in Resources folder and returns it.
        /// </summary>
        /// <param name="path">Asset path.</param>
        /// <returns>Loaded asset if it has been found.</returns>
        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            return ResourcesPool<T>.Load(path);
        }

        /// <summary>
        /// Loads all assets in a folder at a given path starting in Resources folder and returns them.
        /// </summary>
        /// <param name="path">Assets folder path.</param>
        /// <returns>Loaded assets if folder has been found.</returns>
        public static T[] LoadAll<T>(string path) where T : UnityEngine.Object
        {
            return ResourcesPool<T>.LoadAll(path);
        }

        /// <summary>
        /// Removes already loaded path from pool, reloads it at path starting in Resources folder, and returns it.
        /// </summary>
        /// <param name="path">Asset path.</param>
        /// <returns>Loaded asset if it has been found.</returns>
        public static T Reload<T>(string path) where T : UnityEngine.Object
        {
            return ResourcesPool<T>.Reload(path);
        }

        /// <summary>
        /// Removes already loaded assets path from pool, reloads them at path starting in Resources folder, and returns them.
        /// </summary>
        /// <param name="path">Assets folder path.</param>
        /// <returns>Loaded assets if folder has been found.</returns>
        public static T[] ReloadAll<T>(string path) where T : UnityEngine.Object
        {
            return ResourcesPool<T>.ReloadAll(path);
        }
    }
}