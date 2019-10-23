Shader "Scroll" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ScrollSpeed("Scroll Speed", float) = 1.0
		_ScrollSpeedX("Scroll Speed X", float) = 0.0
		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionAmount("Emission Amount", float) = 1.0
		_EmissionMap("Emission Texture", 2D) = "white" {}
		_Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicMap("Metallic Texture", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
	}
	SubShader {
		Name "Scroll"
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.0

		sampler2D _MainTex;
		sampler2D _EmissionMap;
		sampler2D _MetallicMap;
		sampler2D _NormalMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_EmissionMap;
			float2 uv_MetallicMap;
			float2 uv_NormalMap;
		};

		fixed4 _EmissionColor;
		half _EmissionAmount;
		half _Metallic;
		half _ScrollSpeed;
		half _ScrollSpeedX;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 samplePosMod;
			samplePosMod.x = _Time * _ScrollSpeedX;
			samplePosMod.y = _Time * _ScrollSpeed;
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex + samplePosMod) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			//Emission
			fixed4 e = tex2D(_EmissionMap, IN.uv_EmissionMap + samplePosMod) * _EmissionColor * _EmissionAmount;
			o.Emission = e.rgb;
			//Metalic
			fixed4 m = tex2D(_MetallicMap, IN.uv_MetallicMap + samplePosMod);
			o.Metallic = ((m.r + m.g + m.b) / 3) * _Metallic;
			//Normal
			fixed4 n = tex2D(_NormalMap, IN.uv_NormalMap + samplePosMod);
			o.Normal = n.rgb;
		}
		ENDCG

	}

	FallBack "Diffuse"
}