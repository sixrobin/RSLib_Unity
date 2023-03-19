namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Pixelate Blurred")]
    public class PixelateBlurred : CameraPostEffect
    {
        [SerializeField] private bool _lockXY = true;
        [SerializeField] private Vector2Int _scale = new Vector2Int(10, 10);

        private static readonly int s_scaleXID = Shader.PropertyToID("_ScaleX");
        private static readonly int s_scaleYID = Shader.PropertyToID("_ScaleY");

        private int _scaleX;
        private int _scaleY;
        
        protected override string ShaderName => "RSLib/Post Effects/Pixelate Blurred";

        public void SetScaleX(int value)
        {
            this._scale.x = value;
        }

        public void SetScaleY(int value)
        {
            this._scale.y = value;
        }

        public void SetScale(int x, int y)
        {
            SetScaleX(x);
            SetScaleY(y);
        }

        public void SetScale(Vector2Int size)
        {
            SetScaleX(size.x);
            SetScaleY(size.y);
        }

        public void ResetScale()
        {
            SetScaleX(1);
            SetScaleY(1);
        }

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetInt(s_scaleXID, this._scale.x);
            material.SetInt(s_scaleYID, this._scale.y);
        }

        private void Update()
        {
            if (_scaleX != this._scale.x)
            {
                _scaleX = this._scale.x;
                if (_lockXY)
                    _scaleY = this._scale.y = _scaleX;
            }

            if (_scaleY != this._scale.y)
            {
                _scaleY = this._scale.y;
                if (_lockXY)
                    _scaleX = this._scale.x = _scaleY;
            }
        }

        private void OnValidate()
        {
            this._scale.x = Mathf.Clamp(this._scale.x, 1, 200);
            this._scale.y = Mathf.Clamp(this._scale.y, 1, 200);
        }
    }
}