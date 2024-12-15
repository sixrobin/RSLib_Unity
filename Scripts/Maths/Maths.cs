namespace RSLib.Unity.Maths
{
    using System.Linq;
    
    public static class Maths
    {
        #region AVERAGE

        /// <summary>
        /// Computes the average position between Vector2s.
        /// This method uses Linq and a foreach loop, using an array would be better if possible.
        /// </summary>
        /// <param name="vectors">Collection of vectors.</param>
        /// <returns>Computed Vector2.</returns>
        public static UnityEngine.Vector2 ComputeAverageVector(System.Collections.Generic.IEnumerable<UnityEngine.Vector2> vectors)
        {
            UnityEngine.Vector2 average = UnityEngine.Vector2.zero;
            foreach (UnityEngine.Vector2 v in vectors)
                average += v;

            average /= vectors.Count();
            return average;
        }

        /// <summary>
        /// Computes the average position between Vector2s.
        /// </summary>
        /// <param name="vectors">Array of vectors, or multiple vectors as multiple arguments.</param>
        /// <returns>Computed Vector2.</returns>
        public static UnityEngine.Vector2 ComputeAverageVector(params UnityEngine.Vector2[] vectors)
        {
            UnityEngine.Vector2 average = UnityEngine.Vector2.zero;
            for (int i = vectors.Length - 1; i >= 0; --i)
                average += vectors[i];

            average /= vectors.Length;
            return average;
        }

        /// <summary>
        /// Computes the average position between Vector3s.
        /// This method uses Linq and a foreach loop, using an array would be better if possible.
        /// </summary>
        /// <param name="vectors">Collection of vectors.</param>
        /// <returns>Computed Vector3.</returns>
        public static UnityEngine.Vector3 ComputeAverageVector(System.Collections.Generic.IEnumerable<UnityEngine.Vector3> vectors)
        {
            UnityEngine.Vector3 average = UnityEngine.Vector3.zero;
            foreach (UnityEngine.Vector3 v in vectors)
                average += v;

            average /= vectors.Count();
            return average;
        }

        /// <summary>
        /// Computes the average position between Vector3s.
        /// </summary>
        /// <param name="vectors">Array of vectors, or multiple vectors as multiple arguments.</param>
        /// <returns>Computed Vector3.</returns>
        public static UnityEngine.Vector3 ComputeAverageVector(params UnityEngine.Vector3[] vectors)
        {
            UnityEngine.Vector3 average = UnityEngine.Vector3.zero;
            for (int i = vectors.Length - 1; i >= 0; --i)
                average += vectors[i];

            average /= vectors.Length;
            return average;
        }

        #endregion // AVERAGE
    }
}