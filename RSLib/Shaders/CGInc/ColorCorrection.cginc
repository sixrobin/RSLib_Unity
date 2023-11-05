#include "UnityCG.cginc"

#define TAU 6.283

float3 adjustContrast(float3 color, half contrast)
{
    #if !UNITY_COLORSPACE_GAMMA
    color = LinearToGammaSpace(color);
    #endif
    color = saturate(pow(abs(color * 2 - 1), 1 / max(contrast, 0.0001)) * sign(color - 0.5) + 0.5);
    #if !UNITY_COLORSPACE_GAMMA
    color = GammaToLinearSpace(color);
    #endif
    return color;
}
float3 adjustContrastPercentage(float3 color, half contrast)
{
    const float3 averageLuminance = float3(0.5, 0.5, 0.5); // Tweak these values to adjust RGB color channels separately.
    return lerp(averageLuminance, color, contrast);
}

float4 desaturate(float3 color, float desaturation)
{
    float3 grayXfer = float3(0.3, 0.59, 0.11);
    float grayf = dot(grayXfer, color);
    float3 gray = float3(grayf.xxx);
    return float4(lerp(color, gray, desaturation), 1);
}

// Hue shift goes from 0 to 1.
// Saturation goes from 0 to some value around 10 (arbitrary max value).
// Brightness goes from -1 to 1.
float3 yiqCorrection(float3 color, float hueShift, float saturation, float brightness)
{
    const float3x3 yiq_to_rgb = { +1.0000, +0.9563, +0.6210,
                                  +1.0000, -0.2721, -0.6474,
                                  +1.0000, -1.1070, +1.7046 };

    const float3x3 rgb_to_yiq = { +0.2990, +0.5870, +0.1140,
                                  +0.5957, -0.2745, -0.3213,
                                  +0.2115, -0.5226, +0.3112 };

    float3 yiq = mul(rgb_to_yiq, color);
    
    const float hue = atan2(yiq.z, yiq.y) + hueShift * TAU;
    const float chroma = length(float2(yiq.y, yiq.z)) * saturation;

    float y = yiq.x + brightness;
    float i = chroma * cos(hue);
    float q = chroma * sin(hue);

    return saturate(mul(yiq_to_rgb, float3(y,i,q))); // Convert back to rgb.
}
float3 yiqCorrection(float3 color, float3 hueSaturationBrightness)
{
    return yiqCorrection(color, hueSaturationBrightness.x, hueSaturationBrightness.y, hueSaturationBrightness.z);
}

float3 photoshopCorrection(float3 color, float hueShift, float saturation, float brightness, float contrast)
{
    // Hue/saturation. Saturation is brought from [-100,0,100] to [0,1,5]. // [TODO] Not really -100 for hue if this is divided by 360?
    hueShift /= 360.0;
    saturation *= 0.01;
    color = yiqCorrection(color, hueShift, saturation + 1 + saturate(saturation) * saturation * 5, 0);

    // Brightness.
    brightness *= 0.01;
    float3 brightnessColor = float3(step(0, brightness).xxx);
    color = lerp(color, brightnessColor, brightness * brightness);

    // Contrast. Brought from [-100,0,100] to [0,1,3].
    contrast *= 0.01;
    color = adjustContrastPercentage(color, contrast + 1 + saturate(contrast) * contrast * 3);

    return color;
}
float3 photoshopCorrection(float3 color, float4 hueSaturationBrightnessContrast)
{
    return photoshopCorrection(color, hueSaturationBrightnessContrast.x, hueSaturationBrightnessContrast.y, hueSaturationBrightnessContrast.z, hueSaturationBrightnessContrast.w);
}

fixed3 textureHue(fixed3 color, fixed hue)
{
    // Brought from [-100,0,100] to [0,1,5]. // [TODO] Not really -100 if this is divided by 360?
    hue /= 360.0;
    color = yiqCorrection(color, hue, 1, 0);
    return color;
}

float HueToRGB(float f1, float f2, float hue)
{
    if (hue < 0)
        hue += 1;
    else if (hue > 1)
        hue -= 1;
    
    if (6 * hue < 1)
        return f1 + (f2 - f1) * 6 * hue;
    if (2 * hue < 1)
        return f2;
    if (3 * hue < 2)
        return f1 + (f2 - f1) * (2 / 3.0 - hue) * 6;
    
    return f1;
}
float3 RGBToHSL(float3 color)
{
    float3 hsl = float3(0,0,0);
	
    float channelMin = min(min(color.r, color.g), color.b);
    float channelMax = max(max(color.r, color.g), color.b);
    float delta = channelMax - channelMin;

    hsl.z = (channelMax + channelMin) * 0.5; // Luminance.

    if (delta == 0) // Grayscale color, no chroma.
    {
        hsl.x = 0;
        hsl.y = 0;
        return hsl;
    }
    
    // Saturation.
    if (hsl.z < 0.5)
        hsl.y = delta / (channelMax + channelMin);
    else
        hsl.y = delta / (2 - channelMax - channelMin);
	
    float deltaR = ((channelMax - color.r) / 6.0 + delta / 2.0) / delta;
    float deltaG = ((channelMax - color.g) / 6.0 + delta / 2.0) / delta;
    float deltaB = ((channelMax - color.b) / 6.0 + delta / 2.0) / delta;

    // Hue.
    if (color.r == channelMax)
        hsl.x = deltaB - deltaG; 
    else if (color.g == channelMax)
        hsl.x = (1.0 / 3.0) + deltaR - deltaB;
    else if (color.b == channelMax)
        hsl.x = (2.0 / 3.0) + deltaG - deltaR;

    if (hsl.x < 0)
        hsl.x += 1;
    else if (hsl.x > 1)
        hsl.x -= 1;

    return hsl;
}
float3 HSLToRGB(float3 hsl)
{
    if (hsl.y == 0.0) // Grayscale texture, return luminance.
        return float3(hsl.zzz); 
    
    float f2;
    if (hsl.z < 0.5)
        f2 = hsl.z * (1 + hsl.y);
    else
        f2 = hsl.z + hsl.y - hsl.y * hsl.z;
		
    float f1 = 2 * hsl.z - f2;

    float3 rgb;
    rgb.r = HueToRGB(f1, f2, hsl.x + 1.0 / 3.0);
    rgb.g = HueToRGB(f1, f2, hsl.x);
    rgb.b = HueToRGB(f1, f2, hsl.x - 1.0 / 3.0);
	
    return rgb;
}

float3 ContrastSaturationBrightness(float3 color, float brightness, float saturation, float contrast)
{
    const float3 averageLuminance = float3(0.5, 0.5, 0.5); // Tweak these values to adjust RGB color channels separately.
    const float3 luminanceCoefficient = float3(0.2125, 0.7154, 0.0721);
	
    float3 brightenedColor = color * brightness;
    float3 intensity = float3(dot(brightenedColor, luminanceCoefficient).xxx);
    float3 saturatedColor = lerp(intensity, brightenedColor, saturation);
    float3 contrastedColor = lerp(averageLuminance, saturatedColor, contrast);
    
    return contrastedColor;
}