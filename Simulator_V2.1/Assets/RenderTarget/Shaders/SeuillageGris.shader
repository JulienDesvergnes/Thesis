Shader "Custom/SeuillageGris"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Param de shader
            sampler2D _MainTex;
            
            //////////////////////////////////////////////////////////////////////////////////////////////
            // Fonction : vert VERTEX SHADER                                                            //
            //////////////////////////////////////////////////////////////////////////////////////////////
            v2f vert (appdata v)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.uv = v.uv;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
                return OUT;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////
            // Fonction : frag FRAGMENT SHADER                                                          //
            //////////////////////////////////////////////////////////////////////////////////////////////
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
