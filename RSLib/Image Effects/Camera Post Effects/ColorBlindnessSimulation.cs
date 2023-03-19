namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Color Blindness Simulation")]
    public class ColorBlindnessSimulation : CameraPostEffect
    {
        private enum ColorBlindnessType
        {
            [InspectorName("Protanomaly (L cone - Red)")]      PROTANOMALY,
            [InspectorName("Deuteranomaly (M cone - Green)")]  DEUTERANOMALY,
            [InspectorName("Tritanomaly (S cone - Blue)")]     TRITANOMALY,
            [InspectorName("Achromatopsia (Total blindness)")] ACHROMATOPSIA
        }

        [SerializeField]
        private ColorBlindnessType _colorBlindnessType = ColorBlindnessType.PROTANOMALY;

        [SerializeField, Range(0f, 1f)]
        private float _severity = 1f;

        [SerializeField]
        private bool _difference = false;
        
        private static readonly int s_severityID = Shader.PropertyToID("_Severity");
        private static readonly int s_differenceID = Shader.PropertyToID("_Difference");

        protected override string ShaderName => "RSLib/Post Effects/Color Blindness Simulation";

        protected override int Pass => (int)_colorBlindnessType;

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            Material.SetFloat(s_severityID, this._severity);
            Material.SetInt(s_differenceID, this._difference ? 1 : 0);
        }
    }
}