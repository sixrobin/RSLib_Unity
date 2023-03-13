Shader "RSLib/Post Effects/High Pass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", float) = 5
    }
    
    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed _Radius;

            v2f vert(const appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(const v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float max = sqrt(_Radius * _Radius * 2);

                float3 blur = float3(0, 0, 0);
                float sum = 0;

                for (float u = -_Radius; u <= _Radius; u++)
                {
                    for (float v = -_Radius; v <= _Radius; v++)
                    {
                        float weight = max - sqrt(u * u + v * v);
                        blur += weight * tex2D(_MainTex, uv + float2(u, v) / _ScreenParams.xy);
                        sum += weight;
                    }
                }
               
                blur /= sum;
                fixed3 col = tex2D(_MainTex, uv).rgb;
                return fixed4(float3(col - blur) + float3(0.5, 0.5, 0.5), 1);
            }
            
            ENDCG
        }
    }
}