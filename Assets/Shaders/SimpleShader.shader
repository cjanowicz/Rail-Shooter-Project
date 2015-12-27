Shader "Custom/Basic/SimpleShader"
{
	SubShader
	{
		//note: On super simple shaders, you don't need properties.
		Tags {"RenderType" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf Lambert

		struct Input {
			float4 color : COLOR;
		};
		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = 1;
		}

		ENDCG

	}
		FallBack "Diffuse"
}
