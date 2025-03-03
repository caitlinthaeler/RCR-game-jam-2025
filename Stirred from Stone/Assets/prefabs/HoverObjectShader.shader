Shader "Custom/EmissiveOverlayWithFog"
{
    Properties
    {
        _Color ("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Range(0,10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            Blend One One // Additive blending for glow effect
            ZWrite Off     // Prevents depth conflicts
            Cull Back      // Renders only the outer surface

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog // Enables Unity's fog system
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0) // Fog coordinates
            };

            float4 _Color;
            float _EmissionStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o, o.pos); // Pass fog info to the fragment shader
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 emission = _Color * _EmissionStrength;
                UNITY_APPLY_FOG(i.fogCoord, emission); // Apply fog to the overlay
                return emission;
            }
            ENDCG
        }
    }
}
