﻿namespace RSLib.Extensions
{
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        #region GENERAL

        /// <summary>Loops through all KeyValuePairs in the dictionary and executes an action.</summary>
        /// <param name="action">Action to execute.</param>
        public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dict, System.Action<TKey, TValue> action)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in dict)
            {
                action(kvp.Key, kvp.Value);
            }
        }

        /// <summary>Checks the value for key TKey and returns it, or returns the default value for TValue if key was not found.</summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Found existing value, or default TValue value.</returns>
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue customDefault = default)
        {
            return dict.TryGetValue(key, out TValue value) ? value : customDefault;
        }

        /// <summary>Checks the value for key TKey and returns it, or creates a new pair if key was not found.</summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Found existing value, or created one.</returns>
        public static TValue GetOrInsertNew<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            TValue newObj = new TValue();
            dict[key] = newObj;
            return newObj;
        }

        #endregion GENERAL
    }
}