Shader "Custom/Julien_3_3_0PercentNotWorkingScoreboard"
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

      Pass
      {
         CGPROGRAM
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

         fixed4 frag(v2f IN) : SV_Target
         {
            fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

            int NumberOfElectrodesW = 3;
            int NumberOfElectrodesH = 3;
            float tailleZoneW = 1.0 / NumberOfElectrodesW;
            float tailleZoneH = 1.0 / NumberOfElectrodesH;
            bool zoneTrouve = false;
            float WGauche, WDroite, HBas, HHaut;
            int indZone = 0;

            float DELTAX[9];
            float DELTAY[9];
            DELTAX[0] = 0.0237714428;
            DELTAY[0] = 0.0317906407;
            DELTAX[1] = -0.0383440083;
            DELTAY[1] = -0.0402118847;
            DELTAX[2] = 0.0194195045;
            DELTAY[2] = -0.0707307808;
            DELTAX[3] = 0.0698102255;
            DELTAY[3] = -0.0369167421;
            DELTAX[4] = -0.0262206077;
            DELTAY[4] = -0.0625627862;
            DELTAX[5] = -0.0467706203;
            DELTAY[5] = 0.0821325819;
            DELTAX[6] = 0.0359875322;
            DELTAY[6] = -0.0108933361;
            DELTAX[7] = -0.0358368998;
            DELTAY[7] = -0.0202918218;
            DELTAX[8] = -0.0180353234;
            DELTAY[8] = 0.0366405928;

            for (int w = 0; w < NumberOfElectrodesW; w++) {
               for (int h = 0; h < NumberOfElectrodesH; h++) {
                  indZone =  w + NumberOfElectrodesW * h;
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
                  if (zoneTrouve) {
                     break;
                  }
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
            float gray = (0.3 * r) + (0.59 * g) + (0.11 * b);
            accumulateur.r = gray;
            accumulateur.g = gray;
            accumulateur.b = gray;
            return accumulateur;
         }

      ENDCG
      }
      GrabPass
      {
         "_TexturePostCercle_3_3_0PercentNotWorkingScoreboard"
      }

      Pass
      {
         Cull Front ZWrite On
         Blend Off
         CGPROGRAM
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
         sampler2D _TexturePostCercle_3_3_0PercentNotWorkingScoreboard;
         float4 _MainTex_TexelSize;

         int nbGrayLevels(int numImplant) {
            if (numImplant == 0) return 6;
            if (numImplant == 1) return 6;
            if (numImplant == 2) return 5;
            if (numImplant == 3) return 4;
            if (numImplant == 4) return 4;
            if (numImplant == 5) return 4;
            if (numImplant == 6) return 5;
            if (numImplant == 7) return 7;
            if (numImplant == 8) return 4;
            return -1;
         }
         float GrayLevel(int numImplant, int pos) {
            if (numImplant == 0 && pos == 0) return 0.076;
            if (numImplant == 0 && pos == 1) return 0.081;
            if (numImplant == 0 && pos == 2) return 0.198;
            if (numImplant == 0 && pos == 3) return 0.207;
            if (numImplant == 0 && pos == 4) return 0.310;
            if (numImplant == 0 && pos == 5) return 0.403;
            if (numImplant == 1 && pos == 0) return 0.103;
            if (numImplant == 1 && pos == 1) return 0.361;
            if (numImplant == 1 && pos == 2) return 0.470;
            if (numImplant == 1 && pos == 3) return 0.746;
            if (numImplant == 1 && pos == 4) return 0.822;
            if (numImplant == 1 && pos == 5) return 0.920;
            if (numImplant == 2 && pos == 0) return 0.062;
            if (numImplant == 2 && pos == 1) return 0.138;
            if (numImplant == 2 && pos == 2) return 0.175;
            if (numImplant == 2 && pos == 3) return 0.364;
            if (numImplant == 2 && pos == 4) return 0.709;
            if (numImplant == 3 && pos == 0) return 0.279;
            if (numImplant == 3 && pos == 1) return 0.325;
            if (numImplant == 3 && pos == 2) return 0.349;
            if (numImplant == 3 && pos == 3) return 0.532;
            if (numImplant == 4 && pos == 0) return 0.062;
            if (numImplant == 4 && pos == 1) return 0.148;
            if (numImplant == 4 && pos == 2) return 0.469;
            if (numImplant == 4 && pos == 3) return 0.694;
            if (numImplant == 5 && pos == 0) return 0.254;
            if (numImplant == 5 && pos == 1) return 0.431;
            if (numImplant == 5 && pos == 2) return 0.633;
            if (numImplant == 5 && pos == 3) return 0.805;
            if (numImplant == 6 && pos == 0) return 0.277;
            if (numImplant == 6 && pos == 1) return 0.280;
            if (numImplant == 6 && pos == 2) return 0.426;
            if (numImplant == 6 && pos == 3) return 0.628;
            if (numImplant == 6 && pos == 4) return 0.746;
            if (numImplant == 7 && pos == 0) return 0.023;
            if (numImplant == 7 && pos == 1) return 0.545;
            if (numImplant == 7 && pos == 2) return 0.658;
            if (numImplant == 7 && pos == 3) return 0.716;
            if (numImplant == 7 && pos == 4) return 0.780;
            if (numImplant == 7 && pos == 5) return 0.833;
            if (numImplant == 7 && pos == 6) return 0.900;
            if (numImplant == 8 && pos == 0) return 0.372;
            if (numImplant == 8 && pos == 1) return 0.468;
            if (numImplant == 8 && pos == 2) return 0.620;
            if (numImplant == 8 && pos == 3) return 0.932;
            return -1;
         }

         fixed4 frag(v2f IN) : SV_Target
         {
            int NumberOfElectrodesW = 3;
            int NumberOfElectrodesH = 3;
            float tailleZoneW = 1.0 / NumberOfElectrodesW;
            float tailleZoneH = 1.0 / NumberOfElectrodesH;
            float WGauche, WDroite, HBas, HHaut;
            float DELTAX[9];
            float DELTAY[9];
            float RAYONS[9];
            float BROKEN[9];
            DELTAX[0] = 0.0237714428;
            DELTAY[0] = 0.0317906407;
            RAYONS[0] = 0.0555131661;
            BROKEN[0] = false;
            DELTAX[1] = -0.0383440083;
            DELTAY[1] = -0.0402118847;
            RAYONS[1] = 0.0823389623;
            BROKEN[1] = false;
            DELTAX[2] = 0.0194195045;
            DELTAY[2] = -0.0707307808;
            RAYONS[2] = 0.0811387909;
            BROKEN[2] = false;
            DELTAX[3] = 0.0698102255;
            DELTAY[3] = -0.0369167421;
            RAYONS[3] = 0.0497137013;
            BROKEN[3] = true;
            DELTAX[4] = -0.0262206077;
            DELTAY[4] = -0.0625627862;
            RAYONS[4] = 0.0767414086;
            BROKEN[4] = false;
            DELTAX[5] = -0.0467706203;
            DELTAY[5] = 0.0821325819;
            RAYONS[5] = 0.0600517793;
            BROKEN[5] = false;
            DELTAX[6] = 0.0359875322;
            DELTAY[6] = -0.0108933361;
            RAYONS[6] = 0.0431209637;
            BROKEN[6] = false;
            DELTAX[7] = -0.0358368998;
            DELTAY[7] = -0.0202918218;
            RAYONS[7] = 0.0505536829;
            BROKEN[7] = false;
            DELTAX[8] = -0.0180353234;
            DELTAY[8] = 0.0366405928;
            RAYONS[8] = 0.0445749726;
            BROKEN[8] = false;

            for (int w = 0; w < NumberOfElectrodesW; w++) {
               for (int h = 0; h < NumberOfElectrodesH; h++) {
                  WGauche = w * tailleZoneW;
                  WDroite = (w + 1) * tailleZoneW;
                  HBas = h * tailleZoneH;
                  HHaut = (h + 1) * tailleZoneH;
                  int indZone = w + NumberOfElectrodesW * h;
                  float CentreCercleW = ((WDroite - WGauche) / 2.0 + w * tailleZoneW) + DELTAX[indZone];
                  float CentreCercleH = ((HHaut - HBas) / 2.0 + h * tailleZoneH) + DELTAY[indZone];

                  if (sqrt((CentreCercleW - IN.texcoord.x)*(CentreCercleW -  IN.texcoord.x) + (CentreCercleH - IN.texcoord.y)*(CentreCercleH - IN.texcoord.y)) <= RAYONS[indZone]) {
                     if (!BROKEN[indZone]) {
                        fixed4 col = tex2Dproj(_TexturePostCercle_3_3_0PercentNotWorkingScoreboard, IN.grabUV);
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
