Shader "Water/UnderWater"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Scale("Size", float) = 1
		_Frequency("Frequency", float) = 1
		_Speed("Speed", float) = 1
		_PixelOffset("Pixel Offset", float) = 0.005
		_IsEnabled("Is Enabled", float) = 0
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"
#include "noiseSimplex.cginc"
#define PI 3.1415926535897932384626433832795

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
		float4 scrPos : TEXCOORD2;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float _Scale;
	float _Frequency;
	float _Speed;
	float _PixelOffset;
	bool _IsEnabled;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.scrPos = ComputeScreenPos(o.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		float3 pos = float3(i.scrPos.x, i.scrPos.y, 0) * _Frequency * _IsEnabled;
		pos.z += _Time.x * _Speed * _IsEnabled;
		float noise = _Scale * _IsEnabled * ((snoise(pos) + 1) / 2);

		float4 noiseToDir = float4(cos(noise * PI * 2), sin(noise * PI * 2), 0, 0);
		float4 col = tex2Dproj(_MainTex, i.scrPos + (normalize(noiseToDir) * _PixelOffset));

		return col;
	}
		ENDCG
	}
	}
}
