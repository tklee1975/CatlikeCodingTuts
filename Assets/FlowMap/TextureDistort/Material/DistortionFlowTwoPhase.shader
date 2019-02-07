﻿Shader "Custom/DistortionFlowTwoPhase" {
	Properties {
		_Speed ("Speed", Range(-3.0, 3.0)) = 1.0
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _FlowMap ("Flow (RG)", 2D) = "black" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#include "Flow.cginc"

		sampler2D _MainTex;
		sampler2D _FlowMap;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Speed;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float time = _Time.y * _Speed;

			float2 flowVector = tex2D(_FlowMap, IN.uv_MainTex).rg * 2 - 1;	// 0 - 1  >> -1 ~ 1
			float3 uvwA = FlowUVW(IN.uv_MainTex, flowVector, time, 0);
			float3 uvwB = FlowUVW(IN.uv_MainTex, flowVector, time, 0.5);

			fixed4 texA = tex2D (_MainTex, uvwA.xy) * uvwA.z;	// uvw.z is control the brightness
			fixed4 texB = tex2D (_MainTex, uvwB.xy) * uvwB.z;	// uvw.z is control the brightness

			fixed4 c = (texA + texB) * _Color;
			o.Albedo = c.rgb;
			//o.Albedo = float3(flowVector, 0);	// for debug flowmap


			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
