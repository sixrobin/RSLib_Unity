namespace RSLib
{
    using Extensions;
    using System.Linq;

    public static class Helpers
    {
        private static System.Random s_rnd = new System.Random();

        #region BOOLEAN

        /// <summary>Computes a random boolean.</summary>
        /// <returns>Computed boolean.</returns>
        public static bool CoinFlip()
        {
            return s_rnd.Next(2) == 0;
        }

        /// <summary>Computes a random boolean using a weight.</summary>
        /// <param name="percentage01">Chances of returning true, between 0 and 1.</param>
        /// <returns>Computed boolean.</returns>
        public static bool CoinFlip(float percentage01)
        {
            return s_rnd.Next(101) < percentage01 * 100f;
        }

        #endregion BOOLEAN

        #region ENUM

        /// <summary>Computes all values of a given Enum type into an array.</summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>Array with all Enum values.</returns>
        public static T[] GetEnumValues<T>() where T : System.Enum
        {
            return System.Enum.GetValues(typeof(T)) as T[];
        }

        /// <summary>Computes all values of a given Enum type into an array of their integer values.</summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>Array with all integer values.</returns>
        public static int[] GetEnumIntValues<T>() where T : System.Enum
        {
            return System.Enum.GetValues(typeof(T)) as int[];
        }

        #endregion ENUM

        #region FIND

        /// <summary>
        /// Same method as UnityEngine.Object.FindObjectsOfType<T> that looks for every MonoBehaviour and then filters by
        /// the specified type, allowing it to also find instances of an interface.
        /// This method is slower than UnityEngine.Object.FindObjectsOfType<T>, so use it very carefully or for editor purpose.
        /// </summary>
        /// <typeparam name="T">Type to look for.</typeparam>
        /// <returns>IEnumerable containing the instances of the searched type.</returns>
        public static System.Collections.Generic.IEnumerable<T> FindInstancesOfType<T>()
        {
            return UnityEngine.Object.FindObjectsOfType<UnityEngine.MonoBehaviour>().OfType<T>();
        }

        #endregion

        #region MISC

        /// <summary>
        /// Computes the average position between transforms.
        /// This method uses Linq and a foreach loop, using an array would be better if possible.
        /// </summary>
        /// <param name="transforms">Collection of transforms.</param>
        /// <returns>Computed position as a new Vector3.</returns>
        public static UnityEngine.Vector3 ComputeAveragePosition(System.Collections.Generic.IEnumerable<UnityEngine.Transform> transforms)
        {
            UnityEngine.Vector3 average = UnityEngine.Vector3.zero;

            foreach (UnityEngine.Transform t in transforms)
                average += t.position;

            average /= transforms.Count();
            return average;
        }

        /// <summary>Computes the average position between transforms.</summary>
        /// <param name="vectors">Array of transforms, or multiple transforms as multiple arguments.</param>
        /// <returns>Computed position as a new Vector3.</returns>
        public static UnityEngine.Vector3 ComputeAveragePosition(params UnityEngine.Transform[] transforms)
        {
            UnityEngine.Vector3 average = UnityEngine.Vector3.zero;

            for (int vectorIndex = transforms.Length - 1; vectorIndex >= 0; --vectorIndex)
                average += transforms[vectorIndex].position;

            average /= transforms.Length;
            return average;
        }

        /// <summary>Checks if an element equals at least one in a list of elements.</summary>
        /// <param name="source">Element to check.</param>
        /// <param name="list">Elements to compare.</param>
        /// <returns>True if one of the elements is the list equals the checked one.</returns>
        public static bool In<T>(this T source, params T[] list)
        {
            for (int i = list.Length - 1; i >= 0; --i)
                if (list[i].Equals(source))
                    return true;

            return false;
        }

        /// <summary>
        /// Scans a collection, looking for duplicate values. If any is found, the value and the number of occurences will be logged to the console.
        /// This method should only be used for editor purpose as it lacks optimization but only logs.
        /// </summary>
        /// <param name="collection">Collection to scan.</param>
        public static void ScanDuplicates<T>(this System.Collections.Generic.IEnumerable<T> collection)
        {
            System.Collections.Generic.Dictionary<T, int> duplicatas = collection
                .GroupBy(o => o)
                .Where(o => o.Count() > 1)
                .ToDictionary(o => o.Key, o => o.Count());

            if (duplicatas.Count == 0)
                UnityEngine.Debug.Log($"No duplicate has been found in the collection.");
            else
                foreach (System.Collections.Generic.KeyValuePair<T, int> duplicata in duplicatas)
                    UnityEngine.Debug.Log($"Value {duplicata.Key} has been found {duplicata.Value} times in the collection.");
        }

        #endregion MISC

        #region MODULO

        /// <summary>Custom modulo operating method to handle negative values.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="n">Second operand.</param>
        /// <returns>Modulo result.</returns>
        public static int Mod(int a, int n)
        {
            return (a % n + n) % n;
        }

        #endregion
    }
}