Shader "Custom/EsfoShader"
{

    Properties
    {
        //posicion
        _RampTex("Color Ramp", 2D) = "white" {}
        _RampOffset("Ramp offset", Range(-0.5,0.5)) = 0

        //que color vamos a ponerle a nuestro pixel 
        _NoiseTex("Noise Texture", 2D) = "gray" {}
        _Period("Period", Range(0,1)) = 0.5

        // determinado en un nivel o restricción
        _Amount("_Amount", Range(0, 1.0)) = 0.1
        _ClipRange("ClipRange", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert vertex:vert nolightmap 
 
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        //convirtiendo variables
        sampler2D _RampTex; 

        half _RampOffset; 

        sampler2D _NoiseTex; 

        float _Period; 

        float _Amount; 

        float _ClipRange;



        struct Input
        {
            float2 uv_NoiseTex;
        };

        //inout funcion que quiero que salga igual 

       void vert (inout appdata_full v)
       {
           // lod = nivel de detalle
           // cuanto valor de noise le asigna a cada vertice
           float disp = tex2Dlod(_NoiseTex, float4(v.texcoord.xy,0,0)).r;
           float time = sin(_Time[3] * _Period + disp * 10);

           //_Amount es variación de color, variación de tiempo con movimiento, 
           v.vertex.xyz += v.normal * disp * _Amount * time; 


       }

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color

            float3 noise = tex2D (_NoiseTex, IN.uv_NoiseTex);
            float n = saturate(noise.r + _RampOffset);
        

            // el color de 0 a 1 busca en una tabla que asigna más o menos oscuro 

            half4 c = tex2D(_RampTex, float2(n, 0.5)); 
            clip(_ClipRange - n);

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Emission = c.rgb; 
        }
        ENDCG
    }
    FallBack "Diffuse"
}
