Shader "Custom/CameraPos"
{
    Properties
    {
        _TexAlbedo("Albedo (RGB)", 2D) = "white" {}
    }
        SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _TexAlbedo;

        struct Input
        {
            float2 uv_TexAlbedo;
            float3 worldPos;
            float3 viewPort;
            float3 viewDir;
        };


        void surf(Input IN, inout SurfaceOutput o)
        {
            //_WorldSpaceCameraPos
            //o.Albedo = tex2D(_TexAlbedo, IN.uv_TexAlbedo);
            o.Albedo = IN.worldPos;

            /*o.Albedo = dot(normalize(IN.worldPos), normalize(IN.viewPort));*/
            //o.Albedo = dot(normalize(IN.worldPos), normalize(IN.viewDir)) * _LightColor0;

            //o.Albedo = float3(0, 0, 0);

            o.Albedo.g = frac(distance(_WorldSpaceCameraPos, IN.worldPos));

        }
        ENDCG
    }
        FallBack "Diffuse"
}
