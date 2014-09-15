// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "Transparent/Bumped Trans Gloss" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap ("Bumpmap", 2D) = "bump" {}
	_GlossMap ("Glossmap (A)", 2D) = "black" {}
}

Category {
	Tags {Queue=Transparent}
	Alphatest Greater 0
	Fog { Color [_AddFog] }
	ZWrite Off
	ColorMask RGB
	
	// ------------------------------------------------------------------
	// ARB fragment program
	
	#warning Upgrade NOTE: SubShader commented out; uses Unity 2.x per-pixel lighting. You should rewrite shader into a Surface Shader.
/*SubShader {
		UsePass "Transparent/Specular/BASE"
		
		// Pixel lights
		Pass {
			Blend SrcAlpha One
			Name "PPL"
			Tags { "LightMode" = "Pixel" }
CGPROGRAM
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members uvK,uv2,viewDirT,lightDirT)
#pragma exclude_renderers xbox360
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_builtin_noshadows
#pragma fragmentoption ARB_fog_exp2
#pragma fragmentoption ARB_precision_hint_fastest

#include "UnityCG.cginc"
#include "AutoLight.cginc" 

struct v2f {
	float4 pos : SV_POSITION;
	LIGHTING_COORDS
	float3	uvK; // xy = UV, z = specular K
	float2	uv2;
	float3	viewDirT;
	float3	lightDirT;
}; 

uniform float4 _MainTex_ST, _BumpMap_ST;
uniform float _Shininess;


v2f vert (appdata_tan v)
{	
	v2f o;
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.uvK.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
	o.uvK.z = _Shininess * 128;
	o.uv2 = TRANSFORM_TEX(v.texcoord,_BumpMap);

	TANGENT_SPACE_ROTATION;
	o.lightDirT = mul( rotation, ObjSpaceLightDir( v.vertex ) );	
	o.viewDirT = mul( rotation, ObjSpaceViewDir( v.vertex ) );	
	
	TRANSFER_VERTEX_TO_FRAGMENT(o);
	return o;
}

uniform sampler2D _GlossMap;
uniform sampler2D _BumpMap;
uniform sampler2D _MainTex;
uniform float4 _Color;

float4 frag (v2f i) : COLOR
{		
	float4 texcol = tex2D( _MainTex, i.uvK.xy );
	
	// get normal from the normal map
	float3 normal = tex2D(_BumpMap, i.uv2).xyz * 2 - 1;
	
	float gloss = tex2D(_GlossMap, i.uv2).xyz;
	
	half4 c;
	c.a = texcol.a * _Color.a;
	
	texcol.a = gloss;
	c.rgb = SpecularLight( i.lightDirT, i.viewDirT, normal, texcol, i.uvK.z, LIGHT_ATTENUATION(i) ).rgb;
	
	return c;
}
ENDCG  
		}
	}*/
}

FallBack "Transparent/Bumped Specular"

}