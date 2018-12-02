shader "Custom/distort" {
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DisplaceTex("DisplaceTexS (RGB)", 2D) = "white" {}
        _Amount("Amount", Range(0,0.5)) = 0
    }
    SubShader{
        Pass{
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform sampler2D _DisplaceTex;
            float _Amount;

            float4 frag(v2f_img i) : COLOR
            {
                //float4 color = tex2D(_DisplaceTex, i.uv);
                //return color;
                float4 displace = tex2D(_DisplaceTex, i.uv);
                displace = displace * 2 - 1;

                i.uv[0] += (displace.b) * _Amount;
                i.uv = saturate(i.uv);

                float4 c = tex2D(_MainTex, i.uv);
                return c;

            }
            ENDCG
        }
    }
}
