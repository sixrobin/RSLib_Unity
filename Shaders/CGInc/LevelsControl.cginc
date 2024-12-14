float3 levelsControl_GammaCorrection(float3 color, float gamma)
{
    return pow(abs(color), 1.0 / gamma);
}
float3 levelsControl_InputRange(float3 color, float3 minInput, float3 maxInput)
{
    return min(max(color - minInput, 0) / (maxInput - minInput), 1);
}
float3 levelsControl(float3 color, float3 minInput, float3 maxInput, float gamma)
{
    return levelsControl_GammaCorrection(levelsControl_InputRange(color, minInput, maxInput), gamma);
}