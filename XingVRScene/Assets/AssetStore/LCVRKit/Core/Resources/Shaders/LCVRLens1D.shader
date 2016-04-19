//
// LC VR Kit
//
// Copyright (c) 2015 Laurel Code Inc.
// All rights reserved.
//
// Contact: Laurel Code Inc. (support@laurel-code.com)
//


Shader "LCVRKit/LCVRLens1D" {
	Properties {
		_MainTex("Base"      , 2D) = "white" {}
		_DistMap("Distortion", 2D) = "black" {}
		_ScaleAndMaxLength("ScaleAndMaxLength", Vector) = (1.0, 1.0, 1.0, 1.0)
	}

	SubShader {
	  Tags { "RenderType"="Opaque" }
	  Pass
	  {

	  Lighting Off
	  ZWrite   Off 
	  ZTest    Always
	  Cull     Off
	  Fog      { Mode Off }

CGPROGRAM
#pragma vertex   vert
#pragma fragment frag

struct appdata{
	float4 vertex   : POSITION ;
	float2 texcoord : TEXCOORD0;
};

struct v2f{
	float4 pos : SV_POSITION;
	float2 uv  : TEXCOORD0  ;
};

sampler2D _MainTex;
sampler2D _DistMap;

float4 _ScaleAndMaxLength;

fixed4 frag(v2f i) : COLOR
{
	float2 p    = i.uv * 2.0f - 1.0f;

	float2 p2   = p * _ScaleAndMaxLength.xy;
	float4 dist = tex2D(_DistMap, float2(length(p2 / _ScaleAndMaxLength.z), 0.0f));

	float  s    = dist.z * 2.0f - 1.0f;
	float  d    = dist.x * pow(2, dist.y * 255.0f - 128.0f) * s;

	float2 uv   = i.uv + p / (1.0f + d) - p;

	float  mask = (0.0f <= uv.x && uv.x <= 1.0f && 0.0f <= uv.y && uv.y <= 1.0f) ? 1.0f : 0.0f;

	return tex2D(_MainTex, uv) * mask;
}

v2f vert(appdata v)
{
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv  = v.texcoord;
	return o;
}
ENDCG
    }
	}

	Fallback Off
}
