// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GrayscaleShader"
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

			#include "UnityCG.cginc"

			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float2 texcoord : TEXCOORD0;
				float4 position : SV_POSITION;
			};

			vertexOutput vertexShader(vertexInput i)
			{
				vertexOutput o;
				o.position = UnityObjectToClipPos(i.vertex);
				o.texcoord = i.texcoord;
				return o;
			}

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;

			float4 fragmentShader(vertexOutput i) : COLOR
			{
				float4 color = tex2D(_MainTex, i.texcoord);

				float gray = color.r + color.g + color.b;
				gray /= 3.0f;
				//color = lerp(color, float4(gray, gray, gray, 0.0f), 10);
				color = float4(gray, gray, gray, 0.0f);
				return color;
			}
			ENDCG
		}
	}
	Fallback Off
}