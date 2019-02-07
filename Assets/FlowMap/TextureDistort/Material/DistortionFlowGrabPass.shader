// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DistortionFlowGrabPass" {
	Properties {
		_Speed ("Speed", Range(-3.0, 3.0)) = 1.0
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _FlowMap ("Flow (RG)", 2D) = "black" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { 
			"Queue" = "Transparent"
			"RenderType"="Opaque" 
		}
		LOD 200

		GrabPass {} 

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert vertex:vert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#include "Flow.cginc"

		sampler2D _MainTex;
		sampler2D _FlowMap;
		sampler2D _GrabTexture;

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

 		struct Input {
			float2 uv_MainTex;
            float4 grabUV;
			//float main 
        };
 
        void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
            float4 hpos = UnityObjectToClipPos (v.vertex);
            o.grabUV = ComputeGrabScreenPos(hpos);
        }

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			float time = _Time.y * _Speed;

			// float2 grabTexCoord = IN.screenPos.xy / IN.screenPos.w;
			// grabTexCoord.y = 1.0f - grabTexCoord.y;
			float2 grabTexCoord = UNITY_PROJ_COORD(IN.grabUV);


			float2 flowVector = tex2D(_FlowMap, IN.grabUV).rg * 2 - 1;	// 0 - 1  >> -1 ~ 1
			float3 uvwA = FlowUVW(IN.grabUV, flowVector, time, 0);
			float3 uvwB = FlowUVW(IN.grabUV, flowVector, time, 0.5);

			fixed4 texA = tex2Dproj (_GrabTexture, uvwA.xy) * uvwA.z;	// uvw.z is control the brightness
			fixed4 texB = tex2Dproj (_GrabTexture, uvwB.xy) * uvwB.z;	// uvw.z is control the brightness

			fixed4 c = (texA + texB) * _Color;
			//fixed4 c = tex2Dproj (_GrabTexture, IN.grabUV);
			o.Albedo = c.rgb;
			//o.Albedo = float3(flowVector, 0);	// for debug flowmap


			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			//o.Alpha = c.a;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
