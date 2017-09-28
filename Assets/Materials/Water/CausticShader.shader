// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|emission-4090-OUT;n:type:ShaderForge.SFN_Tex2d,id:4292,x:31983,y:32465,ptovrint:False,ptlb:Caustic,ptin:_Caustic,varname:_Caustic,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:470d9e061e0d5e24594da03b24ec6b6e,ntxv:0,isnm:False|UVIN-1302-UVOUT;n:type:ShaderForge.SFN_Panner,id:1302,x:31399,y:33041,varname:node_1302,prsc:2,spu:0.02,spv:0.03|UVIN-749-OUT;n:type:ShaderForge.SFN_TexCoord,id:948,x:30926,y:33042,varname:node_948,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:749,x:31207,y:33021,varname:node_749,prsc:2|A-948-UVOUT,B-2705-OUT;n:type:ShaderForge.SFN_Vector1,id:2705,x:30940,y:32892,varname:node_2705,prsc:2,v1:2;n:type:ShaderForge.SFN_Slider,id:3883,x:31505,y:32109,ptovrint:False,ptlb:Caustic Depth,ptin:_CausticDepth,varname:_CausticDepth,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_DepthBlend,id:9171,x:31965,y:32183,varname:node_9171,prsc:2|DIST-3883-OUT;n:type:ShaderForge.SFN_Lerp,id:4090,x:32348,y:32399,varname:node_4090,prsc:2|A-2084-OUT,B-4292-RGB,T-9171-OUT;n:type:ShaderForge.SFN_Vector1,id:2084,x:32066,y:32334,varname:node_2084,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:3906,x:31208,y:32543,ptovrint:False,ptlb:FoamMasks,ptin:_FoamMasks,varname:_FoamMasks,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:beeb04059f66eca4785873d1a8bd662c,ntxv:0,isnm:False|UVIN-4625-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:6386,x:30733,y:32543,varname:node_6386,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:4625,x:30976,y:32543,varname:node_4625,prsc:2,spu:0.02,spv:0.02|UVIN-6386-UVOUT;n:type:ShaderForge.SFN_Append,id:1237,x:31589,y:32522,varname:node_1237,prsc:2|A-3906-R,B-3906-G;proporder:4292-3883-3906;pass:END;sub:END;*/

Shader "Shader Forge/CausticShader" {
    Properties {
        _Caustic ("Caustic", 2D) = "white" {}
        _CausticDepth ("Caustic Depth", Range(0, 1)) = 1
        _FoamMasks ("FoamMasks", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _Caustic; uniform float4 _Caustic_ST;
            uniform float _CausticDepth;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 projPos : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float node_2084 = 0.0;
                float4 node_6977 = _Time + _TimeEditor;
                float2 node_1302 = ((i.uv0*2.0)+node_6977.g*float2(0.02,0.03));
                float4 _Caustic_var = tex2D(_Caustic,TRANSFORM_TEX(node_1302, _Caustic));
                float3 emissive = lerp(float3(node_2084,node_2084,node_2084),_Caustic_var.rgb,saturate((sceneZ-partZ)/_CausticDepth));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _Caustic; uniform float4 _Caustic_ST;
            uniform float _CausticDepth;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float node_2084 = 0.0;
                float4 node_9988 = _Time + _TimeEditor;
                float2 node_1302 = ((i.uv0*2.0)+node_9988.g*float2(0.02,0.03));
                float4 _Caustic_var = tex2D(_Caustic,TRANSFORM_TEX(node_1302, _Caustic));
                o.Emission = lerp(float3(node_2084,node_2084,node_2084),_Caustic_var.rgb,saturate((sceneZ-partZ)/_CausticDepth));
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
