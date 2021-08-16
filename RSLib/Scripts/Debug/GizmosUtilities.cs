namespace RSLib.Debug
{
    using RSLib.Extensions;
    using System.Linq;

    public static class GizmosUtilities
    {
        /// <summary>
        /// Variant of Gizmos.DrawLine allowing to draw a dotted line. Uses the Gizmos.Color that is currently set without changing it
        /// Last dot can have a various length but it is for precision purpose, so that it always ends precisely to the given target position.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="from">Starting position.</param>
        /// <param name="to">Target position.</param>
        /// <param name="dotLength">Dot length. Spacing between dots will be twice this value.</param>
        public static void DrawDottedLine(UnityEngine.Vector3 from, UnityEngine.Vector3 to, float dotLength = 0.05f)
        {
            float fullLength = (to - from).magnitude;
            UnityEngine.Vector3 normalizedDir = (to - from).normalized;

            int dotsCount = UnityEngine.Mathf.RoundToInt(fullLength / dotLength);
            float dotsSpacing = fullLength / (dotsCount - 1);

            for (int i = 0; i < dotsCount; i += 3)
            {
                UnityEngine.Gizmos.DrawLine(
                    from + i * dotsSpacing * normalizedDir,
                    i >= dotsCount - 4 ? to : from + (i + 1) * dotsSpacing * normalizedDir);
            }
        }

        /// <summary>
        /// Draws a line representing the path joining Vector2s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector2 to draw a path of.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<UnityEngine.Vector2> points, bool cyclic = true, bool dotted = false)
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
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<UnityEngine.Vector2> points, UnityEngine.Vector2 offset, bool cyclic = true, bool dotted = false)
        {
            DrawVectorsPath(points.ToVector3Array(), offset, cyclic);
        }

        /// <summary>
        /// Draws a line representing the path joining Vector3s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector3 to draw a path of.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<UnityEngine.Vector3> points, bool cyclic = true, bool dotted = false)
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
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<UnityEngine.Vector3> points, UnityEngine.Vector3 offset, bool cyclic = true, bool dotted = false)
        {
            UnityEngine.Vector3[] pointsArray = points.ToArray();

            for (int i = pointsArray.Length - 1; i >= 1; --i)
            {
                if (dotted)
                    UnityEngine.Gizmos.DrawLine(pointsArray[i] + offset, pointsArray[i - 1] + offset);
                else
                    DrawDottedLine(pointsArray[i] + offset, pointsArray[i - 1] + offset);
            }

            if (cyclic)
            {
                if (dotted)
                    UnityEngine.Gizmos.DrawLine(pointsArray[0] + offset, pointsArray[pointsArray.Length - 1] + offset);
                else
                    DrawDottedLine(pointsArray[0] + offset, pointsArray[pointsArray.Length - 1] + offset);
            }
        }
    }
}