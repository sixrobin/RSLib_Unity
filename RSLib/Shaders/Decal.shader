Shader "RSLib/Decal"
{
	Properties
	{
		[HDR] _Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent-400"
			"DisableBatching" = "True"
		}

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 position  : SV_POSITION;
				float4 screenPos : TEXCOORD0;
				float3 ray       : TEXCOORD1;
			};

			float3 decal_GetWorldRay(float3 worldPos)  { return worldPos - _WorldSpaceCameraPos; }
			float2 decal_GetScreenUV(float4 screenPos) { return screenPos.xy / screenPos.w; }
			
			float3 decal_GetClippedProjectedObjectPos(float2 screenPos, float3 worldRay)
			{
			    float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenPos);
			    depth = Linear01Depth(depth) * _ProjectionParams.z;

			    worldRay = normalize(worldRay);
			    worldRay /= dot(worldRay, -UNITY_MATRIX_V[2].xyz);

			    float3 worldPos = _WorldSpaceCameraPos + worldRay * depth;
			    float3 objectPos = mul(unity_WorldToObject, float4(worldPos, 1)).xyz;
			    clip(0.5 - abs(objectPos));
			    objectPos += 0.5;
			    
			    return objectPos;
			}
			
			v2f vert(appdata v)
			{
				v2f o;
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.position = UnityWorldToClipPos(worldPos);
				o.ray = decal_GetWorldRay(worldPos);
				o.screenPos = ComputeScreenPos(o.position);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET
			{
				float2 screenUV = i.screenPos.xy / i.screenPos.w;
				float2 uv = decal_GetClippedProjectedObjectPos(screenUV, i.ray).xz;
				return tex2D(_MainTex, uv * _MainTex_ST.xy + _MainTex_ST.zw) * _Color;
			}

			ENDCG
		}
	}
}