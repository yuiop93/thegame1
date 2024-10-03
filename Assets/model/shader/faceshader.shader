Shader "Custom/faceshader"
{
    Properties
    {
        _Darklight("Dark Light", Color) = (0.1, 0.1, 0.1, 1)
        _MainTex("Main Tex", 2D) = "white" {}
        _ShadowTex("Shadow Tex", 2D) = "white" {}
        _Front("Front", Vector) = (0, 0, 0)
        _UP("UP", Vector) = (0, 0, 0)
        _LeftDir("LeftDir", Vector) = (0, 0, 0)
    }
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            float3 _Darklight;
            sampler2D _MainTex;
            sampler2D _ShadowTex;
            float3 _Front;
            float3 _UP;
            float3 _LeftDir;

            struct c2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float2 uv : TEXCOORD;
                float3 objViewDir : COLOR1;
                float3 normal : NORMAL2;
            };

            v2f vert(c2v input)
            {
                v2f output;
                output.pos = UnityObjectToClipPos(input.vertex);
                output.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, input.normal));
                output.uv = input.uv;

                float3 ObjViewDir = normalize(ObjSpaceViewDir(input.vertex));
                output.objViewDir = ObjViewDir;
                output.normal = normalize(input.normal);
                return output;
            }

            fixed3 frag(v2f input) : SV_TARGET0
            {
                fixed3 diffuseColor = _LightColor0.rgb * tex2D(_MainTex, input.uv).rgb;

                // 计算光照方向
                float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                float isShadow = step(dot(input.worldNormal, worldLightDir), 0);

                // 保留现有阴影贴图的逻辑
                float3 RightDir = -_LeftDir;
                float2 Left = normalize(float2(_LeftDir.x, _LeftDir.z));
                float2 Front = normalize(float2(_Front.x, _Front.z));
                float2 LightDir = normalize(float2(worldLightDir.x, worldLightDir.z));

                float ctrl = 1 - (dot(Front, LightDir) * 0.5 + 0.5);
                float flag = step(0, dot(LightDir, Left)) * 2 - 1;
                half4 shadowTex = tex2D(_ShadowTex, float2(flag * input.uv.x, input.uv.y));
                float shadow = shadowTex.r;

                // 使用 smoothstep 减少锯齿
                isShadow = 1 - smoothstep(ctrl - 0.02, ctrl + 0.02, shadow);

                fixed3 col = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor;

                // 去掉阴影颜色贴图
                fixed3 darkCol = tex2D(_MainTex, input.uv).rgb * _Darklight;

                return lerp(col, darkCol, isShadow);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
