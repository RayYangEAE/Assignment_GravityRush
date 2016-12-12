Shader "Custom/DoubleSided_Opaque" {

	Properties {
        _Color ("Main Tint", Color) = (1,1,1,1)
        _MainTex ("Base (RGBA)", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
    	_BackColor ("Back Main Color", Color) = (1,1,1,1)
    	_BackMainTex ("Back Base (RGB) Gloss (A)", 2D) = "white" {}
    }

    SubShader { 
    Tags {"RenderType"="Opaque"}
    LOD 200

    Cull Back
   
	CGPROGRAM
	#pragma surface surf Lambert

	sampler2D _MainTex;
	sampler2D _BumpMap;
	fixed4 _Color;

	struct Input {
    	float2 uv_MainTex;
    	float2 uv_BumpMap;
	};

	void surf (Input IN, inout SurfaceOutput o) {
    	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
    	o.Albedo = tex.rgb * _Color.rgb;
    	o.Gloss = tex.a;
    	o.Alpha = tex.a * _Color.a;
    	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
    }
	ENDCG

	Tags {"RenderType"="Opaque"}
    LOD 200

    Cull front

	CGPROGRAM
	#pragma surface surf Lambert

	sampler2D _BackMainTex;
	sampler2D _BumpMap;
	fixed4 _BackColor;

	struct Input {
    	float2 uv_BackMainTex;
    	float2 uv_BumpMap;
	};

	void surf (Input IN, inout SurfaceOutput o) {
    	fixed4 tex = tex2D(_BackMainTex, IN.uv_BackMainTex);
    	o.Albedo = tex.rgb * _BackColor.rgb;
    	o.Gloss = tex.a;
    	o.Alpha = tex.a * _BackColor.a;
    	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
    }
	ENDCG

	}

    FallBack "Diffuse"

}
