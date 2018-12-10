// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Dotted"
{
	Properties
	{
	    _Color("Color", Color) = (1,1,1,1)
		_Speed("Speed", Range(-50,50)) = 0
        _Theta("Theta", Range(-6.283,6.283)) = 0
        _Scale("Scale", Range(0,20)) = 5

	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"

	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
        float3 worldPos : TEXCOORD0;
	};


	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
        OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
		return OUT;
	}

    fixed4 _Color;
    float _Speed;
    float _Theta;
    float _Scale;

	fixed4 frag(v2f IN) : SV_Target
	{
        fixed4 col = _Color;
        float x = IN.worldPos.x;
        float y = IN.worldPos.y;
        x = (x * cos(-1 * _Theta) - y * sin(-1 * _Theta));
        x = round(x * _Scale + _Time.y * _Speed);
        if (x % 2 == 0)
        {
            col.a = 0;
        }
        col*=col.a;
        return col;
	}
		ENDCG
	}
	}
}
