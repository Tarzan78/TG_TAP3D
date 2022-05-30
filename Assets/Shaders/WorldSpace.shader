Shader "Custom/WorldSpace"
{
    Properties
    {
        _TexAlbedo("Albedo (RGB)", 2D) = "white" {}
    }
        SubShader
    {
        Cull off

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _TexAlbedo;

        struct Input
        {
            float2 uv_TexAlbedo;
            float3 worldPos;
        };


        void surf(Input IN, inout SurfaceOutput o)
        {
            //o.Albedo = tex2D(_TexAlbedo, IN.uv_TexAlbedo);
            //o.Albedo = frac(IN.worldPos.y) < 0.5f ? tex2D(_TexAlbedo, IN.uv_TexAlbedo) : float3 (1,0,1) ;
            //frac pega na parte fractionária 
            if (frac(IN.worldPos.y) < 0.5f)
            {
                o.Albedo = tex2D(_TexAlbedo, IN.uv_TexAlbedo);
            }
            else
            {
                //clip(-1);
                discard;
            }

        }
        ENDCG
    }

        FallBack "Diffuse"
}
