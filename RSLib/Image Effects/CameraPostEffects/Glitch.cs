namespace RSLib.ImageEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Camera Post Effects/Glitch")]
    public class Glitch : CameraPostEffect 
    {
        [SerializeField, Range(0f, 1f)] private float _intensity = 1f;
	    [SerializeField, Range(0f, 1f)] private float _flipIntensity = 1f;
	    [SerializeField, Range(0f, 1f)] private float _colorIntensity = 1f;
	    [SerializeField] private Texture2D _displacementMap = null;

	    private static readonly int s_intensityID = Shader.PropertyToID("_Intensity");
	    private static readonly int s_colorIntensityID = Shader.PropertyToID("_ColorIntensity");
	    private static readonly int s_displacementTextureID = Shader.PropertyToID("_DispTex");
	    private static readonly int s_filterRadiusID = Shader.PropertyToID("filter_radius");
	    private static readonly int s_directionID = Shader.PropertyToID("direction");
	    private static readonly int s_flipUpID = Shader.PropertyToID("flip_up");
	    private static readonly int s_flipDownID = Shader.PropertyToID("flip_down");
	    private static readonly int s_displaceID = Shader.PropertyToID("displace");
	    private static readonly int s_scaleID = Shader.PropertyToID("scale");

	    private float _glitchUp;
	    private float _glitchDown;
	    private float _flicker;
	    private float _glitchUpTime = 0.05f;
	    private float _glitchDownTime = 0.05f;
	    private float _flickerTime = 0.5f;
	    
	    protected override string ShaderName => "RSLib/Post Effects/Glitch";
	    
	    public void IncreaseIntensity(float intensityIncrease, float flipIncrease, float colorIncrease)
	    {
		    _intensity = Mathf.Clamp01(this._intensity + intensityIncrease);
		    _flipIntensity += Mathf.Clamp01(this._intensity + flipIncrease);
		    _colorIntensity += Mathf.Clamp01(this._intensity + colorIncrease);
	    }
	    
	    public void DecreaseIntensity(float intensityDecrease, float flipDecrease, float colorDecrease)
	    {
		    _intensity = Mathf.Clamp01(this._intensity - intensityDecrease);
		    _flipIntensity += Mathf.Clamp01(this._intensity - flipDecrease);
		    _colorIntensity += Mathf.Clamp01(this._intensity - colorDecrease);
	    }

	    public void ResetValues()
	    {
		    _intensity = 0f;
		    _flipIntensity = 0f;
		    _colorIntensity = 0f;
	    }

	    protected override void OnBeforeRenderImage(RenderTexture source, RenderTexture destination, Material material)
	    {
		    material.SetFloat(s_intensityID, _intensity);
            material.SetFloat(s_colorIntensityID, _colorIntensity);
		    material.SetTexture(s_displacementTextureID, _displacementMap);
        
            _flicker += Time.deltaTime * _colorIntensity;
            _glitchUp += Time.deltaTime * _flipIntensity;
            _glitchDown += Time.deltaTime * _flipIntensity;

            if (_flicker > _flickerTime)
		    {
			    material.SetFloat(s_filterRadiusID, Random.Range(-3f, 3f) * _colorIntensity);
			    material.SetVector(s_directionID, Quaternion.AngleAxis(Random.Range(0f, 360f) * _colorIntensity, Vector3.forward) * Vector4.one);
                _flicker = 0f;
			    _flickerTime = Random.value;
		    }

            if (_colorIntensity == 0f)
	            material.SetFloat(s_filterRadiusID, 0f);
        
            if (_glitchUp > _glitchUpTime)
		    {
			    material.SetFloat(s_flipUpID, Random.value < 0.1f * _flipIntensity ? Random.value * _flipIntensity : 0f);
			    _glitchUp = 0f;
			    _glitchUpTime = Random.value * 0.1f;
		    }

            if (_flipIntensity == 0f)
	            material.SetFloat(s_flipUpID, 0f);

            if (_glitchDown > _glitchDownTime)
		    {
			    material.SetFloat(s_flipDownID, Random.value < _flipIntensity * 0.1f ? 1f - Random.value * _flipIntensity : 1f);
			    _glitchDown = 0f;
			    _glitchDownTime = Random.value * 0.1f;
            }

            if (_flipIntensity == 0f)
	            material.SetFloat(s_flipDownID, 1f);

            if (Random.value < 0.05f * _intensity)
		    {
			    material.SetFloat(s_displaceID, Random.value * _intensity);
			    material.SetFloat(s_scaleID, 1f - Random.value * _intensity);
            }
            else
		    {
			    material.SetFloat(s_displaceID, 0f);
		    }
	    }
    }
}