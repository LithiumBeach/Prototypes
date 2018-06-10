// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/MetaballsShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"


			uniform float4x4 _FrustumCorners;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform float4x4 _CameraInvViewMatrix;
			uniform float3 _CameraWorldPos;

			//const
			//const int MAX_METABALLS = 100;
			int MAX_MARCHING_STEPS;
			float MIN_DIST;
			float MAX_DIST;
			float EPSILON;
			//array of all metaballs
			float4 _MBPositions[100]; //todo: use MAX_METABALLS?
			float _MBRadii[100];
			int _MBCount;
			//camera FOV
			float4 _FOV;


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float3 ray : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;

				// Index passed via custom blit function in RaymarchGeneric.cs
				half index = v.vertex.z;
				v.vertex.z = 0.1;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;

#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
#endif

				// Get the eyespace view ray (normalized)
				o.ray = _FrustumCorners[(int)index].xyz;

				// Transform the ray from eyespace to worldspace
				// Note: _CameraInvViewMatrix was provided by the script
				o.ray = mul(_CameraInvViewMatrix, o.ray);
				return o;
			}



			float sphereSDF(float3 samplePoint, float3 spherePos, float r)
			{
				return (length(samplePoint - spherePos) - r);
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
				float sum = 0.0f;
				float mbAccum = 0.0f;
				float prevMBAccum = 0.0f;

				for (int i = 0; i < _MBCount; i++)
				{
					mbAccum = (_MBRadii[i] * _MBRadii[i]) / (
						(_MBPositions[i].x - samplePoint.x) * (_MBPositions[i].x - samplePoint.x) +
						(_MBPositions[i].y - samplePoint.y) * (_MBPositions[i].y - samplePoint.y) +
						(_MBPositions[i].z - samplePoint.z) * (_MBPositions[i].z - samplePoint.z));

					//accumulate SDFs * associated MB samplings
					sum += mbAccum * sphereSDF(samplePoint, _MBPositions[i].xyz, _MBRadii[i]);

				}
				
				return sum;
			}


			/**
			* Return the shortest distance from the eyepoint to the scene surface along
			* the marching direction. If no part of the surface is found between start and end,
			* return end.
			*
			* eye: the eye point, acting as the origin of the ray
			* marchingDirection: the normalized direction to march in
			* start: the starting distance away from the eye
			* end: the max distance away from the eye to march before giving up
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
					depth += dist;
					if (depth >= end)
					{
						return end;
					}
				}
				return end;
			}

			fixed4 frag (v2f i) : SV_Target
			{

				//fixed4 col = fixed4(i.ray, 1);
				//return col;

				float3 rayDir = normalize(i.ray.xyz);
				float dist = shortestDistanceToSurface(_CameraWorldPos, rayDir, 0, MAX_DIST);

				if (dist >= MAX_DIST)
				{
					// Didn't hit anything -- return color before this shader.
					return tex2D(_MainTex, i.uv);
				}
				float3 color = float3(0.2, 1.0, 0.0);


				return float4(color, 1.0);//lerp(float4(color, .8), tex2D(_MainTex, i.uv), .5);


			}
			ENDCG
		}
	}
}
