Shader "CelShader/CelShaderWithTex"
{
    Properties
    {
        _Darklight("Dark Light",Color) = (0.1,0.1,0.1,1)
        _MainTex("Main Tex",2D) = "white"{}
    }
    SubShader
    {
         
 
 
 
        Pass
        {
            Tags{"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
 
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
 
            float3 _Darklight;
            sampler2D _MainTex;
            struct c2v
            {
                float4 vertex:POSITION;
                float3 normal:NORMAL;
                float2 uv:TEXCOORD0;
            };
 
            struct v2f
            {
                float4 pos:SV_POSITION;
                float3 worldNormal:NORMAL;
                float2 uv:TEXCOORD;
                float3 objViewDir:COLOR1;
                float3 normal:NORMAL2;
            };
 
            v2f vert(c2v input)
            {
                v2f output;
                output.pos = UnityObjectToClipPos(input.vertex);
                output.worldNormal = normalize( mul((float3x3)unity_ObjectToWorld,input.normal) );
                output.uv = input.uv;
 
 
                float3 ObjViewDir = normalize(ObjSpaceViewDir(input.vertex));
                output.objViewDir = ObjViewDir;
                output.normal = normalize(input.normal);
 
                return output;
            }
 
 
            fixed3 frag(in v2f input):SV_TARGET0
            {
                fixed3 diffuseColor = _LightColor0.rgb *  tex2D(_MainTex, input.uv).rgb ;
                fixed3 col = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor;
                
                
                
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
 
 
                if(dot(input.worldNormal,worldLightDir)<0)
                {
                    return col * _Darklight;
                }
                return col;
            }
 
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}