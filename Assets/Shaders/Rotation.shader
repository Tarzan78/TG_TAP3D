Shader "Unlit/Rotation"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float2 rodar2D(float2 pos, float angle)
            {
                float2x2 rot = { cos(angle), sin(angle),
                                -sin(angle), cos(angle) };

                return mul(rot, pos);
            }

            float3 geneal3DRotation(float3 pos, float x, float y, float z) 
            {
                float3x3 rot = { (cos(y) * cos(z))  , (sin(x) * sin(y) * cos(z)) - (cos(x) * sin(z))    , (cos(x) * sin(y) * cos(z)) + (sin(x) * sin(z)),
                                 (cos(y) * sin(z))  , (sin(x) * sin(y) * sin(z)) + (cos(x) * cos(z))    , (cos(x) * sin(y) * sin(z)) - (sin(x) * cos(z)),
                                     -sin(y)        ,                     sin(x) * cos(y)              ,                     cos(x) * cos(y)
                };
                
                float3 temp = mul(rot, pos);
                //é possivel estar inverso mas na wiki está assim 
                return float3(temp.z, temp.y, temp.x);
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                //2D
                //v.vertex.xy = rodar2D(v.vertex.xy, _Time.y);

                //3D
                v.vertex.xyz = geneal3DRotation(v.vertex.xyz, _Time.x, _Time.y, _Time.z);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv/*rodar2D(i.uv, 3.14)*/);

                return col;
            }
            ENDCG
        }
    }
}
