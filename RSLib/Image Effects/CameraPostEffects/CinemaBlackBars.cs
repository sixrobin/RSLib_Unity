namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Cinema Black Bars")]
    public class CinemaBlackBars : CameraPostEffect
    {
        [SerializeField, Range(0f, 1f)]
        public float _width = 0.25f;
        
        private static readonly int s_widthID = Shader.PropertyToID("_Width");
        
        protected override string ShaderName => "RSLib/Post Effects/Cinema Black Bars";
        
        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(s_widthID, _width);
        }
    }
}