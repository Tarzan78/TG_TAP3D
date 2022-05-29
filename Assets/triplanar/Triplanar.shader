Shader "Unlit/Triplanar"
{
    Properties
    {
        _TopTex("top", 2D) = "white" {}
        _BottText("bot", 2D) = "white" {}
        _DissolveTex("Dissolve", 2D) = "white" {}
        _ErvaRange("ErvaRange", Range(-1,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 wPos : TEXCOORD0;
                float3 normalNoMundo : TEXCOORD1;

                float4 color : COLOR;
            };

            sampler2D _TopTex;
            float4 _TopTex_ST;

            sampler2D _BottText;
            float4 _BottText_ST;

            sampler2D _DissolveTex;
            float4 _DissolveTex_ST;

            fixed _ErvaRange;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.pos);
                o.wPos = mul(unity_ObjectToWorld, v.pos);
                o.normalNoMundo = UnityObjectToWorldNormal(v.normal).xyz;

                // ligt
                float lambertEfect = max(0, dot(o.normalNoMundo, _WorldSpaceLightPos0.xyz));

                o.color = (lambertEfect + 0.2) * _LightColor0;

                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                float3 triplanarUV = i.wPos;
                float3 normalNoMundoRep = abs(i.normalNoMundo);

                float4 x = tex2D(_BottText, triplanarUV.yz);
                float4 y = tex2D(_BottText, triplanarUV.xz);
                float4 z = tex2D(_BottText, triplanarUV.xy);

                float4 pedraTex = x * normalNoMundoRep.x + y * normalNoMundoRep.y + z * normalNoMundoRep.z;

                float4 xTop = tex2D(_TopTex, triplanarUV.yz);
                float4 yTop = tex2D(_TopTex, triplanarUV.xz);
                float4 zTop = tex2D(_TopTex, triplanarUV.xy);
                float4 ervaTex = xTop * normalNoMundoRep.x + yTop * normalNoMundoRep.y + zTop * normalNoMundoRep.z;

                float4 xDissolve = tex2D(_DissolveTex, triplanarUV.yz);
                float4 yDissolve = tex2D(_DissolveTex, triplanarUV.xz);
                float4 zDissolve = tex2D(_DissolveTex, triplanarUV.xy);
                float4 dissolveTex = xDissolve * normalNoMundoRep.x + yDissolve * normalNoMundoRep.y + zDissolve * normalNoMundoRep.z;

                float4 col = step(_ErvaRange * dissolveTex.x, i.normalNoMundo.y) * ervaTex + step(i.normalNoMundo.y, _ErvaRange * dissolveTex.x) * pedraTex;

                return col * i.color;
            }
            ENDCG
        }
    }
}
