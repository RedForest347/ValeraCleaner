﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/AlphaGradient"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [MaterialToggle] _UseFade("Use Fade", Float) = 0
        _FadeOrigin("Fade Origin", vector) = (1,1,1)
        _FadeAreaMin("Fade Area Min", float) = 1.5
        _FadeAreaMax("Fade Area Max", float) = 2.5
        _MaxOpacity("Max Opacity", Range(0,1)) = 0.5
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
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    float4 objCoords : TEXCOORD1;
                    fixed4 color : COLOR;
                    half2 texcoord  : TEXCOORD0;
                };

                fixed4 _Color;

                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.objCoords = mul(unity_ObjectToWorld, IN.vertex);
                    OUT.texcoord = IN.texcoord;
                    OUT.color = IN.color * _Color;
                    #ifdef PIXELSNAP_ON
                    OUT.vertex = UnityPixelSnap(OUT.vertex);
                    #endif

                    return OUT;
                }

                sampler2D _MainTex;
                float _UseFade;
                float _FadeAreaMin;
                float _FadeAreaMax;
                vector _FadeOrigin;
                float _MaxOpacity;

                fixed4 frag(v2f IN) : SV_Target
                {
                    fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
                    float percent = 1;

                    if (_UseFade == 1)
                    {
                        float pixDist = distance(_FadeOrigin.xy, IN.objCoords.xy);

                        if (pixDist < _FadeAreaMin)
                        {
                            percent = _MaxOpacity;
                        }
                        else
                        {
                            if (_FadeAreaMin > _FadeAreaMax)
                            {
                                _FadeAreaMin = _FadeAreaMax;
                            }

                            if (_FadeAreaMax < _FadeAreaMin)
                            {
                                _FadeAreaMax = _FadeAreaMin;
                            }

                            if (pixDist > _FadeAreaMin && pixDist < _FadeAreaMax)
                            {
                                float fadeAreaSize = _FadeAreaMax - _FadeAreaMin;
                                float pixPos = pixDist - _FadeAreaMin;
                                float fadePercent = pixPos / fadeAreaSize;

                                if (fadePercent < _MaxOpacity)
                                {
                                    fadePercent = _MaxOpacity;
                                }

                                percent = fadePercent;
                            }
                        }
                    }

                    c.rgba *= c.a * percent;

                    return c;
                }
            ENDCG
            }
        }
}