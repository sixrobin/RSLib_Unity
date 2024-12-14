#define HEX_RATIO 1.73205 // sqrt(3)

float HexagonalDistance(float2 uv)
{
    uv = abs(uv);
    return max(dot(uv, normalize(float2(1, HEX_RATIO))), uv.x);
}
float HexagonalMask(float distance, float radius)
{
    return step(distance, radius * HEX_RATIO);
}
float HexagonalMaskSmooth(float distance, float radius, float smooth)
{
    smooth = max(0.0001, smooth);
    return smoothstep(radius * HEX_RATIO + max(0.0001, smooth), radius * HEX_RATIO - max(0.0001, smooth), distance);
}