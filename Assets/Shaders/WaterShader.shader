Shader "Custom/WaterShader" {
    Properties {
        _MainTex ("Water Texture", 2D) = "white" {}
        _Color ("Water Color", Color) = (0.2, 0.5, 1.0, 0.5)
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveStrength ("Wave Strength", Float) = 0.1
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _WaveSpeed;
            float _WaveStrength;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                // Animate UV with time for wave effect
                o.uv = v.uv + sin(_Time.y * _WaveSpeed + v.uv.yx * 10.0) * _WaveStrength;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float4 tex = tex2D(_MainTex, i.uv);
                return tex * _Color;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
