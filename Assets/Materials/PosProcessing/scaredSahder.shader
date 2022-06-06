Shader "Hidden/scaredSahder"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Radius("Radius Size", Range(0, 1)) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Radius;


            bool isInside(float2 centerXY, float rad, float2 xy)
                {
                // Compare radius of circle with
                // distance of its center from
                // given point
                if ((xy.x - centerXY.x) * (xy.x - centerXY.x) +
                    (xy.y - centerXY.y) * (xy.y - centerXY.y) <= rad * rad)
                    return true;
                else
                    return false;
            }

            float PercentageOfFade(float minRadius, float maxRadius, float pointDistance)
            {
                float temp = pointDistance - minRadius;

                float percentage = temp / maxRadius;

                return percentage;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed tempAvg = (col.r + col.g + col.b) / 3;

                fixed4 blackAndWhite = fixed4(tempAvg, tempAvg, tempAvg, 1);

                i.uv.x *= _MainTex_TexelSize.z;
                i.uv.y *= _MainTex_TexelSize.w;

                float2 centerXY = float2(_MainTex_TexelSize.z / 2, _MainTex_TexelSize.w / 2);
                float2 xy = i.uv;

                if (!isInside(centerXY, _MainTex_TexelSize.w * _Radius, xy)) {
                    //[0;1000 ]
                    float distanceCenter = sqrt((xy.x - centerXY.x) * (xy.x - centerXY.x) + (xy.y - centerXY.y) * (xy.y - centerXY.y));

                    distanceCenter /= _MainTex_TexelSize.w;

                    //adapt the max to get more darker 

                    float percentage = PercentageOfFade(/*_MainTex_TexelSize.w * */_Radius, 0.5, distanceCenter);

                    //fixed4 shader = fixed4(0, 0, 0, 1);

                    //if (distanceCenter > 0.5 )
                    //{
                    //    col = fixed4(1,0,0,1);
                    //}
                    //else
                    //{
                    //    col = fixed4(1, 1, 0, 1);
                    //
                    //}
                    //shader *= percentage;
                    //float max = 1;
                    col *= (1 - percentage);
                    //col.r = distanceCenter;
                    //col.g = distanceCenter;
                    //col.b = distanceCenter;
                    //col += shader;
                }

                return col;
            }
        ENDCG
        }
    }
}
