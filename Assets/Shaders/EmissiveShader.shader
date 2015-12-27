Shader "Custom/Basic/EmissiveShader"
{
	Properties{
		//All the maps and settings we need
		_MainTex("Texture", 2D) = "white"{}
		_BumpMap("Bumpmap", 2D) = "bump"{}
		_SpecMap("Specularmap", 2D) = "black"{}
		_SpecColor("Specular Color", Color) = (0.5,0.5,0.5,1.0)
		_SpecPower("SPecular Power", Range(0, 1)) = 0.5
		_EmitMap("EmissiveMap", 2D) = "black"{}
		_EmitPower("Emit Power", Range(0,2)) = 0.0
	}

			SubShader{
				Tags{"RenderType" = "Opaque"}
				CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma exclude_renderers flash

				sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _SpecMap;
			float _SpecPower;
			sampler2D _EmitMap;
			float _EmitPower;

			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float2 uv_SpecMap;
				float2 uv_EmitMap;
				//float4 color : COLOR;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				//o.Albedo = 1;
				fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 specTex = tex2D(_SpecMap, IN.uv_MainTex);
				fixed4 emitTex = tex2D(_EmitMap, IN.uv_EmitMap);
				o.Albedo = tex.rgb;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Specular = _SpecPower;
				o.Gloss = specTex.rgb;
				o.Emission = emitTex.rgb * _EmitPower;
			}

			ENDCG

		}
			FallBack "Specular"
}
