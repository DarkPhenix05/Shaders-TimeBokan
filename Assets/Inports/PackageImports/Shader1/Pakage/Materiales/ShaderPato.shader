Shader "Custom/ShaderPata"
{
    Properties
    {
        // _Color ("Color", Color) = (1,1,1,1)

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RampTex("Ramp", 2D) = "white" {}

        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200 //level of detail 

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Toon

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
         sampler2D _RampTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        // half _Glossiness;
        // half _Metallic;
        // fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color

             o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;


            // o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            // o.Metallic = _Metallic;
            // o.Smoothness = _Glossiness;
            // o.Alpha = c.a;
        }



         fixed4 LightingToon(SurfaceOutput s /* shader de pixel */ , fixed3 lightDir /* direccion de luz de unity */, fixed atten)
        {
            // First calculate the dot product of the light direction and
            // the surface's normal
            half NdotL = dot(s.Normal, lightDir);

            // Remap NdotL to the value on the ramp map
            NdotL = tex2D(_RampTex, fixed2(NdotL, 0.5));

            // Next, set what color should be returned
            half4 color;

            color.rgb = s.Albedo * /* multiplicación de color */ _LightColor0.rgb * (NdotL * atten);
            color.a = s.Alpha;

            // Return the calculated color
            return color;
        }


        ENDCG
    }
    FallBack "Diffuse"
}
