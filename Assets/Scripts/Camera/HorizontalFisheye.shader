Shader "Custom/HorizontalFisheye"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Strength ("Distortion Strength", Range(-1, 1)) = 0.3
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Center UV (0.5, 0.5)
                float2 centered = i.uv - 0.5;
                
                // Horizontal distortion: shift x more when far from center
                // Factor = (abs(centered.x) * 2)  // 0 at center, 1 at edges
                float factor = abs(centered.x) * 2.0;
                float distortedX = centered.x * (1.0 + _Strength * factor);
                
                // Keep y unchanged
                float2 distortedUV = float2(distortedX + 0.5, i.uv.y);
                
                // Clamp to avoid artifacts at edges
                distortedUV = clamp(distortedUV, 0.001, 0.999);
                
                fixed4 col = tex2D(_MainTex, distortedUV);
                return col;
            }
            ENDCG
        }
    }
}