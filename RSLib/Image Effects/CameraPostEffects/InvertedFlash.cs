namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Inverted Flash")]
    public class InvertedFlash : CameraPostEffect
    {
        [SerializeField, Range(0f, 1f)]
        private float _percentage = 1f;

        [SerializeField]
        public bool Desaturated = true;

        [SerializeField]
        private Vector2 _desaturationSmoothstep = new Vector2(0.45f, 0.55f);

        private static readonly int s_percentageID = Shader.PropertyToID("_Percentage");
        private static readonly int s_desaturateID = Shader.PropertyToID("_Desaturate");
        private static readonly int s_desaturationSmoothstepID = Shader.PropertyToID("_DesaturationSmoothstep");

        protected override string ShaderName => "RSLib/Post Effects/Inverted Flash";
        
        public float Percentage
        {
            get => _percentage;
            set => _percentage = Mathf.Clamp01(value);
        }

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(s_percentageID, _percentage);
            material.SetFloat(s_desaturateID, Desaturated ? 1f : 0f);
            material.SetVector(s_desaturationSmoothstepID, _desaturationSmoothstep);
        }

        private void OnValidate()
        {
            _desaturationSmoothstep.x = Mathf.Clamp01(_desaturationSmoothstep.x);
            _desaturationSmoothstep.y = Mathf.Clamp01(_desaturationSmoothstep.y);
        }
    }
}