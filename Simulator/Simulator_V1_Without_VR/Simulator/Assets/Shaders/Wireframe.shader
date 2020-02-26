Shader "Custom/Wireframe"
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

             float sobel(sampler2D tex, float2 uv) {
                float prof = 0.02;
                float2 delta = float2(prof, prof);

                float4 hr = float4(0, 0, 0, 0);
                float4 vt = float4(0, 0, 0, 0);

                hr += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
                hr += tex2D(tex, (uv + float2(1.0, -1.0) * delta)) * -1.0;
                hr += tex2D(tex, (uv + float2(-1.0, 0.0) * delta)) *  2.0;
                hr += tex2D(tex, (uv + float2(1.0, 0.0) * delta)) * -2.0;
                hr += tex2D(tex, (uv + float2(-1.0, 1.0) * delta)) *  1.0;
                hr += tex2D(tex, (uv + float2(1.0, 1.0) * delta)) * -1.0;

                vt += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
                vt += tex2D(tex, (uv + float2(0.0, -1.0) * delta)) *  2.0;
                vt += tex2D(tex, (uv + float2(1.0, -1.0) * delta)) *  1.0;
                vt += tex2D(tex, (uv + float2(-1.0, 1.0) * delta)) * -1.0;
                vt += tex2D(tex, (uv + float2(0.0, 1.0) * delta)) * -2.0;
                vt += tex2D(tex, (uv + float2(1.0, 1.0) * delta)) * -1.0;

                return sqrt(hr * hr + vt * vt);
             }

             fixed4 frag(v2f IN) : SV_Target {
                float s = sobel(_MainTex, IN.texcoord);

                int _NBZonesW = 18;
                int _NBZonesH = 15;
                float tailleZoneW = 1.0 / _NBZonesW;
                float tailleZoneH = 1.0 / _NBZonesH;

                bool zoneTrouve = false;
                float WGauche, WDroite, HBas, HHaut;
                int indZone = 0;

                for (int w = 0; w < _NBZonesW; w++) {
                   for (int h = 0; h < _NBZonesW; h++) {
                      indZone = w + 2 * h;
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
                for (int localw = -10; localw < 10; localw++) {
                   for (int localh = -10; localh < 10; localh++) {
                      if (!zoneTrouve) {
                         return fixed4(0, 0, 0, 1);
                      }
                      accumulateur.rgb = accumulateur.rgb + sobel(_MainTex, float2(WGauche + tailleZoneW / 2 + localw * 0.001, HBas + tailleZoneH / 2 + localh * 0.001));
                   }
                }
                accumulateur.rgb /= 4 * 10 * 10;
                float r = accumulateur.r;
                float g = accumulateur.g;
                float b = accumulateur.b;
                accumulateur.r = (0.3 * r) + (0.59 * g) + (0.11 * b);
                accumulateur.g = (0.3 * r) + (0.59 * g) + (0.11 * b);
                accumulateur.b = (0.3 * r) + (0.59 * g) + (0.11 * b);

                if (accumulateur.r > 0.1) { 
                   accumulateur.r = 1; 
                   accumulateur.g = 1;
                   accumulateur.b = 1;
                } else { 
                   accumulateur.r = 0;
                   accumulateur.g = 0;
                   accumulateur.b = 0;
                }
                return accumulateur;//float4(s, s, s, 1);              
             }
          ENDCG
       }

       GrabPass { "_TexturePostCercleW" }

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
           sampler2D _TexturePostCercleW;
           float4 _MainTex_TexelSize;

           fixed4 frag(v2f IN) : SV_Target
           {
              fixed4 col = tex2Dproj(_TexturePostCercleW, IN.grabUV);

              int _NBZonesW = 18;
              int _NBZonesH = 15;
              float tailleZoneW = 1.0 / _NBZonesW;
              float tailleZoneH = 1.0 / _NBZonesH;

              for (int w = 0; w < _NBZonesW; w++) {
                 for (int h = 0; h < _NBZonesH; h++) {

                    float WGauche = w * tailleZoneW;
                    float WDroite = (w + 1) * tailleZoneW;
                    float HBas = h * tailleZoneH;
                    float HHaut = (h + 1) * tailleZoneH;
                    int indZone = w + 2 * h;

                    float CentreCercleW = ((WDroite - WGauche) / 2.0 + w * tailleZoneW); //+ tabx[indZone];
                    float CentreCercleH = ((HHaut - HBas) / 2.0 + h * tailleZoneH); //+ taby[indZone];

                    // dans le cercle ou pas ?
                    if (sqrt((CentreCercleW - IN.texcoord.x)*(CentreCercleW - IN.texcoord.x) + (CentreCercleH - IN.texcoord.y)*(CentreCercleH - IN.texcoord.y)) <= 0.01/*rayons[indZone]*/) {
                       if (/*!defaillante[indZone]*/true) {
                          fixed4 col = tex2Dproj(_TexturePostCercleW, IN.grabUV);
                          /*int nbLevels = nbGrayLevels(indZone);
                          for (int i = 0; i < 7; ++i) {
                             if (i > nbLevels) {}
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
                          }*/
                          return col;
                       }
                    }
                 }
              }
              return fixed4(0, 0, 0, 1);

              return col;
           }
           ENDCG
       }
    }
}
