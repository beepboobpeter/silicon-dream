Shader "Custom/FogShader"
{
    Properties
    {
        _MainTex("Albedo", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _FogColor("Fog Color", Color) = (0.5,0.5,0.5,1)
        _FogDensity("Fog Density", Range(0,0.1)) = 0.02
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // -------------------------
            // Properties
            // -------------------------
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _Color;
            float4 _FogColor;
            float _FogDensity;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            // -------------------------
            // Vertex shader
            // -------------------------
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionWS = worldPos;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.positionCS = TransformWorldToHClip(worldPos);
                OUT.uv = IN.uv;
                return OUT;
            }

            // -------------------------
            // Fragment shader
            // -------------------------
            float4 frag(Varyings IN) : SV_Target
            {
                // Sample texture
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;

                // Compute distance from camera
                float distance = length(IN.positionWS - _WorldSpaceCameraPos);

                // Exponential fog factor
                float fogFactor = 1.0 - exp(-distance * _FogDensity);
                fogFactor = saturate(fogFactor);

                // Mix object color with fog
                float3 finalColor = lerp(albedo.rgb, _FogColor.rgb, fogFactor);

                return float4(finalColor, albedo.a);
            }

            ENDHLSL
        }
    }
}