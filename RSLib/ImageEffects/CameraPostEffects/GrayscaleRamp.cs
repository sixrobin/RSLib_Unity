namespace RSLib.ImageEffects.CameraPostEffects
{
    using RSLib.Extensions;
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Grayscale Ramp")]
    public class GrayscaleRamp : CameraPostEffect
    {
        [SerializeField]
        private Texture2D _textureRamp = null;
        [SerializeField, Range(-1f, 1f)]
        private float _offset = 0f;
        [SerializeField, Range(0f, 1f)]
        private float _weight = 1f;

        private static readonly int s_rampTexID = Shader.PropertyToID("_RampTex");
        private static readonly int s_rampOffsetID = Shader.PropertyToID("_RampOffset");
        private static readonly int s_weightID = Shader.PropertyToID("_Weight");

        public bool Inverted;
        private Texture2D _initRamp;
        
        protected override string ShaderName => "RSLib/Post Effects/Grayscale Ramp";

        public Texture2D TextureRamp => _textureRamp;

        public float Offset
        {
            get => _offset;
            set => _offset = Mathf.Clamp(value, -1f, 1f);
        }

        public void SetRamp(Texture2D ramp)
        {
            _textureRamp = ramp;
        }

        public void ResetRamp()
        {
            _textureRamp = _initRamp;
        }

        private void Awake()
        {
            _initRamp = _textureRamp;
        }

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetTexture(s_rampTexID, Inverted && _textureRamp != null ? _textureRamp.FlipX() : _textureRamp);
            material.SetFloat(s_rampOffsetID, _offset);
            material.SetFloat(s_weightID, _weight);
        }
    }
}