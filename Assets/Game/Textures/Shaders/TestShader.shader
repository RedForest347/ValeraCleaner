Shader "Custom/TestShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _NormalMapTex ("Normal Map Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _LightOrigin("Light Origin", vector) = (1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex       : POSITION;
                float4 color        : COLOR;
                float2 texcoord     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex       : SV_POSITION;
                fixed4 color        : COLOR;
                float4 objCoords : TEXCOORD1;
                half2 texcoord      : TEXCOORD0;
            };

            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _NormalMapTex;
            vector _LightOrigin;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.objCoords = mul(unity_ObjectToWorld, IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 out_color;

                fixed4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                color.rgb *= color.a;

                fixed4 normal_map = tex2D(_NormalMapTex, IN.texcoord);
                normal_map.rgb = (normal_map.rgb - 0.5) * 2;
                normal_map.rgb *= normal_map.a;
                //normal_map.rgb = normalize(normal_map.rgb);
                fixed3 normal = normal_map.rgb;

                float LightDist = distance(_LightOrigin.xy, IN.objCoords.xy);
                float intensity = _LightOrigin.w / (LightDist + 1);
                if(intensity > 1)
                    intensity = 1;
                color.rgb = color.rgb * intensity;

                //float intensity = dot((_LightOrigin - IN.objCoords), normal);
                //color.rgb = color.rgb * intensity;
                //color.rgb = fixed3(1, 1, 1) * intensity;
                //color.rgb = intensity;
                //color.rgb = normal_map;
                

                //out_color = fixed4(normal, 1);
                out_color = color;

                return out_color;
            }

        ENDCG
        }
    }
}
