namespace RSLib.ImageEffects.CameraPostEffects
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
        
        private static readonly int s_curvatureID = Shader.PropertyToID("_Curvature");
        private static readonly int s_vignetteWidthID = Shader.PropertyToID("_VignetteWidth");
        private static readonly int s_scanlinesMultiplierID = Shader.PropertyToID("_ScanlinesMultiplier");
        private static readonly int s_rgbMultiplierID = Shader.PropertyToID("_RGBMultiplier");
        
        protected override string ShaderName => "RSLib/Post Effects/CRT";

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(s_curvatureID, _curvature);
            material.SetFloat(s_vignetteWidthID, _vignetteWidth);
            material.SetFloat(s_scanlinesMultiplierID, _scanlinesMultiplier);
            material.SetVector(s_rgbMultiplierID, _rgbMultiplier); 
        }
    }
}