Shader "Custom/Basic/TextureShader"
{
	Properties{
		_MainTex("Texture", 2D) = "white"{}
	_BumpMap("BumpMap", 2D) = "bump"{}
	}

		SubShader{
		Tags{"RenderType" = "Opaque"}
		CGPROGRAM
#pragma surface surf Lambert
	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		//float4 color : COLOR;
	};
	sampler2D _MainTex;
	sampler2D _BumpMap;

	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		//o.Albedo = 1;
	}

	ENDCG

	}
		FallBack "Diffuse"
}
