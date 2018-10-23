Shader "Oldschool/FishEye"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Distortion("Amount", Range(-3, 3)) = -1
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
			fixed _Distortion;
			
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
				//lens distortion coefficient
				float k = -0.15;

				float r2 = (i.uv.x - 0.5) * (i.uv.x - 0.5) + (i.uv.y - 0.5) * (i.uv.y - 0.5);
				float f = 0;

				f = 1 + r2 * (k + _Distortion * sqrt(r2));

				//get the right pixel for the current position;
				float x = f * (i.uv.x - 0.5) + 0.5;
				float y = f * (i.uv.y - 0.5) + 0.5;
				return tex2D(_MainTex, float2(x, y));
			}
			ENDCG
		}
	}
}
