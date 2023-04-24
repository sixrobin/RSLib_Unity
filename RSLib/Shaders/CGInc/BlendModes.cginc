// Cannot be included dynamically as it's not part of the CGPROGRAM block.
// [KeywordEnum(Normal, Add, Average, ColorBurn, ColorDodge, Darken, Difference, Exclusion, Glow, HardLight, Lighten, LinearBurn, LinearDodge, LinearLight, Multiply, Negation, Overlay, Phoenix, PinLight, Reflect, Screen, SoftLight, Subtract, VividLight, HardMix)]

#pragma multi_compile _BLEND_NORMAL _BLEND_ADD _BLEND_AVERAGE _BLEND_COLORBURN _BLEND_COLORDODGE _BLEND_DARKEN _BLEND_DIFFERENCE _BLEND_EXCLUSION _BLEND_GLOW _BLEND_HARDLIGHT _BLEND_LIGHTEN _BLEND_LINEARBURN _BLEND_LINEARDODGE _BLEND_LINEARLIGHT _BLEND_MULTIPLY _BLEND_NEGATION _BLEND_NORMAL _BLEND_OVERLAY _BLEND_PHOENIX _BLEND_PINLIGHT _BLEND_REFLECT _BLEND_SCREEN _BLEND_SOFTLIGHT _BLEND_SUBTRACT _BLEND_VIVIDLIGHT _BLEND_HARDMIX

float3 Blend_Add(float3 a, float3 b)
{
    return min(a + b, 1);
}
float3 Blend_Average(float3 a, float3 b)
{
    return (a + b) * 0.5;
}
float3 Blend_ColorBurn(float3 a, float3 b)
{
    return b == 0 ? b : max(1 - (1 - a) / b, 0);
}
float3 Blend_ColorDodge(float3 a, float3 b)
{
    return b == 1 ? b : min(a / (1 - b), 1);
}
float3 Blend_Darken(float3 a, float3 b)
{
    return min(a, b);
}
float3 Blend_Difference(float3 a, float3 b)
{
    return abs(a - b);
}
float3 Blend_Exclusion(float3 a, float3 b)
{
    return a + b - 2 * a * b;
}
float3 Blend_Glow(float3 a, float3 b)
{
    return a == 1 ? a : min(b * b / (1 - a), 1);
}
float3 Blend_HardLight(float3 a, float3 b)
{
    return b < 0.5 ? (2 * a * b) : (1 - 2 * (1 - a) * (1 - b));
}
float3 Blend_Lighten(float3 a, float3 b)
{
    return max(a, b);
}
float3 Blend_LinearBurn(float3 a, float3 b)
{
    return max(a + b - 1, 0);
}
float3 Blend_LinearDodge(float3 a, float3 b)
{
    return min(a + b, 1);
}
float3 Blend_LinearLight(float3 a, float3 b)
{
    return b < 0.5 ? Blend_LinearBurn(a, 2 * b) : Blend_LinearDodge(a, 2 * (b - 0.5));
}
float3 Blend_Multiply(float3 a, float3 b)
{
    return a * b;
}
float3 Blend_Negation(float3 a, float3 b)
{
    return 1 - abs(1 - a - b);
}
float3 Blend_Normal(float3 a, float3 b)
{
    return b;
}
float3 Blend_Overlay(float3 a, float3 b)
{
    return a < 0.5 ? (2 * a * b) : (1 - 2 * (1 - a) * (1 - b));
}
float3 Blend_Phoenix(float3 a, float3 b)
{
    return min(a, b) - max(a, b) + 1;
}
float3 Blend_PinLight(float3 a, float3 b)
{
    return b < 0.5 ? Blend_Darken(a, 2 * b) : Blend_Lighten(a, 2 * (b - 0.5));
}
float3 Blend_Reflect(float3 a, float3 b)
{
    return b == 1 ? b : min(a * a / (1 - b), 1);
}
float3 Blend_Screen(float3 a, float3 b)
{
    return 1 - (1 - a) * (1 - b);
}
float3 Blend_SoftLight(float3 a, float3 b)
{
    return b < 0.5 ? (2 * a * b + a * a * (1 - 2 * b)) : (sqrt(a) * (2 * b - 1) + (2 * a) * (1 - b));
}
float3 Blend_Subtract(float3 a, float3 b)
{
    return max(a + b - 1, 0);
}
float3 Blend_VividLight(float3 a, float3 b)
{
    return b < 0.5 ? Blend_ColorBurn(a, 2 * b) : Blend_ColorDodge(a, 2 * (b - 0.5));
}
float3 Blend_HardMix(float3 a, float3 b)
{
    return Blend_VividLight(a, b) < 0.5 ? 0 : 1;
}

float3 Blend_Dynamic(float3 a, float3 b, float blend_type)
{
    if (blend_type == 1)
        return Blend_Add(a, b);
    if (blend_type == 2)
        return Blend_Average(a, b);
    if (blend_type == 3)
        return Blend_ColorBurn(a, b);
    if (blend_type == 4)
        return Blend_ColorDodge(a, b);
    if (blend_type == 5)
        return Blend_Darken(a, b);
    if (blend_type == 6)
        return Blend_Difference(a, b);
    if (blend_type == 7)
        return Blend_Exclusion(a, b);
    if (blend_type == 8)
        return Blend_Glow(a, b);
    if (blend_type == 9)
        return Blend_HardLight(a, b);
    if (blend_type == 10)
        return Blend_Lighten(a, b);
    if (blend_type == 11)
        return Blend_LinearBurn(a, b);
    if (blend_type == 12)
        return Blend_LinearDodge(a, b);
    if (blend_type == 13)
        return Blend_LinearLight(a, b);
    if (blend_type == 14)
        return Blend_Multiply(a, b);
    if (blend_type == 15)
        return Blend_Negation(a, b);
    if (blend_type == 16)
        return Blend_Overlay(a, b);
    if (blend_type == 17)
        return Blend_Phoenix(a, b);
    if (blend_type == 18)
        return Blend_PinLight(a, b);
    if (blend_type == 19)
        return Blend_Reflect(a, b);
    if (blend_type == 20)
        return Blend_Screen(a, b);
    if (blend_type == 21)
        return Blend_SoftLight(a, b);
    if (blend_type == 22)
        return Blend_Subtract(a, b);
    if (blend_type == 23)
        return Blend_VividLight(a, b);
    if (blend_type == 24)
        return Blend_HardMix(a, b);

    return Blend_Normal(a, b);
}