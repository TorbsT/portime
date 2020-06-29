Shader "Custom/RewindShader"
{

    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f i) : SV_Target
            {
                //float screenX = i.vertex.x / _ScreenParams.x;
                //float screenY = i.vertex.y / _ScreenParams.y;
                fixed4 col = tex2D(_MainTex, i.uv + float2(0, sin(i.vertex.x/25 + _Time[1]*10)/100));
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                
                float colorImpact = 0.45f;
                col.r = col.r * (1 - colorImpact) + (1 - col.g) * colorImpact;
                col.b = col.b * (1 - colorImpact) + (1 - col.g) * colorImpact;
                col.g = col.g * (1 - colorImpact) + (1) * colorImpact;
                return col;
            }
            ENDCG
        }
    }
}
