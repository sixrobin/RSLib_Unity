namespace RSLib.ImageEffects
{
    using UnityEngine;
    
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("RSLib/Camera Post Effects/CRT")]
    public class CRT : CameraPostEffect
    {
        [SerializeField, Range(1f, 30f)] private float _curvature = 8f;
        [SerializeField, Range(0f, 100f)] private float _vignetteWidth = 50f;
        [SerializeField, Range(0.5f, 3f)] private float _scanlinesMultiplier = 1f;
        [SerializeField] private Vector3 _rgbMultiplier = new(0.25f, 0.2f, 0.3f);
        
        private static readonly int s_crtCurvature = Shader.PropertyToID("_Curvature");
        private static readonly int s_crtVignetteWidth = Shader.PropertyToID("_VignetteWidth");
        private static readonly int s_crtScanlinesMultiplier = Shader.PropertyToID("_ScanlinesMultiplier");
        private static readonly int s_crtRGBMultiplier = Shader.PropertyToID("_RGBMultiplier");
        
        protected override string ShaderName => "RSLib/Post Effects/CRT";

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(s_crtCurvature, this._curvature);
            material.SetFloat(s_crtVignetteWidth, this._vignetteWidth);
            material.SetFloat(s_crtScanlinesMultiplier, this._scanlinesMultiplier);
            material.SetVector(s_crtRGBMultiplier, this._rgbMultiplier); 
        }
    }
}