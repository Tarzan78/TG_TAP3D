Shader "Custom/FireMag"
{
    Properties
    {
        _TexWater("Water Texture", 2D) = "white" {}
        _TexNoise("Noise Texture", 2D) = "white" {}
    }

    SubShader
        {
            //Day Night ""Pass""
           CGPROGRAM
           #pragma surface surf Lambert

           sampler2D _TexWater;
           sampler2D _TexNoise;

           float _Movimento;
           float _Movimento1;
           float _Movimento2;

           struct Input
           {
               float2 uv_TexWater;
               float2 uv_TexNoise;
           };


           void surf(Input IN, inout SurfaceOutput o)
           {
               float2 sinusoidWave = float2(sin(_Time.z * 0.01), cos(_Time.z * 0.01)); // multiplyer is to slowdown 

               sinusoidWave /= 2; //pixel amount 

               float3 tempColorNoise = tex2D(_TexNoise, IN.uv_TexNoise /** sinusoidWave*/);

               float2 tempCord = float2(tempColorNoise.r, tempColorNoise.r /** sinusoidWave.x*/);

               //tempCord *= float2(sin(_Time.y), cos(_Time.y)); -> efeito elétrico está nice até
               tempCord.x += sinusoidWave.x;
               tempCord.y += sinusoidWave.y;

               o.Albedo = tex2D(_TexWater, tempCord /** (sinusoidWave * -1)*/);           
            }
           ENDCG
        }

    FallBack "Diffuse"
}
