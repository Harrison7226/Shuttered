Shader "Custom/PostProcessGlitchShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1, 0, 0, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _TintColor;

            fixed4 frag (v2f_img i) : SV_Target
            {
                // Sample the base texture
                fixed4 color = tex2D(_MainTex, i.uv);

                // Apply the tint color
                color.rgb *= _TintColor.rgb;

                return color;
            }
            ENDCG
        }
    }
}
