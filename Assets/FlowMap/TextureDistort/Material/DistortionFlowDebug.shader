Shader "Custom/DistortionFlowDebug" {
	Properties {
		_TestTime ("Time", Range(0.0, 5.0)) = 0
		_Tiling ("Tiling", Float) = 1
		_JumpU ("_JumpU", Range(-0.25, 0.25)) = 0
		_JumpV ("_JumpV", Range(-0.25, 0.25)) = 0
		_FlowStrength ("Flow Strength", Float) = 1
		_FlowOffset ("Flow Offset", Float) = 0
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
		float _TestTime;
		float _JumpU, _JumpV, _FlowOffset, _FlowStrength, _Tiling;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float noise = tex2D(_FlowMap, IN.uv_MainTex).a * 2;
			float time = _TestTime + noise;
			float2 jump = float2(_JumpU, _JumpV);

			

			float2 flowVector = tex2D(_FlowMap, IN.uv_MainTex).rg * 2 - 1;	// 0 - 1  >> -1 ~ 1

			flowVector *= _FlowStrength;

			float3 uvwA = FlowUVW(IN.uv_MainTex, flowVector, _FlowOffset, _Tiling, jump, time, 0);
			float3 uvwB = FlowUVW(IN.uv_MainTex, flowVector, _FlowOffset, _Tiling, jump, time, 0.5);

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
