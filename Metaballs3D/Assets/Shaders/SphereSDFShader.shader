// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Unlit/SphereSDFShader"
{
	Properties
	{
		_MainTex("Source", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vertexShader
			#pragma fragment fragmentShader
			#pragma target 3.0

			#include "UnityCG.cginc"

			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 position : SV_POSITION;
				//float3 ray : TEXCOORD1;
			};

			v2f vertexShader(vertexInput i)
			{
				v2f o;
				//o.position = UnityObjectToClipPos(i.vertex);
				o.position = UnityObjectToClipPos(i.vertex);
				o.uv = i.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;

			//const
			//const int MAX_METABALLS = 100;
			const int MAX_MARCHING_STEPS = 255;
			const float MIN_DIST = 0.0;
			const float MAX_DIST = 100.0;
			const float EPSILON = 0.0001;

			//array of all metaballs
			float4 _MBPositions[100]; //todo: use MAX_METABALLS?
			float4 _MBRadii[100];
			int _MBCount;
			//camera FOV
			float4 _FOV;
			//screen resolution
			//float _ScreenResolutionX;
			//float _ScreenResolutionY;// Signed distance function for a sphere centered at the origin with radius 1.0;
			//camera
			float3 _CameraPos;
			float3 _CameraDir;



			float sphereSDF(float3 samplePoint, float3 spherePos, float r)
			{
				return length(samplePoint - spherePos) - r;
			}

			/**
			* Signed distance function describing the scene.
			*
			* Absolute value of the return value indicates the distance to the surface.
			* Sign indicates whether the point is inside or outside the surface,
			* negative indicating inside.
			*/
			float sceneSDF(float3 samplePoint)
			{
				//float sum = 0.0f;
				//float mbAccum = 0.0f;
				//for (int i = 0; i < _MBCount; i++)
				//{
				//	mbAccum = (_MBRadii[i] * _MBRadii[i]) / (
				//		(_MBPositions[i].x - samplePoint.x) * (_MBPositions[i].x - samplePoint.x) +
				//		(_MBPositions[i].y - samplePoint.y) * (_MBPositions[i].y - samplePoint.y) +
				//		(_MBPositions[i].z - samplePoint.z) * (_MBPositions[i].z - samplePoint.z));
				//
				//	//accumulate SDFs * associated MB samplings
				//	sum += mbAccum * sphereSDF(samplePoint, _MBPositions[i], _MBRadii[i]);
				//}
				//
				//return sum;
				return sphereSDF(samplePoint, float3(0, 0, 0), .25);
			}


			/**
			* Return the shortest distance from the eyepoint to the scene surface along
			* the marching direction. If no part of the surface is found between start and end,
			* return end.
			*
			* eye: the eye point, acting as the origin of the ray
			* marchingDirection: the normalized direction to march in
			* start: the starting distance away from the eye
			* end: the max distance away from the ey to march before giving up
			*/
			float shortestDistanceToSurface(float3 eye, float3 marchingDirection, float start, float end)
			{
				//ray march until inside sceneSDF
				float depth = start;
				for (int i = 0; i < MAX_MARCHING_STEPS; i++)
				{
					float dist = sceneSDF(eye + depth * marchingDirection);
					if (dist < EPSILON)
					{
						return depth;
					}
					//depth keeps increasing until dist < Epsilon
					depth += dist;
					if (depth >= end)
					{
						return end;
					}
				}
				return end;
			}

			/**
			* Return the normalized direction to march in from the eye point for a single pixel.
			*
			* fieldOfView: vertical field of view in degrees
			* size: resolution of the output image
			* fragCoord: the x,y coordinate of the pixel in the output image
			*/
			float3 rayDirection(float fieldOfView, float2 size, float2 fragCoord)
			{
				float2 xy = fragCoord - size / 2.0;
				float z = size.y / tan(radians(fieldOfView) / 2.0);
				return normalize(float3(xy, -z));
			}


			float4 fragmentShader(v2f i) : COLOR
			{
				float4 color = tex2D(_MainTex, i.uv);
				float2 screenPos = i.position;
				float3 dir = rayDirection(_FOV, _ScreenParams.xy, screenPos);
				float dist = shortestDistanceToSurface(_CameraPos, dir, MIN_DIST, MAX_DIST);

				if (dist >= MAX_DIST)
				{
					// Didn't hit anything
					return color;
				}

				return float4(0, 1.0, 0, 0);
			}

			
			ENDCG
		}
	}
	Fallback Off
}