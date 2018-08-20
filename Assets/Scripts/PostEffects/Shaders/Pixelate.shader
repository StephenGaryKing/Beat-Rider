Shader "Oldschool/Pixelate"
{
	Properties{
			_MainTex("Base (RGB)", 2D) = "white" {}
			_PixelateAmount("Pixelate Amount", Range(0.0001,0.05)) = 1
		}

			SubShader{
			Tags{ "RenderType" = "Opaque" }
			LOD 200

			CGINCLUDE

	#include "UnityCG.cginc"

		sampler2D _MainTex;
		half _PixelateAmount;

		float4 frag(v2f_img IN) : COLOR{
			float2 steppedUV = IN.uv;
			steppedUV /= _PixelateAmount;
			steppedUV = round(steppedUV);
			steppedUV *= _PixelateAmount;
			return tex2D(_MainTex,steppedUV);
		}

			ENDCG

			Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			ENDCG
		}

	}
		FallBack "Diffuse"
}