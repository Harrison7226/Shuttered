Shader "Custom/ScanLineShader"
{
    Properties
    {
        _MainTex ("Font Texture", 2D) = "white" {}
        _FaceColor ("Face Color", Color) = (1, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _WarpStrength ("Warp Strength", Float) = 10.0
        _Frequency ("Warp Frequency", Float) = 5.0
        _Speed ("Warp Speed", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _FaceColor;
            float4 _OutlineColor;
            float _WarpStrength;
            float _Frequency;
            float _Speed;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the SDF font texture
                fixed4 sdfSample = tex2D(_MainTex, i.uv);

                // Use smoothstep for sharp edges based on the SDF value
                float sdf = sdfSample.a;
                float alpha = smoothstep(0.45, 0.55, sdf);

                // Apply the face color with the alpha
                fixed4 col = _FaceColor;
                col.a *= alpha;

                // Calculate a horizontal wave distortion
                float wave = sin(i.uv.y * _Frequency + _Speed * _Time.y);
                float distortion = wave * _WarpStrength / _ScreenParams.x;

                // Apply the distortion to the UV coordinates
                i.uv.x += distortion;

                // Sample the texture with the modified UVs
                fixed4 finalColor = tex2D(_MainTex, i.uv);
                finalColor.rgb = lerp(_OutlineColor.rgb, col.rgb, alpha);
                finalColor.a = col.a;

                return finalColor;
            }
            ENDCG
        }
    }
}
