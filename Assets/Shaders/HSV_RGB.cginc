#ifndef HSV_RGB_INCLUDED
#define HSV_RGB_INCLUDED

float3 rgb2hsv(float3 rgb)
{
    float M = max(rgb.r, max(rgb.g, rgb.b));
    float m = min(rgb.r, min(rgb.g, rgb.b));
    float C = M - m;

    float H = 0;
    float S = 0;
    float V = M; // Val
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
    return float3(H,S,V);
}

float4 rgb2hsv(float4 rgb)
{
    return float4(rgb2hsv(rgb.rgb), rgb.a);
}

float3 hsv2rgb(float3 hsv)
{
    float R = abs(hsv.x * 6 - 3) - 1;
    float G = 2 - abs(hsv.x * 6 - 2);
    float B = 2 - abs(hsv.x * 6 - 4);

    float3 RGB = saturate(float3(R, G, B));
    RGB = ((RGB - 1) * hsv.y + 1) * hsv.z;

    return RGB;
}

float4 hsv2rgb(float4 hsv)
{
    return float4(hsv2rgb(hsv.xyz), hsv.a);
}

float3 negative_hsv(float3 hsv)
{
    hsv.x = (hsv.x + 0.5) % 1;
    return hsv;
}

float4 negative_hsv(float4 hsv)
{
    return float4(negative_hsv(hsv.xyz), hsv.a);
}

float3 negative(float3 rgb)
{
    return hsv2rgb(negative_hsv(rgb2hsv(rgb)));
}

float4 negative(float4 rgb)
{
    return float4(negative(rgb.rgb), rgb.a);
}

float3 rgb2grayscale(float3 rgb)
{
    float lum = 0.299 * rgb.r + 0.587 * rgb.g + 0.114 * rgb.b;
    return float3(lum, lum, lum);
}

float4 rgb2grayscale(float4 rgba)
{
    return float4(rgb2grayscale(rgba.rgb), rgba.a);
}

#endif // HSV_RGB_INCLUDED
