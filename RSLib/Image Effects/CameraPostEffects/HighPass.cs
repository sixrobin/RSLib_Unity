namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/High Pass")]
    public class HighPass : CameraPostEffect
    {
        [SerializeField, Range(0.51f, 30f)]
        public float _radius = 5f;
        
        private static readonly int s_radiusID = Shader.PropertyToID("_Radius");
        
        protected override string ShaderName => "RSLib/Post Effects/High Pass";
        
        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetFloat(s_radiusID, _radius);
        }
    }
}