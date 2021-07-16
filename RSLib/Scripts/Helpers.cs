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
        /// Draws a line representing the path joining Vector2s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector2 to draw a path of.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<UnityEngine.Vector2> points, bool cyclic = true)
        {
            DrawVectorsPath(points.ToVector3Array(), UnityEngine.Vector2.zero, cyclic);
        }

        /// <summary>
        /// Draws a line representing the path joining Vector2s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector2 to draw a path of.</param>
        /// <param name="offset">Offset applied to all points.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<UnityEngine.Vector2> points, UnityEngine.Vector2 offset, bool cyclic = true)
        {
            DrawVectorsPath(points.ToVector3Array(), offset, cyclic);
        }

        /// <summary>
        /// Draws a line representing the path joining Vector3s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector3 to draw a path of.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<UnityEngine.Vector3> points, bool cyclic = true)
        {
            DrawVectorsPath(points, UnityEngine.Vector3.zero, cyclic);
        }

        /// <summary>
        /// Draws a line representing the path joining Vector3s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector3 to draw a path of.</param>
        /// <param name="offset">Offset applied to all points.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<UnityEngine.Vector3> points, UnityEngine.Vector3 offset, bool cyclic = true)
        {
            UnityEngine.Vector3[] pointsArray = points.ToArray();
            for (int i = pointsArray.Length - 1; i >= 1; --i)
                UnityEngine.Gizmos.DrawLine(pointsArray[i] + offset, pointsArray[i - 1] + offset);

            if (cyclic)
                UnityEngine.Gizmos.DrawLine(pointsArray[0] + offset, pointsArray[pointsArray.Length - 1] + offset);
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