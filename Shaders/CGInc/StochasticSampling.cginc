#pragma exclude_renderers gles // Excluded shader from OpenGL ES 2.0 because it uses non-square matrices

float2 stochasticSampling_hash2D2D(float2 s)
{
	return frac(sin(fmod(float2(dot(s, float2(127.1, 311.7)), dot(s, float2(269.5, 183.3))), 3.1415)) * 43758.5453);
}

float4x3 stochasticSampling_computeBlendWeights(float2 uv)
{
	// UV transformed into triangular grid space with UV scaled by an approximation of 2 * sqrt(3).
	float2 skewUV = mul(float2x2(1, 0, -0.57735027, 1.15470054), uv * 3.464);
	
	// Vertex IDs and barycentric coordinates.
	float2 vertexID = float2(floor(skewUV));
	float3 barry = float3(frac(skewUV), 0);
	barry.z = 1 - barry.x - barry.y;
	
	// Triangle vertices and blend weights.
	// blendWeightsVertices[0..2].xyz = triangle vertices.
	// blendWeightsVertices[3].xy = blend weights (z is unused).
	float4x3 blendWeightsVertices = barry.z > 0
									? float4x3(float3(vertexID,  0), float3(vertexID + float2(0,1), 0), float3(vertexID + float2(1,0), 0), barry.zyx)
									: float4x3(float3(vertexID + float2(1,1), 0), float3(vertexID + float2(1,0), 0), float3(vertexID + float2(0,1), 0), float3(-barry.z, 1 - barry.y, 1 - barry.x));

	return blendWeightsVertices;
}

float4 tex2DStochastic(sampler2D tex, float2 uv)
{
	float4x3 blendWeightsVertices = stochasticSampling_computeBlendWeights(uv);
	
	// Calculate derivatives to avoid triangular grid artifacts.
	float2 dx = ddx(uv);
	float2 dy = ddy(uv);
    
	// Blend samples with calculated weights.
	return mul(tex2D(tex, uv + stochasticSampling_hash2D2D(blendWeightsVertices[0].xy), dx, dy), blendWeightsVertices[3].x)
		   + mul(tex2D(tex, uv + stochasticSampling_hash2D2D(blendWeightsVertices[1].xy), dx, dy), blendWeightsVertices[3].y)
		   + mul(tex2D(tex, uv + stochasticSampling_hash2D2D(blendWeightsVertices[2].xy), dx, dy), blendWeightsVertices[3].z);
}
