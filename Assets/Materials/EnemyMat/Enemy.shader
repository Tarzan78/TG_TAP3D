Shader "Custom/Enemy"
{
    Properties
    {
        _TexAlbedo("Albedo (RGB)", 2D) = "white" {}
        _TexNoise("Noise (RGB)", 2D) = "white" {}
        _TexDisolve("Disolve (RGB)", 2D) = "white" {}
        _disolve("disolve", Range(0, 1)) = 0
        _ColorX("display name", Color) = (1,1,1,1)
    }
        SubShader
        {
            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _TexAlbedo;
            sampler2D _TexNoise;
            sampler2D _TexDisolve;

            float _disolve;
            float3 _ColorX;

            struct Input
            {
                float2 uv_TexAlbedo;
                float2 uv_TexNoise;
                float2 uv_TexDisolve;
            };


            void surf(Input IN, inout SurfaceOutput o)
            {

                float dissolverResult = tex2D(_TexNoise, IN.uv_TexNoise);
                float3 zero = (0, 0, 0);

                clip(dissolverResult - _disolve);

                if ((dissolverResult - _disolve - 0.05f) < 0)
                {
                    o.Albedo = tex2D(_TexDisolve, IN.uv_TexDisolve);
                }
                else
                {
                    o.Albedo = tex2D(_TexAlbedo, IN.uv_TexAlbedo);
                }
            }
            ENDCG
        }

            FallBack "Diffuse"
}
