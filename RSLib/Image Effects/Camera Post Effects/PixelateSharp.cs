namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Pixelate Sharp")]
    public class PixelateSharp : CameraPostEffect
    {
        [SerializeField] private bool _lockXY = true;
        [SerializeField] private Vector2 _scale = new Vector2(2f, 2f);

        private static readonly int s_scaleXID = Shader.PropertyToID("_ScaleX");
        private static readonly int s_scaleYID = Shader.PropertyToID("_ScaleY");

        private float _scaleX;
        private float _scaleY;
        
        protected override string ShaderName => "RSLib/Post Effects/Pixelate Sharp";

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
            this.SetScaleX(x);
            this.SetScaleY(y);
        }

        public void SetScale(Vector2Int size)
        {
            this.SetScaleX(size.x);
            this.SetScaleY(size.y);
        }

        public void ResetScale()
        {
            SetScaleX(1);
            SetScaleY(1);
        }

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(s_scaleXID, this._scale.x);
            material.SetFloat(s_scaleYID, this._scale.y);
        }

        private void Update()
        {
            if (Mathf.Abs(this._scaleX - this._scale.x) > 0.001f)
            {
                _scaleX = this._scale.x;
                if (_lockXY)
                    _scaleY = this._scale.y = _scaleX;
            }

            if (Mathf.Abs(this._scaleY - this._scale.y) > 0.001f)
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