namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    public class RaycastsController
    {
        protected class Origins
        {
            public Vector2 BottomLeft;
            public Vector2 BottomRight;
            public Vector2 TopLeft;
            public Vector2 TopRight;
        }

        protected const float SKIN_WIDTH = 0.01f;
        protected const float RAYCASTS_SPACING = 0.1f;
        protected const float EDGE_MIN_HEIGHT = 0.5f;

        public RaycastsController(BoxCollider2D boxCollider2D)
        {
            _boxCollider2D = boxCollider2D;
            ComputeRaycastsSpacings();
            ComputeRaycastOrigins();
        }
        
        private BoxCollider2D _boxCollider2D;

        protected int HorizontalRaycastsCount { get; private set; }
        protected int VerticalRaycastsCount { get; private set; }
        protected float HorizontalRaycastsSpacing { get; private set; }
        protected float VerticalRaycastsSpacing { get; private set; }

        protected Origins RaycastsOrigins { get; } = new Origins();

        public void ComputeRaycastOrigins()
        {
            Bounds bounds = GetColliderBoundsWithSkinWidth();

            RaycastsOrigins.BottomLeft = bounds.min;
            RaycastsOrigins.TopRight = bounds.max;
            RaycastsOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            RaycastsOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        }

        private Bounds GetColliderBoundsWithSkinWidth()
        {
            Bounds bounds = _boxCollider2D.bounds;
            bounds.Expand(SKIN_WIDTH * -2f);
            return bounds;
        }

        private void ComputeRaycastsSpacings()
        {
            Vector2 boundsSize = GetColliderBoundsWithSkinWidth().size;

            HorizontalRaycastsCount = Mathf.Max(2, Mathf.RoundToInt(boundsSize.y / RAYCASTS_SPACING));
            VerticalRaycastsCount = Mathf.Max(2, Mathf.RoundToInt(boundsSize.x / RAYCASTS_SPACING));

            HorizontalRaycastsSpacing = boundsSize.y / (HorizontalRaycastsCount - 1);
            VerticalRaycastsSpacing = boundsSize.x / (VerticalRaycastsCount - 1);
        }
    }
}