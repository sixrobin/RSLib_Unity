namespace RSLib.Extensions
{
    using UnityEngine;

    public static class CameraExtensions
    {
        public static Vector3? GetPlaneIntersectionFromViewportRay(this Camera camera, Vector3 screenPosition, Plane plane)
        {
            Ray ray = camera.ViewportPointToRay(screenPosition);
            return plane.Raycast(ray, out float dist) ? ray.GetPoint(dist) : null;
        }
    }
}