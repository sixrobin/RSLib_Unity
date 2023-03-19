namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("RSLib/Camera Post Effects/Radial Blur")]
    public class RadialBlur : CameraPostEffect
    {
        [SerializeField, Range(0, 50)] private int _samples = 25;
        [SerializeField, Range(0f, 1f)] private float _strength = 0.25f;
        [SerializeField] private Vector2 _center = new Vector2(0.5f, 0.5f);

        private static readonly int s_samplesID = Shader.PropertyToID("_Samples");
        private static readonly int s_strengthID = Shader.PropertyToID("_Strength");
        private static readonly int s_centerXID = Shader.PropertyToID("_CenterX");
        private static readonly int s_centerYID = Shader.PropertyToID("_CenterY");

        protected override string ShaderName => "RSLib/Post Effects/Radial Blur";

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            material.SetInt(s_samplesID, _samples + 1);
            material.SetFloat(s_strengthID, _strength);
            material.SetFloat(s_centerXID, _center.x);
            material.SetFloat(s_centerYID, _center.y);
        }
    }
}