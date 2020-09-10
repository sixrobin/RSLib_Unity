namespace RSLib.Framework
{
    public static class ResourcesPooler<T> where T : UnityEngine.Object
    {
        private static System.Collections.Generic.Dictionary<string, T> resources = new System.Collections.Generic.Dictionary<string, T>();
        private static System.Collections.Generic.Dictionary<string, T[]> resourcesAll = new System.Collections.Generic.Dictionary<string, T[]>();

        /// <summary>
        /// Clears loaded assets in pool, without unloading them.
        /// </summary>
        public static void Clear()
        {
            resources.Clear();
            resourcesAll.Clear();
        }

        /// <summary>Loads asset at a given path starting in Resources folder and returns it.</summary>
        /// <param name="path">Asset path.</param>
        /// <returns>Loaded asset if it has been found.</returns>
        public static T Load(string path)
        {
            if (resources.TryGetValue(path, out T resource))
            {
                return resource;
            }

            resources.Add(path, UnityEngine.Resources.Load<T>(path));
            return resources[path];
        }

        /// <summary>Loads all assets in a folder at a given path starting in Resources folder and returns them.</summary>
        /// <param name="path">Assets folder path.</param>
        /// <returns>Loaded assets if folder has been found.</returns>
        public static T[] LoadAll(string path)
        {
            if (resourcesAll.TryGetValue(path, out T[] resources))
            {
                return resources;
            }

            resourcesAll.Add(path, UnityEngine.Resources.LoadAll<T>(path));
            return resourcesAll[path];
        }

        /// <summary>Removes already loaded path from pool, reloads it at path starting in Resources folder, and returns it.</summary>
        /// <param name="path">Asset path.</param>
        /// <returns>Loaded asset if it has been found.</returns>
        public static T Reload(string path)
        {
            if (resources.ContainsKey(path))
            {
                resources.Remove(path);
            }

            return Load(path);
        }

        /// <summary>Removes already loaded assets path from pool, reloads them at path starting in Resources folder, and returns them.</summary>
        /// <param name="path">Assets folder path.</param>
        /// <returns>Loaded assets if folder has been found.</returns>
        public static T[] ReloadAll(string path)
        {
            if (resourcesAll.ContainsKey(path))
            {
                resourcesAll.Remove(path);
            }

            return LoadAll(path);
        }
    }
}