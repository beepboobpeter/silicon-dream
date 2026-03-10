Shader "Custom/LitRetroDither_FullGradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _MidColor ("Mid Color", Color) = (0.5,0.5,0.5,1)
        _LightColor ("Highlight Color", Color) = (1,1,1,1)
        _Roughness ("Roughness", Range(0,1)) = 0.5
        _Levels ("Dither Levels", Range(2,8)) = 4
        _LightDirection ("Light Direction", Vector) = (0,-1,0,0)
        _LightIntensity ("Light Intensity", Range(0,10)) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;
            float4 _ShadowColor;
            float4 _MidColor;
            float4 _LightColor;
            float _Roughness;
            float _Levels;
            float3 _LightDirection;
            float _LightIntensity;

            float Dither4x4(float2 pixelPos)
            {
                int x = (int)pixelPos.x % 4;
                int y = (int)pixelPos.y % 4;

                const float dither[16] =
                {
                    0.0, 8.0, 2.0, 10.0,
                    12.0,4.0,14.0,6.0,
                    3.0,11.0,1.0,9.0,
                    15.0,7.0,13.0,5.0
                };
                return dither[y*4+x]/16.0;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float3 normal = normalize(IN.normalWS);

                // Sample texture
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // User-controlled light
                float3 lightDir = normalize(_LightDirection);

                // Lambert diffuse
                float NdotL = saturate(dot(normal, -lightDir));
                float3 diffuse = albedo.rgb * NdotL * _LightIntensity;

                // Simple specular
                float3 viewDir = normalize(_WorldSpaceCameraPos - IN.positionWS);
                float3 halfDir = normalize(viewDir - lightDir);
                float spec = pow(saturate(dot(normal, halfDir)), (1 - _Roughness) * 64);
                float3 lighting = diffuse + spec;

                // Dither
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                float2 pixelPos = screenUV * _ScreenParams.xy;
                float threshold = Dither4x4(pixelPos);

                float brightness = dot(lighting, float3(0.299,0.587,0.114));
                float quant = floor(brightness * _Levels + threshold) / _Levels;

                // Interpolate using shadow → mid → light
                float3 finalColor;
                if (quant <= 0.5)
                {
                    float t = quant / 0.5; // 0 → 1
                    finalColor = lerp(_ShadowColor.rgb, _MidColor.rgb * albedo.rgb, t);
                }
                else
                {
                    float t = (quant - 0.5) / 0.5; // 0 → 1
                    finalColor = lerp(_MidColor.rgb * albedo.rgb, _LightColor.rgb * albedo.rgb, t);
                }

                return float4(finalColor, 1);
            }

            ENDHLSL
        }
    }
}