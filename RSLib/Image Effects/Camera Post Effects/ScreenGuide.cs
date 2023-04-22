namespace RSLib.ImageEffects.CameraPostEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Screen Guide")]
    public class ScreenGuide : CameraPostEffect
    {
        [Header("SETTINGS")]
        [SerializeField]
        private bool _alwaysShowCenter = false;
        [SerializeField, Range(0, 10)]
        private int _horizontalLines = 3;
        [SerializeField, Range(0, 10)]
        private int _verticalLines = 3;

        [Header("STYLE")]
        [SerializeField]
        private Color _colorX = Color.red;
        [SerializeField]
        private Color _colorY = Color.green;
        
        [SerializeField, Range(1f, 10f)]
        private float _scale = 1;

        private const string ALWAYS_SHOW_CENTER_KEYWORD = "ALWAYSSHOWCENTER";
        private static readonly int s_linesXID = Shader.PropertyToID("_LinesX");
        private static readonly int s_linesYID = Shader.PropertyToID("_LinesY");
        private static readonly int s_colorXID = Shader.PropertyToID("_ColorX");
        private static readonly int s_colorYID = Shader.PropertyToID("_ColorY");
        private static readonly int s_scaleID = Shader.PropertyToID("_LineScale");
        
        protected override string ShaderName => "RSLib/Post Effects/Screen Guide";

        protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
        {
            if (_alwaysShowCenter)
                Material.EnableKeyword(ALWAYS_SHOW_CENTER_KEYWORD);
            else
                Material.DisableKeyword(ALWAYS_SHOW_CENTER_KEYWORD);

            Material.SetFloat(s_linesXID, _verticalLines);
            Material.SetFloat(s_linesYID, _horizontalLines);
            Material.SetColor(s_colorXID, _colorX);
            Material.SetColor(s_colorYID, _colorY);
            Material.SetFloat(s_scaleID, _scale);
        }
    }
}