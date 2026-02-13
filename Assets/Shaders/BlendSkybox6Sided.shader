Shader "Custom/Blended Skybox 6 Sided"
{
    Properties
    {
        _Tint("Tint Color", Color) = (1,1,1,1)
        _Exposure("Exposure", Range(0, 8)) = 1.0
        _Rotation("Rotation", Range(0, 360)) = 0

        _Blend("Blend (0=Clean,1=Polluted)", Range(0,1)) = 0

        // Clean textures
        _FrontTexClean ("Front (Clean)", 2D) = "white" {}
        _BackTexClean  ("Back (Clean)", 2D)  = "white" {}
        _LeftTexClean  ("Left (Clean)", 2D)  = "white" {}
        _RightTexClean ("Right (Clean)", 2D) = "white" {}
        _UpTexClean    ("Up (Clean)", 2D)    = "white" {}
        _DownTexClean  ("Down (Clean)", 2D)  = "white" {}

        // Polluted textures
        _FrontTexPolluted ("Front (Polluted)", 2D) = "white" {}
        _BackTexPolluted  ("Back (Polluted)", 2D)  = "white" {}
        _LeftTexPolluted  ("Left (Polluted)", 2D)  = "white" {}
        _RightTexPolluted ("Right (Polluted)", 2D) = "white" {}
        _UpTexPolluted    ("Up (Polluted)", 2D)    = "white" {}
        _DownTexPolluted  ("Down (Polluted)", 2D)  = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off
        Fog { Mode Off }
        ZWrite Off

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

            sampler2D _FrontTexClean;
            sampler2D _BackTexClean;
            sampler2D _LeftTexClean;
            sampler2D _RightTexClean;
            sampler2D _UpTexClean;
            sampler2D _DownTexClean;

            sampler2D _FrontTexPolluted;
            sampler2D _BackTexPolluted;
            sampler2D _LeftTexPolluted;
            sampler2D _RightTexPolluted;
            sampler2D _UpTexPolluted;
            sampler2D _DownTexPolluted;

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

                // Use vertex position as direction.
                // Unity renders the skybox as a cube around the camera,
                // so this naturally follows camera rotation.
                float3 dir = normalize(v.vertex.xyz);

                // Optional rotation around Y axis
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

            // Decide which face to use and compute UVs
            void GetFaceAndUV(float3 dir, out int faceIndex, out float2 uv)
            {
                float3 a = abs(dir);

                if (a.x >= a.y && a.x >= a.z)
                {
                    // X-major
                    if (dir.x > 0)
                    {
                        // +X (Right)
                        faceIndex = 0;
                        uv = float2(-dir.z, dir.y) / a.x;
                    }
                    else
                    {
                        // -X (Left)
                        faceIndex = 1;
                        uv = float2(dir.z, dir.y) / a.x;
                    }
                }
                else if (a.y >= a.x && a.y >= a.z)
                {
                    // Y-major
                    if (dir.y > 0)
                    {
                        // +Y (Up)
                        faceIndex = 2;
                        uv = float2(dir.x, -dir.z) / a.y;
                    }
                    else
                    {
                        // -Y (Down)
                        faceIndex = 3;
                        uv = float2(dir.x, dir.z) / a.y;
                    }
                }
                else
                {
                    // Z-major
                    if (dir.z > 0)
                    {
                        // +Z (Front)
                        faceIndex = 4;
                        uv = float2(dir.x, dir.y) / a.z;
                    }
                    else
                    {
                        // -Z (Back)
                        faceIndex = 5;
                        uv = float2(-dir.x, dir.y) / a.z;
                    }
                }

                // Map from [-1,1] to [0,1]
                uv = uv * 0.5 + 0.5;

                // Edge shrink to hide seams
                const float edge = 0.002; // tweak 0.001–0.005 if needed
                uv = uv * (1.0 - 2.0 * edge) + edge;

                uv = saturate(uv);
            }

            fixed4 SampleBlendedSkybox(float3 dir)
            {
                int face;
                float2 uv;
                GetFaceAndUV(dir, face, uv);

                fixed4 colClean;
                fixed4 colPolluted;

                if (face == 0)       // Right
                {
                    colClean    = tex2D(_RightTexClean, uv);
                    colPolluted = tex2D(_RightTexPolluted, uv);
                }
                else if (face == 1)  // Left
                {
                    colClean    = tex2D(_LeftTexClean, uv);
                    colPolluted = tex2D(_LeftTexPolluted, uv);
                }
                else if (face == 2)  // Up
                {
                    colClean    = tex2D(_UpTexClean, uv);
                    colPolluted = tex2D(_UpTexPolluted, uv);
                }
                else if (face == 3)  // Down
                {
                    colClean    = tex2D(_DownTexClean, uv);
                    colPolluted = tex2D(_DownTexPolluted, uv);
                }
                else if (face == 4)  // Front
                {
                    colClean    = tex2D(_FrontTexClean, uv);
                    colPolluted = tex2D(_FrontTexPolluted, uv);
                }
                else                 // Back
                {
                    colClean    = tex2D(_BackTexClean, uv);
                    colPolluted = tex2D(_BackTexPolluted, uv);
                }

                fixed4 col = lerp(colClean, colPolluted, _Blend);
                col.rgb *= _Tint.rgb * _Exposure;
                col.a = 1.0;
                return col;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return SampleBlendedSkybox(normalize(i.dir));
            }
            ENDCG
        }
    }

    FallBack Off
}
