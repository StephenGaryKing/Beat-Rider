Shader "Oldschool/ShadowMask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ShadowMask("Base (RGB)", 2D) = "white" {}
		_Amount("Amount", Range(0, 1)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _ShadowMask;
			float4 _ShadowMask_ST;
			half _Amount;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//samples are halved and combined to keep from over exposing the image

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) / max(1,(2 * _Amount));
				float2 samplePos;
				// sample the shadow mask with tiling
				samplePos.x = i.uv.x * _ShadowMask_ST.x;
				samplePos.y = i.uv.y * _ShadowMask_ST.y;
				col += tex2D(_ShadowMask, samplePos) * (0.5 * _Amount);
				return col;
			}
			ENDCG
		}
	}
}
