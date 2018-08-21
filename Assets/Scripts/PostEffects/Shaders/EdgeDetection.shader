Shader "Custom/EdgeDetection"
{
	Properties{
			_MainTex("Base (RGB)", 2D) = "white" {}
			_DeltaX("Delta X", Float) = 0.01
			_DeltaY("Delta Y", Float) = 0.01
		}

			SubShader{
			Tags{ "RenderType" = "Opaque" }
			LOD 200

			CGINCLUDE

	#include "UnityCG.cginc"

		sampler2D _MainTex;
		float _DeltaX;
		float _DeltaY;

		// This function samples groups of pixels to perform "find edges" on the texture, "tex"
		float sobel(sampler2D tex, float2 uv) {
			// delta is used to process the image in texels
			float2 delta = float2(_DeltaX, _DeltaY);

			float4 hr = float4(0, 0, 0, 0);
			float4 vt = float4(0, 0, 0, 0);

			hr += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
			hr += tex2D(tex, (uv + float2(0.0, -1.0) * delta)) *  0.0;
			hr += tex2D(tex, (uv + float2(1.0, -1.0) * delta)) * -1.0;
			hr += tex2D(tex, (uv + float2(-1.0,  0.0) * delta)) *  2.0;
			hr += tex2D(tex, (uv + float2(0.0,  0.0) * delta)) *  0.0;
			hr += tex2D(tex, (uv + float2(1.0,  0.0) * delta)) * -2.0;
			hr += tex2D(tex, (uv + float2(-1.0,  1.0) * delta)) *  1.0;
			hr += tex2D(tex, (uv + float2(0.0,  1.0) * delta)) *  0.0;
			hr += tex2D(tex, (uv + float2(1.0,  1.0) * delta)) * -1.0;

			vt += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
			vt += tex2D(tex, (uv + float2(0.0, -1.0) * delta)) *  2.0;
			vt += tex2D(tex, (uv + float2(1.0, -1.0) * delta)) *  1.0;
			vt += tex2D(tex, (uv + float2(-1.0,  0.0) * delta)) *  0.0;
			vt += tex2D(tex, (uv + float2(0.0,  0.0) * delta)) *  0.0;
			vt += tex2D(tex, (uv + float2(1.0,  0.0) * delta)) *  0.0;
			vt += tex2D(tex, (uv + float2(-1.0,  1.0) * delta)) * -1.0;
			vt += tex2D(tex, (uv + float2(0.0,  1.0) * delta)) * -2.0;
			vt += tex2D(tex, (uv + float2(1.0,  1.0) * delta)) * -1.0;

			return sqrt(hr * hr + vt * vt);
		}

		float4 frag(v2f_img IN) : COLOR{
			float s = 1-sobel(_MainTex, IN.uv);
			s = step(0, s);
			//return float4(s, s, s, 0);
			return tex2D(_MainTex, IN.uv) * s;
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