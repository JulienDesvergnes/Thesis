Shader "Custom/RenduClassique"
{
   Properties
   {
       [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
       _Color("Tint", Color) = (1,1,1,1)
       [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
   }

   SubShader
   {
      Tags
      {
         "Queue" = "Transparent"
         "IgnoreProjector" = "True"
         "RenderType" = "Opaque"
         "PreviewType" = "Plane"
         "CanUseSpriteAtlas" = "True"
      }

      Cull Off
      Lighting Off
      ZWrite Off
      // Blend One OneMinusSrcAlpha

      Pass
      {
         CGPROGRAM
         #pragma enable_d3d11_debug_symbols
         #pragma vertex vert
         #pragma fragment frag
         #pragma multi_compile _ PIXELSNAP_ON
         #include "UnityCG.cginc"

         struct appdata_t
         {
               float4 vertex   : POSITION;
               float4 color    : COLOR;
               float2 texcoord : TEXCOORD0;
         };

         struct v2f
         {
               float4 vertex   : SV_POSITION;
               fixed4 color : COLOR;
               half2 texcoord  : TEXCOORD0;
         };

         fixed4 _Color;

         v2f vert(appdata_t IN)
         {
               v2f OUT;
               OUT.vertex = UnityObjectToClipPos(IN.vertex);
               OUT.texcoord = IN.texcoord;
               OUT.color = IN.color * _Color;
               #ifdef PIXELSNAP_ON
               OUT.vertex = UnityPixelSnap(OUT.vertex);
               #endif
               return OUT;
         }

         sampler2D _MainTex;
         float4 _MainTex_TexelSize;

         fixed4 frag(v2f IN) : SV_Target{

            fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
            return c;
         }
         ENDCG
      }
   }
}
