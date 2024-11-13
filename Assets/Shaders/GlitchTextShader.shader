Shader "Custom/GlitchTextShader"
{
    Properties
    {
        _MainTex ("Font Texture", 2D) = "white" {}
        _FaceColor ("Text Color", Color) = (1, 1, 1, 1)
        _GlitchOffset ("Glitch Offset", Float) = 5.0
        _EdgeThreshold ("Edge Threshold", Float) = 0.5
        _EdgeSoftness ("Edge Softness", Float) = 0.1
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Lighting Off
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _FaceColor;
            float _GlitchOffset;
            float _EdgeThreshold;
            float _EdgeSoftness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate UV offsets for RGB channels
                float2 uvRed = i.uv + float2(_GlitchOffset / _ScreenParams.x, 0);
                float2 uvGreen = i.uv - float2(_GlitchOffset / _ScreenParams.x, 0);
                float2 uvBlue = i.uv + float2(0, _GlitchOffset / _ScreenParams.y);

                // Sample the SDF texture for each color channel
                float redSDF = tex2D(_MainTex, uvRed).a;
                float greenSDF = tex2D(_MainTex, uvGreen).a;
                float blueSDF = tex2D(_MainTex, uvBlue).a;

                // Use smoothstep with proper thresholds for sharp edges
                float redEdge = smoothstep(_EdgeThreshold - _EdgeSoftness, _EdgeThreshold + _EdgeSoftness, redSDF);
                float greenEdge = smoothstep(_EdgeThreshold - _EdgeSoftness, _EdgeThreshold + _EdgeSoftness, greenSDF);
                float blueEdge = smoothstep(_EdgeThreshold - _EdgeSoftness, _EdgeThreshold + _EdgeSoftness, blueSDF);

                // Combine the RGB channels
                fixed4 color;
                color.r = redEdge * _FaceColor.r;
                color.g = greenEdge * _FaceColor.g;
                color.b = blueEdge * _FaceColor.b;
                color.a = max(max(redEdge, greenEdge), blueEdge);

                return color;
            }
            ENDCG
        }
    }
}
