namespace RSLib.ImageEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Pixelate")]
    public class Pixelate : CameraPostEffect
    {
        [SerializeField] private bool _lockXY = true;
        [SerializeField] private Vector2Int _size = Vector2Int.one;

        private static readonly int s_pixelateXID = Shader.PropertyToID("_PixelateX");
        private static readonly int s_pixelateYID = Shader.PropertyToID("_PixelateY");

        private int _pixelSizeX;
        private int _pixelSizeY;
        
        protected override string ShaderName => "RSLib/Post Effects/Pixelate";

        public void SetSizeX(int value)
        {
            _size.x = value;
        }

        public void SetSizeY(int value)
        {
            _size.y = value;
        }

        public void SetSize(int x, int y)
        {
            SetSizeX(x);
            SetSizeY(y);
        }

        public void SetSize(Vector2Int size)
        {
            SetSizeX(size.x);
            SetSizeY(size.y);
        }

        public void ResetSize()
        {
            SetSizeX(1);
            SetSizeY(1);
        }

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetInt(s_pixelateXID, _size.x);
            material.SetInt(s_pixelateYID, _size.y);
        }

        private void Update()
        {
            if (_pixelSizeX != _size.x)
            {
                _pixelSizeX = _size.x;
                if (_lockXY)
                    _pixelSizeY = _size.y = _pixelSizeX;
            }

            if (_pixelSizeY != _size.y)
            {
                _pixelSizeY = _size.y;
                if (_lockXY)
                    _pixelSizeX = _size.x = _pixelSizeY;
            }
        }

        private void OnValidate()
        {
            _size.x = Mathf.Clamp(_size.x, 1, 200);
            _size.y = Mathf.Clamp(_size.y, 1, 200);
        }
    }
}