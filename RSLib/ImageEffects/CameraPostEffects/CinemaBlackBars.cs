namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Cinema Black Bars")]
    public class CinemaBlackBars : CameraPostEffect
    {
        private static readonly int WIDTH_ID = Shader.PropertyToID("_Width");

        [SerializeField, Range(0f, 1f)]
        public float Width = 0.25f;
        
        protected override string ShaderName => "RSLib/Post Effects/Cinema Black Bars";
        
        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(WIDTH_ID, Width);
        }
    }
}