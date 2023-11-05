namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Inverted")]
    public class Inverted : CameraPostEffect
    {
        [SerializeField, Range(0f, 1f)]
        public float _percentage = 1f;

        private static readonly int s_percentageID = Shader.PropertyToID("_Percentage");

        protected override string ShaderName => "RSLib/Post Effects/Inverted";

        public float Percentage
        {
            get => _percentage;
            set => _percentage = Mathf.Clamp01(value);
        }

        public void SetInverted(bool inverted)
        {
            Percentage = inverted ? 1f : 0f;
        }

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(s_percentageID, _percentage);
        }
    }
}