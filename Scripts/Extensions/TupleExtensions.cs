namespace RSLib.Extensions
{
    using UnityEngine;

    public static class TupleExtensions
    {
        /// <summary>
        /// Converts a Tuple of 2 floats to a UnityEngine.Vector2 struct.
        /// </summary>
        /// <param name="tuple">Reference tuple.</param>
        /// <returns>Tuple in UnityEngine.Vector2 format.</returns>
        public static Vector2 ToVector2(this System.Tuple<float, float> tuple)
        {
            return new Vector2(tuple.Item1, tuple.Item2);
        }
        
        /// <summary>
        /// Converts a Tuple of 2 doubles to a UnityEngine.Vector2 struct.
        /// </summary>
        /// <param name="tuple">Reference tuple.</param>
        /// <returns>Tuple in UnityEngine.Vector2 format.</returns>
        public static Vector2 ToVector2(this System.Tuple<double, double> tuple)
        {
            return new Vector2((float)tuple.Item1, (float)tuple.Item2);
        }
        
        /// <summary>
        /// Converts a Tuple of 2 floats to a UnityEngine.Vector3 struct.
        /// </summary>
        /// <param name="tuple">Reference tuple.</param>
        /// <param name="useY">Should the second item of the Tuple be applied to Y or Z component of the vector.</param>
        /// <returns>Tuple in UnityEngine.Vector3 format.</returns>
        public static Vector3 ToVector3(this System.Tuple<float, float> tuple, bool useY)
        {
            return new Vector3(tuple.Item1, useY ? tuple.Item2 : 0f, useY ? 0f : tuple.Item2);
        }
        
        /// <summary>
        /// Converts a Tuple of 2 doubles to a UnityEngine.Vector3 struct.
        /// </summary>
        /// <param name="tuple">Reference tuple.</param>
        /// <param name="useY">Should the second item of the Tuple be applied to Y or Z component of the vector.</param>
        /// <returns>Tuple in UnityEngine.Vector3 format.</returns>
        public static Vector3 ToVector3(this System.Tuple<double, double> tuple, bool useY)
        {
            return new Vector3((float)tuple.Item1, useY ? (float)tuple.Item2 : 0f, useY ? 0f : (float)tuple.Item2);
        }
        
        /// <summary>
        /// Converts a Tuple of 3 floats to a UnityEngine.Vector3 struct.
        /// </summary>
        /// <param name="tuple">Reference tuple.</param>
        /// <returns>Tuple in UnityEngine.Vector3 format.</returns>
        public static Vector3 ToVector3(this System.Tuple<float, float, float> tuple)
        {
            return new Vector3(tuple.Item1, tuple.Item2, tuple.Item3);
        }
        
        /// <summary>
        /// Converts a Tuple of 3 doubles to a UnityEngine.Vector3 struct.
        /// </summary>
        /// <param name="tuple">Reference tuple.</param>
        /// <returns>Tuple in UnityEngine.Vector3 format.</returns>
        public static Vector3 ToVector3(this System.Tuple<double, double, double> tuple)
        {
            return new Vector3((float)tuple.Item1, (float)tuple.Item2, (float)tuple.Item3);
        }
    }
}
