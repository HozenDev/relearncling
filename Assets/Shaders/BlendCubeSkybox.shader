Shader "Custom/Blended Cubemap Skybox"
{
    Properties
    {
        _Tint("Tint Color", Color) = (1,1,1,1)
        _Exposure("Exposure", Range(0, 8)) = 1.0
        _Rotation("Rotation", Range(0, 360)) = 0

        _Blend("Blend (0=Clean,1=Polluted)", Range(0,1)) = 0

        _CubemapClean("Cubemap (Clean)", CUBE) = "" {}
        _CubemapPolluted("Cubemap (Polluted)", CUBE) = "" {}
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off
        ZWrite Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Tint;
            float _Exposure;
            float _Rotation;
            float _Blend;

            samplerCUBE _CubemapClean;
            samplerCUBE _CubemapPolluted;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 dir : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Vertex direction (cube around camera)
                float3 dir = normalize(v.vertex.xyz);

                // Rotate around Y axis
                float rotRad = radians(_Rotation);
                float cosR = cos(rotRad);
                float sinR = sin(rotRad);

                float3x3 rotY = float3x3(
                    cosR, 0, -sinR,
                    0,    1,  0,
                    sinR, 0,  cosR
                );

                o.dir = mul(rotY, dir);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 dir = normalize(i.dir);

                fixed4 colClean    = texCUBE(_CubemapClean, dir);
                fixed4 colPolluted = texCUBE(_CubemapPolluted, dir);

                fixed4 col = lerp(colClean, colPolluted, _Blend);
                col.rgb *= _Tint.rgb * _Exposure;
                col.a = 1.0;

                return col;
            }
            ENDCG
        }
    }

    FallBack Off
}
