// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-6452-OUT;n:type:ShaderForge.SFN_Tex2d,id:8701,x:31387,y:32268,ptovrint:False,ptlb:Distortionmask,ptin:_Distortionmask,varname:node_2274,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:9628,x:31848,y:32294,varname:node_9628,prsc:2;n:type:ShaderForge.SFN_Multiply,id:5817,x:32096,y:32404,varname:node_5817,prsc:2|A-9628-RGB,B-1396-R;n:type:ShaderForge.SFN_Multiply,id:6452,x:32340,y:32308,varname:node_6452,prsc:2|A-3587-OUT,B-5817-OUT;n:type:ShaderForge.SFN_Slider,id:3587,x:32017,y:32262,ptovrint:False,ptlb:GlowStrength,ptin:_GlowStrength,varname:node_5128,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:150;n:type:ShaderForge.SFN_Tex2d,id:8528,x:31218,y:32642,ptovrint:False,ptlb:DistortionTexture,ptin:_DistortionTexture,varname:node_7352,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5438-UVOUT;n:type:ShaderForge.SFN_Panner,id:5438,x:31035,y:32642,varname:node_5438,prsc:2,spu:1,spv:1|UVIN-3705-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:3705,x:30833,y:32642,varname:node_3705,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:8756,x:31218,y:32834,ptovrint:False,ptlb:DistortionTexture2,ptin:_DistortionTexture2,varname:node_6930,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7575-UVOUT;n:type:ShaderForge.SFN_Panner,id:7575,x:31049,y:32834,varname:node_7575,prsc:2,spu:-1,spv:-1|UVIN-3705-UVOUT;n:type:ShaderForge.SFN_Append,id:4900,x:31403,y:32670,varname:node_4900,prsc:2|A-8528-R,B-8756-R;n:type:ShaderForge.SFN_Multiply,id:3811,x:31653,y:32659,varname:node_3811,prsc:2|A-9312-OUT,B-8701-G;n:type:ShaderForge.SFN_Tex2d,id:1396,x:32009,y:32733,ptovrint:False,ptlb:ParticleTexture,ptin:_ParticleTexture,varname:node_1396,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3811-OUT;n:type:ShaderForge.SFN_Multiply,id:9312,x:31486,y:32519,varname:node_9312,prsc:2|A-1567-OUT,B-4900-OUT;n:type:ShaderForge.SFN_Slider,id:1567,x:31047,y:32515,ptovrint:False,ptlb:node_1567,ptin:_node_1567,varname:node_1567,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:8701-3587-8528-8756-1396-1567;pass:END;sub:END;*/

Shader "Shader Forge/AdditiveDistortion" {
    Properties {
        _Distortionmask ("Distortionmask", 2D) = "white" {}
        _GlowStrength ("GlowStrength", Range(1, 150)) = 1
        _DistortionTexture ("DistortionTexture", 2D) = "white" {}
        _DistortionTexture2 ("DistortionTexture2", 2D) = "white" {}
        _ParticleTexture ("ParticleTexture", 2D) = "white" {}
        _node_1567 ("node_1567", Range(0, 1)) = 0
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
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Distortionmask; uniform float4 _Distortionmask_ST;
            uniform float _GlowStrength;
            uniform sampler2D _DistortionTexture; uniform float4 _DistortionTexture_ST;
            uniform sampler2D _DistortionTexture2; uniform float4 _DistortionTexture2_ST;
            uniform sampler2D _ParticleTexture; uniform float4 _ParticleTexture_ST;
            uniform float _node_1567;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_4823 = _Time;
                float2 node_5438 = (i.uv0+node_4823.g*float2(1,1));
                float4 _DistortionTexture_var = tex2D(_DistortionTexture,TRANSFORM_TEX(node_5438, _DistortionTexture));
                float2 node_7575 = (i.uv0+node_4823.g*float2(-1,-1));
                float4 _DistortionTexture2_var = tex2D(_DistortionTexture2,TRANSFORM_TEX(node_7575, _DistortionTexture2));
                float4 _Distortionmask_var = tex2D(_Distortionmask,TRANSFORM_TEX(i.uv0, _Distortionmask));
                float2 node_3811 = ((_node_1567*float2(_DistortionTexture_var.r,_DistortionTexture2_var.r))*_Distortionmask_var.g);
                float4 _ParticleTexture_var = tex2D(_ParticleTexture,TRANSFORM_TEX(node_3811, _ParticleTexture));
                float3 emissive = (_GlowStrength*(i.vertexColor.rgb*_ParticleTexture_var.r));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
