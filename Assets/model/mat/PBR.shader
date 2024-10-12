Shader "Custom/PBR"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Roughness("Roughness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0
        _OutlineThickness("Outline Thickness", Range(0, 0.1)) = 0.01
    }
    SubShader
    {
        Tags { "LightMode" = "UniversalForward" }
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float _Roughness;
            float _Metallic;
            float _OutlineThickness;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 获取主纹理颜色
                fixed4 albedo = tex2D(_MainTex, i.uv);
                fixed4 normalColor = tex2D(_NormalMap, i.uv);
                
                // 计算法线
                float3 normal = normalize(i.normal + normalColor.rgb * 2 - 1);

                // PBR 光照计算
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float diff = max(0, dot(normal, lightDir));
                fixed3 lighting = diff * albedo.rgb;

                // 加入 NPR 的轮廓效果
                float outline = smoothstep(0.5, 1.0, diff) * _OutlineThickness;

                return fixed4(lighting + outline, albedo.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}