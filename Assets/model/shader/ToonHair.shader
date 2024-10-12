Shader "Custom/ToonHair"
{
    Properties
    {
        _Darklight("Dark Light", Color) = (0.1, 0.1, 0.1, 1)
        _MainTex("Main Tex", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {} // 添加法线贴图
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
            sampler2D _NormalMap; // 法线贴图采样器

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
                float3 tangentSpaceNormal : TEXCOORD1; // 添加用于法线贴图的空间法线
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

                // Tangent space normal for normal map
                output.tangentSpaceNormal = input.normal;

                return output;
            }

            fixed3 frag(v2f input) : SV_TARGET0
            {
                // 从法线贴图中提取法线
                fixed3 tangentNormal = UnpackNormal(tex2D(_NormalMap, input.uv));

                // 计算世界空间的法线，应用到光照
                fixed3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, tangentNormal));

                // 计算漫反射颜色
                fixed3 diffuseColor = _LightColor0.rgb * tex2D(_MainTex, input.uv).rgb;
                fixed3 col = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor;

                // 世界光照方向
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                
                // 计算阴影
                float isShadow = step(dot(worldNormal, worldLightDir), 0);
                
                // 计算暗色调
                fixed3 darkCol = tex2D(_MainTex, input.uv).rgb * _Darklight;

                // 插值法应用阴影
                return lerp(col, darkCol, isShadow);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}