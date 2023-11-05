namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/High Pass")]
    public class HighPass : CameraPostEffect
    {
        private static readonly int RADIUS_ID = Shader.PropertyToID("_Radius");

        [SerializeField, Range(0.51f, 30f)]
        public float Radius = 5f;
        
        protected override string ShaderName => "RSLib/Post Effects/High Pass";
        
        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(RADIUS_ID, Radius);
        }
    }
}