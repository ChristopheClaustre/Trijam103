Shader "Hidden/PP/Zawarudo"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DistanceMax("Zawarudo Limit max", Float) = 0.0
        _DistanceMin("Zawarudo Limit min", Float) = 0.0
        _DistanceSafe("Zawarudo safe zone radius", Float) = 0.0
        _LimitWidth("Zawarudo Limit width", Float) = 0.001
        _LimitColor("Limit color (RGB)", Color) = (1,1,1)
        _HFov("Horizontal Aperture", Float) = 0.0
        _VFov("Vertical Aperture", Float) = 0.0
    }
    
    SubShader
    {
        Pass
        {
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "HSV_RGB.cginc"

            uniform sampler2D _MainTex;
            uniform sampler2D _CameraDepthTexture;
            
            float _DistanceMax;
            float _DistanceMin;
            float _DistanceSafe;
            float _HFov;
            float _VFov;

            float3 _LimitColor;
            float _LimitWidth;

            float4 frag(v2f_img i) : COLOR
            {
                float4 rgba = tex2D(_MainTex, i.uv);
                float3 rgb = rgba.rgb;
                float3 grayscale = rgb2grayscale(rgb);

                float depthFixed = 0;
                {
                    float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                    depth = Linear01Depth(depth);

                    // correct depth perspective
                    depthFixed = depth / cos((i.uv.x - 0.5) * _HFov) * cos((i.uv.y) * _VFov / 2);
                }

                float3 pulsatingColor;
                {
                    float3 hsv = rgb2hsv(rgb);
                    hsv.x = (hsv.x + _Time.y + depthFixed) % 1;
                    pulsatingColor = hsv2rgb(hsv);
                    pulsatingColor = float3(pulsatingColor.rgb + frac(-_Time.y + 10 * depthFixed * UNITY_PI) / 2);
                }

                float3 result = depthFixed < _DistanceMax + _LimitWidth ? _LimitColor : rgb;
                result = depthFixed < _DistanceMax - _LimitWidth ? pulsatingColor : result;
                result = depthFixed < _DistanceMin + _LimitWidth ? _LimitColor : result;
                result = depthFixed < _DistanceMin - _LimitWidth ? grayscale : result;
                result = depthFixed < _DistanceSafe ? rgb : result;

                return float4(result, rgba.a);
            }
            ENDCG
        }
    }
}
