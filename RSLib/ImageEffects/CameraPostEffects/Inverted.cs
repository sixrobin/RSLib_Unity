namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Inverted")]
    public class Inverted : CameraPostEffect
    {
        private static readonly int PERCENTAGE_ID = Shader.PropertyToID("_Percentage");

        [SerializeField, Range(0f, 1f)]
        private float _percentage = 1f;

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
            material.SetFloat(PERCENTAGE_ID, _percentage);
        }
    }
}