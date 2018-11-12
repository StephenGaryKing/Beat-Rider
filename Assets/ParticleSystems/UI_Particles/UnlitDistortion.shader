// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32964,y:32656,varname:node_3138,prsc:2|emission-4604-OUT,clip-2274-R;n:type:ShaderForge.SFN_Tex2d,id:2274,x:32191,y:32835,ptovrint:False,ptlb:ParticleShape,ptin:_ParticleShape,varname:node_2274,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6260-OUT;n:type:ShaderForge.SFN_VertexColor,id:2770,x:32156,y:32534,varname:node_2770,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1603,x:32404,y:32644,varname:node_1603,prsc:2|A-2770-RGB,B-2274-R;n:type:ShaderForge.SFN_Multiply,id:4604,x:32648,y:32548,varname:node_4604,prsc:2|A-5128-OUT,B-1603-OUT;n:type:ShaderForge.SFN_Slider,id:5128,x:32325,y:32502,ptovrint:False,ptlb:GlowStrength,ptin:_GlowStrength,varname:node_5128,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:150;n:type:ShaderForge.SFN_Tex2d,id:7352,x:31526,y:32882,ptovrint:False,ptlb:DistortionTexture,ptin:_DistortionTexture,varname:node_7352,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1147-UVOUT;n:type:ShaderForge.SFN_Panner,id:1147,x:31343,y:32882,varname:node_1147,prsc:2,spu:0.25,spv:-0.5|UVIN-1959-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1959,x:31141,y:32882,varname:node_1959,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Slider,id:8564,x:31398,y:32675,ptovrint:False,ptlb:DistortionStrength,ptin:_DistortionStrength,varname:node_8564,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Tex2d,id:6930,x:31526,y:33074,ptovrint:False,ptlb:DistortionTexture2,ptin:_DistortionTexture2,varname:node_6930,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3617-UVOUT;n:type:ShaderForge.SFN_Panner,id:3617,x:31357,y:33074,varname:node_3617,prsc:2,spu:-0.32,spv:0.567|UVIN-1959-UVOUT;n:type:ShaderForge.SFN_Append,id:6260,x:31890,y:32892,varname:node_6260,prsc:2|A-2878-OUT,B-7425-OUT;n:type:ShaderForge.SFN_Multiply,id:2878,x:31727,y:32766,varname:node_2878,prsc:2|A-8564-OUT,B-7352-R;n:type:ShaderForge.SFN_Multiply,id:7425,x:31726,y:33056,varname:node_7425,prsc:2|A-6930-R,B-8564-OUT;proporder:2274-5128-7352-8564-6930;pass:END;sub:END;*/

Shader "Shader Forge/UnlitDistortion" {
    Properties {
        _ParticleShape ("ParticleShape", 2D) = "white" {}
        _GlowStrength ("GlowStrength", Range(1, 150)) = 1
        _DistortionTexture ("DistortionTexture", 2D) = "white" {}
        _DistortionStrength ("DistortionStrength", Range(0, 1)) = 0.5
        _DistortionTexture2 ("DistortionTexture2", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _ParticleShape; uniform float4 _ParticleShape_ST;
            uniform float _GlowStrength;
            uniform sampler2D _DistortionTexture; uniform float4 _DistortionTexture_ST;
            uniform float _DistortionStrength;
            uniform sampler2D _DistortionTexture2; uniform float4 _DistortionTexture2_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_1444 = _Time;
                float2 node_1147 = (i.uv0+node_1444.g*float2(0.25,-0.5));
                float4 _DistortionTexture_var = tex2D(_DistortionTexture,TRANSFORM_TEX(node_1147, _DistortionTexture));
                float2 node_3617 = (i.uv0+node_1444.g*float2(-0.32,0.567));
                float4 _DistortionTexture2_var = tex2D(_DistortionTexture2,TRANSFORM_TEX(node_3617, _DistortionTexture2));
                float2 node_6260 = float2((_DistortionStrength*_DistortionTexture_var.r),(_DistortionTexture2_var.r*_DistortionStrength));
                float4 _ParticleShape_var = tex2D(_ParticleShape,TRANSFORM_TEX(node_6260, _ParticleShape));
                clip(_ParticleShape_var.r - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (_GlowStrength*(i.vertexColor.rgb*_ParticleShape_var.r));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _ParticleShape; uniform float4 _ParticleShape_ST;
            uniform sampler2D _DistortionTexture; uniform float4 _DistortionTexture_ST;
            uniform float _DistortionStrength;
            uniform sampler2D _DistortionTexture2; uniform float4 _DistortionTexture2_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_2561 = _Time;
                float2 node_1147 = (i.uv0+node_2561.g*float2(0.25,-0.5));
                float4 _DistortionTexture_var = tex2D(_DistortionTexture,TRANSFORM_TEX(node_1147, _DistortionTexture));
                float2 node_3617 = (i.uv0+node_2561.g*float2(-0.32,0.567));
                float4 _DistortionTexture2_var = tex2D(_DistortionTexture2,TRANSFORM_TEX(node_3617, _DistortionTexture2));
                float2 node_6260 = float2((_DistortionStrength*_DistortionTexture_var.r),(_DistortionTexture2_var.r*_DistortionStrength));
                float4 _ParticleShape_var = tex2D(_ParticleShape,TRANSFORM_TEX(node_6260, _ParticleShape));
                clip(_ParticleShape_var.r - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
