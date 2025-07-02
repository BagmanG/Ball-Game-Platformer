Shader "Animmal/Ice URP"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
        _Specularity("Specularity", Range(0, 1)) = 0.5
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Tint("Tint", Color) = (0.2867647,0.704868,1,0)
        
        [Header(Textures)]
        _BaseMap("Base Map", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _TranslucencyMap("Translucency Map", 2D) = "white" {}
        
        [Header(Translucency)]
        _TranslucencyColor("Translucency Color", Color) = (0,0.6275863,1,0)
        _TranslucencyPower("Translucency Power", Range(0, 10)) = 1
        _TranslucencyDistortion("Distortion", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 tangentWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
                float4 shadowCoord : TEXCOORD5;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);
            TEXTURE2D(_TranslucencyMap); SAMPLER(sampler_TranslucencyMap);
            
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BumpMap_ST;
            float4 _TranslucencyMap_ST;
            float4 _Tint;
            float4 _BaseColor;
            float4 _TranslucencyColor;
            float _Specularity;
            float _Smoothness;
            float _TranslucencyPower;
            float _TranslucencyDistortion;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.tangentWS = normalInput.tangentWS;
                output.bitangentWS = normalInput.bitangentWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                
                output.shadowCoord = TransformWorldToShadowCoord(output.positionWS);
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Sample textures
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                half4 translucencyMap = SAMPLE_TEXTURE2D(_TranslucencyMap, sampler_TranslucencyMap, input.uv);
                half3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, input.uv));
                
                // Calculate normal in world space
                float3x3 tangentToWorld = float3x3(
                    input.tangentWS,
                    input.bitangentWS,
                    input.normalWS
                );
                half3 normalWS = normalize(mul(normalTS, tangentToWorld));
                
                // Lighting setup
                Light mainLight = GetMainLight(input.shadowCoord);
                float3 viewDir = normalize(_WorldSpaceCameraPos - input.positionWS);
                float3 lightDir = mainLight.direction;
                
                // Standard lighting
                half NdotL = saturate(dot(normalWS, lightDir));
                half3 diffuse = mainLight.color * NdotL;
                
                // Specular
                half3 halfDir = normalize(lightDir + viewDir);
                half NdotH = saturate(dot(normalWS, halfDir));
                half specular = pow(NdotH, _Specularity * 128) * _Smoothness;
                
                // Translucency
                half3 transLightDir = lightDir + normalWS * _TranslucencyDistortion;
                half transDot = pow(saturate(dot(viewDir, -transLightDir)), _TranslucencyPower);
                half3 translucency = mainLight.color * transDot * translucencyMap.rgb * _TranslucencyColor.rgb;
                
                // Combine everything
                half3 color = (_Tint * baseMap * _BaseColor).rgb;
                color = color * diffuse + specular + translucency;
                
                return half4(color, 1);
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformWorldToHClip(TransformObjectToWorld(input.positionOS.xyz));
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
}