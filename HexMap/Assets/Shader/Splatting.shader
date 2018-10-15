Shader "Custom/VetexColor Splatting"
{
	Properties
	{
		_MainTint("Tint", Color) = (1,1,1,1)
		_Texture1("Texture 1", 2D) = "" {}
		_Texture2("Texture 2", 2D) = "" {}
		_Texture3("Texture 3", 2D) = "" {}
		_Texture4("Texture 4", 2D) = "" {}
		_Texture5("Texture 5", 2D) = "" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		float4 _MainTint;
		sampler2D _Texture1;
		sampler2D _Texture2;
		sampler2D _Texture3;
		sampler2D _Texture4;
		sampler2D _Texture5;

		struct Input
		{
			float2 uv_MainTex;
			float4 col;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			//纹理采样
			float4 texColor1 = tex2D(_Texture1, IN.uv_MainTex);
			float4 texColor2 = tex2D(_Texture2, IN.uv_MainTex);
			float4 texColor3 = tex2D(_Texture3, IN.uv_MainTex);
			float4 texColor4 = tex2D(_Texture4, IN.uv_MainTex);
			float4 texColor5 = tex2D(_Texture5, IN.uv_MainTex);

			//使用lerp()函数(线性插值)对纹理进行混合。

			float4 finalColor;
			finalColor = lerp(texColor1, texColor2, IN.col.r);
			finalColor = lerp(finalColor, texColor3, IN.col.g);
			finalColor = lerp(finalColor, texColor4, IN.col.b);
			finalColor = lerp(finalColor, texColor5, IN.col.a);
			finalColor.a = 1.0;

			o.Albedo = finalColor.rgb * _MainTint.rgb;
			o.Alpha = finalColor.a;
		}

		ENDCG
	}
	
	FallBack "Diffuse"
}