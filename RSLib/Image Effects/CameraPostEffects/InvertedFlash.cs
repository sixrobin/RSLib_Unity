namespace RSLib.ImageEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Inverted Flash")]
    public class InvertedFlash : CameraPostEffect
    {
        [SerializeField, Range(0f, 1f)]
        public float _percentage = 1f;

        [SerializeField]
        public bool Desaturated = true;

        [SerializeField]
        private Vector2 _desaturationSmoothstep = new Vector2(0.45f, 0.55f);

        private static readonly int PercentageID = Shader.PropertyToID("_Percentage");
        private static readonly int DesaturateID = Shader.PropertyToID("_Desaturate");
        private static readonly int DesaturationSmoothstepID = Shader.PropertyToID("_DesaturationSmoothstep");

        protected override string ShaderName => "RSLib/Post Effects/Inverted Flash";
        
        public float Percentage
        {
            get => _percentage;
            set => _percentage = Mathf.Clamp01(value);
        }

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(PercentageID, _percentage);
            material.SetFloat(DesaturateID, this.Desaturated ? 1f : 0f);
            material.SetVector(DesaturationSmoothstepID, _desaturationSmoothstep);
        }

        private void OnValidate()
        {
            _desaturationSmoothstep.x = Mathf.Clamp01(_desaturationSmoothstep.x);
            _desaturationSmoothstep.y = Mathf.Clamp01(_desaturationSmoothstep.y);
        }
    }
}