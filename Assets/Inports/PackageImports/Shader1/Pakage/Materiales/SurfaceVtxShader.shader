Shader "Custom/SurfaceVtxShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

      _Amount("Extrusion Amount", Range(-1, 1)) = 0
      _Speed ("Wave Speed", Range(0.1, 80)) = 5
      _Absolute("Absolte", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM        
        #pragma surface surf Standard vertex:vert        
        #pragma target 3.0

        sampler2D _MainTex;

        float _Speed;

        int _Absolute;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float _Amount;

        void vert(inout appdata_full v)
        {
          float time = _Time * _Speed;
          float waveValueA;

          if(_Absolute == 0)
          {
            waveValueA = sin(time * 2.0) * 0.1;
          }
          else
          {
            waveValueA = abs(sin(time * 2.0) * 0.1);
          }

          v.vertex.xyz += v.normal * waveValueA * _Amount;
        }
      
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {            
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
