Shader "RSLib/Debug/Color Channels"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        [MaterialToggle] _ShowRed ("Red", Float) = 1
        [MaterialToggle] _ShowGreen ("Green", Float) = 1
        [MaterialToggle] _ShowBlue ("Blue", Float) = 1
    }
    
    SubShader
    {
        Pass
        {
            Tags
            {
                "RenderType"="Opaque"
                "Queue"="Transparent"
            }
            
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            
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
            fixed4 _MainTex_ST;
            fixed _ShowRed;
            fixed _ShowGreen;
            fixed _ShowBlue;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);

                color.r *= _ShowRed;
                color.g *= _ShowGreen;
                color.b *= _ShowBlue;

                return color;
            }
            
            ENDCG
        }
    }
}
