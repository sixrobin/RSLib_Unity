Shader "RSLib/Debug/Wireframe"
{
    Properties
    {
        _WireframeFrontColour ("Front Colour", Color) = (1,1,1,1)
        _WireframeBackColour ("Back Colour", Color) = (0.5, 0.5, 0.5, 1)
        _WireframeWidth ("Width", Float) = 0.05
    }
    
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            Cull Front
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            struct g2f
            {
                float4 pos         : SV_POSITION;
                float3 barycentric : TEXCOORD0;
            };

            fixed4 _WireframeBackColour;
            float _WireframeWidth;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2f IN[3], inout TriangleStream<g2f> triStream)
            {
                // This applies the barycentric coordinates to each vertex in a triangle.

                g2f o;
                
                o.pos = IN[0].vertex;
                o.barycentric = float3(1,0,0);
                triStream.Append(o);
                
                o.pos = IN[1].vertex;
                o.barycentric = float3(0,1,0);
                triStream.Append(o);
                
                o.pos = IN[2].vertex;
                o.barycentric = float3(0,0,1);
                triStream.Append(o);
            }

            fixed4 frag(g2f i) : SV_Target
            {
                float closest = min(i.barycentric.x, min(i.barycentric.y, i.barycentric.z)); // Find the barycentric coordinate closest to the edge.
                float alpha = step(closest, _WireframeWidth);
                return fixed4(_WireframeBackColour.rgb, alpha);
            }
            
            ENDCG
        }

        Pass
        {
            Cull Back
            
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            struct g2f
            {
                float4 pos         : SV_POSITION;
                float3 barycentric : TEXCOORD0;
            };

            fixed4 _WireframeFrontColour;
            float _WireframeWidth;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // This applies the barycentric coordinates to each vertex in a triangle.
            [maxvertexcount(3)]
            void geom(triangle v2f IN[3], inout TriangleStream<g2f> triStream)
            {
                g2f o;
                
                o.pos = IN[0].vertex;
                o.barycentric = float3(1,0,0);
                triStream.Append(o);
                
                o.pos = IN[1].vertex;
                o.barycentric = float3(0,1,0);
                triStream.Append(o);
                
                o.pos = IN[2].vertex;
                o.barycentric = float3(0,0,1);
                triStream.Append(o);
            }

            fixed4 frag(g2f i) : SV_Target
            {
                float closest = min(i.barycentric.x, min(i.barycentric.y, i.barycentric.z)); // Find the barycentric coordinate closest to the edge.
                float alpha = step(closest, _WireframeWidth);
                return fixed4(_WireframeFrontColour.rgb, alpha);
            }
            
            ENDCG
        }
    }
}
