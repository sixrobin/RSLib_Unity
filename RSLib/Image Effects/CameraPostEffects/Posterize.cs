namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Posterize")]
    public class Posterize : CameraPostEffect
    {
        [SerializeField, Range(1, 255)]
        public int _steps = 4;

        private static readonly int s_stepsID = Shader.PropertyToID("_Steps");

        protected override string ShaderName => "RSLib/Post Effects/Posterize";

        public int Steps
        {
            get => _steps;
            set => _steps = Mathf.Clamp(value, 1, 255);
        }

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(s_stepsID, _steps);
        }
    }
}