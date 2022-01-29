#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define MAXLIGHT 50

Texture2D SpriteTexture;

float2  ScreenDimensions;
float2  PointLightPosition[MAXLIGHT];
float4  PointLightColor[MAXLIGHT];
float	PointLightRadius[MAXLIGHT];
float	PointLightIntensity[MAXLIGHT];

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION0;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

// This avoids an overflow that happens because floats are capped at 2^14
float GetDistSafe(float2 Point1, float2 Point2)
{
	float2 adjustedPoint1 = Point1 / ScreenDimensions.y;
	float2 adjustedPoint2 = Point2 / ScreenDimensions.y;
	
	float2 diffs = adjustedPoint2 - adjustedPoint1;
	
	float interTerm = dot(diffs, diffs);
	float adjustedDist = sqrt(interTerm);

	return adjustedDist * ScreenDimensions.y;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color * .15;

	for (int ii = 0; ii < MAXLIGHT; ii++)
	{
		if (PointLightRadius[ii] > 0)
		{
			float2 thisPixelPos = float2(input.TextureCoordinates.x * ScreenDimensions.x, input.TextureCoordinates.y * ScreenDimensions.y);
			float dist = GetDistSafe(thisPixelPos, PointLightPosition[ii].xy);
			if (dist <= PointLightRadius[ii])
			{
                float fog = 1 - clamp(dist / PointLightRadius[ii], 0.0, 1.0);
                float intensity = PointLightIntensity[ii];
                float3 adjustedLighting = float3(PointLightColor[ii].r * intensity, PointLightColor[ii].g * intensity, PointLightColor[ii].b * intensity);
                color *= float4(lerp(float3(1.0, 1.0, 1.0), adjustedLighting, fog), 1.0);
            }
			//if (dist <= 4)
			//{
				//color = PointLightColor[ii];
			//}
		}
	}

	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};