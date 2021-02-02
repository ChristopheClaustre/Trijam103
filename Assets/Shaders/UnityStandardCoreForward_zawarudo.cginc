// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

#ifndef UNITY_STANDARD_CORE_FORWARD_ZAWARUDO_INCLUDED
#define UNITY_STANDARD_CORE_FORWARD_ZAWARUDO_INCLUDED

#if defined(UNITY_NO_FULL_STANDARD_SHADER)
#   define UNITY_STANDARD_SIMPLE 1
#endif

#include "UnityStandardConfig.cginc"

#if UNITY_STANDARD_SIMPLE
    #include "UnityStandardCoreForwardSimple.cginc"
    VertexOutputBaseSimple vertBase (VertexInput v) { return vertForwardBaseSimple(v); }
    VertexOutputForwardAddSimple vertAdd (VertexInput v) { return vertForwardAddSimple(v); }
    half4 fragBase (VertexOutputBaseSimple i) : SV_Target { return fragForwardBaseSimpleInternal(i); }
    half4 fragAdd (VertexOutputForwardAddSimple i) : SV_Target { return fragForwardAddSimpleInternal(i); }
#else
    #include "UnityStandardCore.cginc"
    VertexOutputForwardBase vertBase (VertexInput v) { return vertForwardBase(v); }
    VertexOutputForwardAdd vertAdd (VertexInput v) { return vertForwardAdd(v); }
    half4 fragBase (VertexOutputForwardBase i) : SV_Target { return fragForwardBaseInternal(i); }
    half4 fragAdd (VertexOutputForwardAdd i) : SV_Target { return fragForwardAddInternal(i); }
#endif

half4 rgb2hsv(half4 rgb)
{
    half M = max(rgb.r, max(rgb.g, rgb.b));
    half m = min(rgb.r, min(rgb.g, rgb.b));
    half C = M - m;

    half H = 0;
    half S = 0;
    half V = M; // Val
    if (C != 0)
    {
        // Hue
        float3 Delta = (V - rgb.rgb) / C;
        Delta.rgb -= Delta.brg;
        Delta.rgb += float3(2, 4, 6);
        Delta.brg = step(V, rgb.rgb) * Delta.brg;

        H = max(Delta.r, max(Delta.g, Delta.b));
        H = frac(H / 6);

        // Sat
        S = C / M;
    }
    return half4(H,S,V,rgb.a);
}

half4 hsv2rgb(half4 hsv)
{
    half R = abs(hsv.x * 6 - 3) - 1;
    half G = 2 - abs(hsv.x * 6 - 2);
    half B = 2 - abs(hsv.x * 6 - 4);

    half3 RGB = saturate(half3(R,G,B));
    RGB = ((RGB - 1) * hsv.y + 1) * hsv.z;

    return half4(RGB, hsv.a);
}

half4 negative(half4 color)
{
    half4 hsv = rgb2hsv(color);
    hsv.x = (hsv.x + 0.5) % 1;
    return hsv2rgb(hsv);
}

float _FreezeTime = 0;
int _ReversedFreeze = 0;

half4 fragZawarudo(VertexOutputForwardBase i) : SV_Target
{
    half4 rgb = fragForwardBaseInternal(i);
    half4 grayscale;
    grayscale.rgb = 0.299 * rgb.r + 0.587 * rgb.g + 0.114 * rgb.b;
    grayscale.a = rgb.a;

    float len = length(i.eyeVec);
    float sinceTime = _Time.y - _FreezeTime;

    //sinceTime = clamp(_ReversedFreeze == 0 ? sinceTime : 3 - (2*sinceTime), 0, 3);
    half4 result = rgb;

    if (_FreezeTime > 0)
    {
        result = pow(sinceTime, 2) < len ? rgb : negative(rgb) + pow(cos(len + sinceTime * 5), 6) * 0.5;
    }

    return result;
}

#endif // UNITY_STANDARD_CORE_FORWARD_INCLUDED
