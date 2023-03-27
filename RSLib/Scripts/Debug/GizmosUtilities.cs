namespace RSLib.Debug
{
    using System.Linq;
    using UnityEngine;

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
        public static void DrawDottedLine(Vector3 from, Vector3 to, float dotLength = 0.05f)
        {
            float fullLength = (to - from).magnitude;
            Vector3 normalizedDir = (to - from).normalized;

            int dotsCount = Mathf.RoundToInt(fullLength / dotLength);
            float dotsSpacing = fullLength / (dotsCount - 1);

            for (int i = 0; i < dotsCount; i += 3)
            {
                Gizmos.DrawLine(
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
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<Vector2> points, bool cyclic = true, bool dotted = false)
        {
            DrawVectorsPath(points, Vector2.zero, cyclic, dotted);
        }

        /// <summary>
        /// Draws a line representing the path joining Vector2s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector2 to draw a path of.</param>
        /// <param name="offset">Offset applied to all points.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<Vector2> points, Vector2 offset, bool cyclic = true, bool dotted = false)
        {
            // Convert Vector2 collection to Vector3.
            System.Collections.Generic.List<Vector3> vectors = new System.Collections.Generic.List<Vector3>();
            foreach (Vector2 point in points)
                vectors.Add(point);

            DrawVectorsPath(vectors, offset, cyclic, dotted);
        }

        /// <summary>
        /// Draws a line representing the path joining Vector3s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector3 to draw a path of.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<Vector3> points, bool cyclic = true, bool dotted = false)
        {
            DrawVectorsPath(points, Vector3.zero, cyclic, dotted);
        }

        /// <summary>
        /// Draws a line representing the path joining Vector3s.
        /// This must be called only inside OnDrawGizmos or OnDrawGizmosSelected methods.
        /// </summary>
        /// <param name="points">Collection of Vector3 to draw a path of.</param>
        /// <param name="offset">Offset applied to all points.</param>
        /// <param name="cyclic">Should the first and the last points be joined together.</param>
        public static void DrawVectorsPath(System.Collections.Generic.IEnumerable<Vector3> points, Vector3 offset, bool cyclic = true, bool dotted = false)
        {
            Vector3[] pointsArray = points.ToArray();

            for (int i = pointsArray.Length - 1; i >= 1; --i)
            {
                if (dotted)
                    Gizmos.DrawLine(pointsArray[i] + offset, pointsArray[i - 1] + offset);
                else
                    DrawDottedLine(pointsArray[i] + offset, pointsArray[i - 1] + offset);
            }

            if (cyclic)
            {
                if (dotted)
                    Gizmos.DrawLine(pointsArray[0] + offset, pointsArray[pointsArray.Length - 1] + offset);
                else
                    DrawDottedLine(pointsArray[0] + offset, pointsArray[pointsArray.Length - 1] + offset);
            }
        }

        public static void DrawArrowHead(Vector3 position, Vector3 direction, float length = 0.25f, float angle = 45f)
        {
            if (direction == Vector3.zero)
                return;

            float theta = angle * 0.5f;
            
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-theta, 0f, 0f) * Vector3.back;
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(theta, 0f, 0f) * Vector3.back;
            Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, theta, 0f) * Vector3.back;
            Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, -theta, 0f) * Vector3.back;
            
            Gizmos.DrawRay(position + direction, left * length);
            Gizmos.DrawRay(position + direction, right * length);
            Gizmos.DrawRay(position + direction, up * length);
            Gizmos.DrawRay(position + direction, down * length);
        }
    }
}