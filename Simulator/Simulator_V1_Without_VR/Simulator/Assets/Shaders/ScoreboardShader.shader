Shader "Custom/Scoreboard"
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

            int _NBZonesW = 3;
            int _NBZonesH = 3;
            float tailleZoneW = 1.0 / _NBZonesW;
            float tailleZoneH = 1.0 / _NBZonesH;

            bool zoneTrouve = false;
            float WGauche, WDroite, HBas, HHaut;
            int indZone = 0;

            float tabx[9]; //= [0.1, 0.5, 0.1, 0.2];
            float taby[9]; //= [0.1, 0.4, 0.3, 0.1];

            tabx[0] = 0.1;
            tabx[1] = 0.0;
            tabx[2] = 0.0;
            tabx[3] = 0.0;
            tabx[4] = 0.0;
            tabx[5] = 0.0;
            tabx[6] = 0.15;
            tabx[7] = 0.0;
            tabx[8] = 0.0;

            taby[0] = 0.1;
            taby[1] = 0.0;
            taby[2] = 0.0;
            taby[3] = 0.0;
            taby[4] = 0.0;
            taby[5] = 0.0;
            taby[6] = 0.0;
            taby[7] = 0.0;
            taby[8] = 0.0;

            /*float eps = 0.005;

            for (int w = 0; w <= _NBZonesW; w++) {
               for (int h = 0; h <= _NBZonesH; h++) {
                  indZone =  w + 2 * h;
                  if (abs((h + 0.5) * tailleZoneH  + taby[indZone] - IN.texcoord.y ) < eps &&
                           abs((w + 0.5) * tailleZoneW  + tabx[indZone] - IN.texcoord.x ) < eps)
                  {
                     return fixed4(1, 0, 1, 1);
                  } 
               }
            }*/


            for (int w = 0; w < _NBZonesW; w++) {
               for (int h = 0; h < _NBZonesW; h++) {
                  indZone = w + 2 * h;
                  /*if (h * tailleZoneH + taby[indZone] < IN.texcoord.y && IN.texcoord.y < (h + 1) * tailleZoneH + taby[indZone]
                     && w * tailleZoneW + tabx[indZone] < IN.texcoord.x  && IN.texcoord.x < (w + 1) * tailleZoneW + tabx[indZone]) {*/
                  if (h * tailleZoneH < IN.texcoord.y && IN.texcoord.y < (h + 1) * tailleZoneH
                     && w * tailleZoneW < IN.texcoord.x  && IN.texcoord.x < (w + 1) * tailleZoneW) {
                     WGauche = w * tailleZoneW;
                     WDroite = (w + 1) * tailleZoneW;
                     HBas = h * tailleZoneH;
                     HHaut = (h + 1) * tailleZoneH;
                     zoneTrouve = true;
                           
                     break;
                  }
               }
               if (zoneTrouve) {
                  break;
               }
            }
            
            fixed4 accumulateur = fixed4(0, 0, 0, 1);
            for (int localw = -100; localw < 100; localw++) {
               for (int localh = -100; localh < 100; localh++) {
                  if (!zoneTrouve) {
                     return fixed4(0, 0, 0, 1);
                  }
                  accumulateur.rgb = accumulateur.rgb + tex2D(_MainTex, float2(WGauche + tailleZoneW / 2 + localw * 0.001, HBas + tailleZoneH / 2 + localh * 0.001));
               }
            }
            accumulateur.rgb /= 4 * 100 * 100;
            float r = accumulateur.r;
            float g = accumulateur.g;
            float b = accumulateur.b;
            accumulateur.r = (0.3 * r) + (0.59 * g) + (0.11 * b);
            accumulateur.g = (0.3 * r) + (0.59 * g) + (0.11 * b);
            accumulateur.b = (0.3 * r) + (0.59 * g) + (0.11 * b);
            return accumulateur;
         }
      ENDCG
      }

      GrabPass { "_TexturePostCercleB" }

         Pass
      {
         Cull Front ZWrite On
         // BlendOp RevSub
         Blend Off
         CGPROGRAM
         #pragma enable_d3d11_debug_symbols
         #pragma vertex vert
         #pragma fragment frag

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
            float4 grabUV : TEXCOORD1;
         };

         fixed4 _Color;
         const int NBLEVELMAX = 5;

         v2f vert(appdata_t IN)
         {
            v2f OUT;
            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.texcoord = IN.texcoord;
            OUT.color = IN.color * _Color;
            #ifdef PIXELSNAP_ON
            OUT.vertex = UnityPixelSnap(OUT.vertex);
            #endif
            OUT.grabUV = ComputeGrabScreenPos(OUT.vertex);

            return OUT;
         }

         sampler2D _MainTex;
         sampler2D _TexturePostCercleB;
         float4 _MainTex_TexelSize;

         int nbGrayLevels(int numImplant) {
            if (numImplant == 0) return 7;
            if (numImplant == 1) return 4;
            if (numImplant == 2) return 3;
            if (numImplant == 3) return 8;
            if (numImplant == 4) return 2;
            if (numImplant == 5) return 6;
            if (numImplant == 6) return 6;
            if (numImplant == 7) return 7;
            if (numImplant == 8) return 1;
            return -1;
         }

         float GrayLevel(int numImplant, int pos) {
            if (numImplant == 0 && pos == 0) return 0.0f / 256;
            if (numImplant == 0 && pos == 1) return 25.0f / 256;
            if (numImplant == 0 && pos == 2) return 67.0f / 256;
            if (numImplant == 0 && pos == 3) return 116.0f / 256;
            if (numImplant == 0 && pos == 4) return 159.0f / 256;
            if (numImplant == 0 && pos == 5) return 197.0f / 256;
            if (numImplant == 0 && pos == 6) return 256.0f / 256;
            
            if (numImplant == 1 && pos == 0) return 0.0f / 256;
            if (numImplant == 1 && pos == 1) return 125.0f / 256;
            if (numImplant == 1 && pos == 2) return 208.0f / 256;
            if (numImplant == 1 && pos == 3) return 256.0f / 256;

            if (numImplant == 2 && pos == 0) return 0.0f / 256;
            if (numImplant == 2 && pos == 1) return 135.0f / 256;
            if (numImplant == 2 && pos == 2) return 256.0f / 256;
   
            

            if (numImplant == 1) return 4;
            if (numImplant == 2) return 3;
            if (numImplant == 3) return 8;
            if (numImplant == 4) return 2;
            if (numImplant == 5) return 6;
            if (numImplant == 6) return 6;
            if (numImplant == 7) return 7;
            if (numImplant == 8) return 1;
            return -1;
         }

         fixed4 frag(v2f IN) : SV_Target
         {

               int _NBZonesW = 3;
               int _NBZonesH = 3;
               float tailleZoneW = 1.0 / _NBZonesW;
               float tailleZoneH = 1.0 / _NBZonesH;
               float tabx[9]; //= [0.1, 0.5, 0.1, 0.2];
               float taby[9]; //= [0.1, 0.4, 0.3, 0.1];
               float rayons[9]; //= [0.1, 0.4, 0.3, 0.1];
               int defaillante[9];

               defaillante[0] = false;
               defaillante[1] = false;
               defaillante[2] = false;
               defaillante[3] = false;
               defaillante[4] = false;
               defaillante[5] = false;
               defaillante[6] = false;
               defaillante[7] = false;
               defaillante[8] = false;

               tabx[0] = 0.1;
               tabx[1] = 0.0;
               tabx[2] = 0.0;
               tabx[3] = 0.0;
               tabx[4] = 0.0;
               tabx[5] = 0.0;
               tabx[6] = 0.15;
               tabx[7] = 0.0;
               tabx[8] = 0.0;

               taby[0] = 0.1;
               taby[1] = 0.0;
               taby[2] = 0.0;
               taby[3] = 0.0;
               taby[4] = 0.0;
               taby[5] = 0.0;
               taby[6] = 0.0;
               taby[7] = 0.0;
               taby[8] = 0.0;

               rayons[0] = 0.21 * min(tailleZoneW, tailleZoneH);
               rayons[1] = 0.31 * min(tailleZoneW, tailleZoneH);
               rayons[2] = 0.18 * min(tailleZoneW, tailleZoneH);
               rayons[3] = 0.25 * min(tailleZoneW, tailleZoneH);
               rayons[4] = 0.25 * min(tailleZoneW, tailleZoneH);
               rayons[5] = 0.25 * min(tailleZoneW, tailleZoneH);
               rayons[6] = 0.25 * min(tailleZoneW, tailleZoneH);
               rayons[7] = 0.25 * min(tailleZoneW, tailleZoneH);
               rayons[8] = 0.25 * min(tailleZoneW, tailleZoneH);






               for (int w = 0; w < _NBZonesW; w++) {
                  for (int h = 0; h < _NBZonesH; h++) {

                     float WGauche = w * tailleZoneW;
                     float WDroite = (w + 1) * tailleZoneW;
                     float HBas = h * tailleZoneH;
                     float HHaut = (h + 1) * tailleZoneH;
                     int indZone = w + 2 * h;

                     float CentreCercleW = ((WDroite - WGauche) / 2.0 + w * tailleZoneW) + tabx[indZone];
                     float CentreCercleH = ((HHaut - HBas) / 2.0 + h * tailleZoneH) + taby[indZone];

                     // dans le cercle ou pas ?
                     if (sqrt((CentreCercleW - IN.texcoord.x)*(CentreCercleW -  IN.texcoord.x) + (CentreCercleH - IN.texcoord.y)*(CentreCercleH - IN.texcoord.y)) <= rayons[indZone]) {
                        if (!defaillante[indZone]) {
                           fixed4 col = tex2Dproj(_TexturePostCercleB, IN.grabUV);
                           int nbLevels = nbGrayLevels(indZone);
                           for (int i = 0; i < 7; ++i) {
                              if (i > nbLevels) {  }
                              else if ((GrayLevel(indZone, i) < col.r) && (GrayLevel(indZone, i + 1) > col.r)) {
                                 if (abs(GrayLevel(indZone, i) - col.r) > abs(GrayLevel(indZone, i + 1) - col.r)) {
                                    col.r = GrayLevel(indZone, i + 1);
                                    col.g = GrayLevel(indZone, i + 1);
                                    col.b = GrayLevel(indZone, i + 1);
                                 }
                                 else {
                                    col.r = GrayLevel(indZone, i);
                                    col.g = GrayLevel(indZone, i);
                                    col.b = GrayLevel(indZone, i);
                                 }
                              }
                           }                           
                           return col;
                        }
                     }
                  }
               }
               return fixed4(0, 0, 0, 1); 
         }
         ENDCG
      }
   }
}
