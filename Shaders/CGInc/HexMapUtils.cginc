#define SQRT3 1.73205080757

float3 hexMap_AxialToCube(float2 axialPos)
{
    return float3(axialPos.x, axialPos.y, -axialPos.x - axialPos.y);
}

int2 hexMap_CubeToOffset(int3 cubePos)
{
    int x = cubePos.x + (cubePos.y - (cubePos.y & 1)) / 2;
    return int2(x, cubePos.y);
}

int2 hexMap_OffsetToAxial(int2 offsetPos)
{
    return int2(offsetPos.x - (offsetPos.y - (offsetPos.y & 1)) / 2, offsetPos.y);
}

int3 hexMap_CubeRound(float3 cubePosition)
{
    int3 roundedPos = int3(round(cubePosition.x), round(cubePosition.y), round(cubePosition.z));
    float3 posDiff = float3(abs(roundedPos.x - cubePosition.x), abs(roundedPos.y - cubePosition.y), abs(roundedPos.z - cubePosition.z));

    if (posDiff.x > posDiff.y && posDiff.x > posDiff.z)
        roundedPos.x = -roundedPos.y - roundedPos.z;
    else if (posDiff.y > posDiff.z)
        roundedPos.y = -roundedPos.x - roundedPos.z;
    else
        roundedPos.z = -roundedPos.x - roundedPos.y;

    return roundedPos;
}

float2 hexMap_WorldToCell(float2 worldPosition)
{
    float2 axialPos = float2((SQRT3 / 3 * worldPosition.x - (1/3.0) * worldPosition.y) * 2, (2/3.0) * worldPosition.y * 2);
    float3 cubePos = hexMap_AxialToCube(axialPos);
    float3 cubePosRounded = hexMap_CubeRound(cubePos);
    int2 offsetPos = hexMap_CubeToOffset(cubePosRounded);
    return offsetPos;
}

float2 hexMap_CellToWorld(int2 offsetPos)
{
    float x = SQRT3 / 2 * (offsetPos.x + 0.5 * (offsetPos.y & 1));
    float y = offsetPos.y * 0.75;
    return float2(x, y);
}
