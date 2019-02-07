#if !defined(FLOW_INCLUDED)
#define FLOW_INCLUDED

float2 FlowUV (float2 uv, float2 flowVector, float time) {
    float progress = frac(time);        // progress = 0 ~ 1
    //float progress =
	return uv - flowVector * progress; // * progress;  // time;
    // return uv - flowVector + time; // * progress;  // time;
    //return uv - flowVector * time; // * progress;  // time;
}

float3 FlowUVW (float2 uv, float2 flowVector, float time) {
	float progress = frac(time);
	float3 uvw;
	uvw.xy = uv - flowVector * progress;
	uvw.z = 1 - abs(1 - 2 * progress);      // 1 - abs(1 - 2 * {0 ~ 1}) => 1 - {1 ~ 0 ~ 1}
	return uvw;
}

// Add phaseOffset param so that can blend two texture
float3 FlowUVW (float2 uv, float2 flowVector, float time, float phaseOffset) {
	float progress = frac(time + phaseOffset);
	float3 uvw;
	uvw.xy = uv - flowVector * progress + phaseOffset;
	uvw.z = 1 - abs(1 - 2 * progress);      // 1 - abs(1 - 2 * {0 ~ 1}) => 1 - {1 ~ 0 ~ 1}
	return uvw;
}

float3 FlowUVW (float2 uv, float2 flowVector, float2 jump, float time, float phaseOffset) {
	float progress = frac(time + phaseOffset);
	float3 uvw;
	uvw.xy = uv - flowVector * progress + phaseOffset;
    uvw.xy += (time - progress) * jump;     // Jump to next phase
	
	uvw.z = 1 - abs(1 - 2 * progress);      // 1 - abs(1 - 2 * {0 ~ 1}) => 1 - {1 ~ 0 ~ 1}
	return uvw;
}

float3 FlowUVW (float2 uv, float2 flowVector, float flowOffset, 
                float tiling, float time, float2 jump, float phaseOffset) {
	float progress = frac(time + phaseOffset);
	float3 uvw;
    
    uvw.xy = uv - flowVector * (progress + flowOffset);
    uvw.xy *= tiling;
    uvw.xy += phaseOffset;
    uvw.xy += (time - progress) * jump;     // Jump to next phase
	
	uvw.z = 1 - abs(1 - 2 * progress);      // 1 - abs(1 - 2 * {0 ~ 1}) => 1 - {1 ~ 0 ~ 1}
	return uvw;
}

float2 MoveUV (float2 uv, float time) {
	return uv + time;
}

float2 MoveUV (float2 uv, float2 offset) {
	return uv + offset;
}

#endif