Shader "Custom/SimulateurV2_5Shader"
{
   Properties
   {
       _MainTex("Texture", 2D) = "white" {}
       _NumberOfElectrodesW("NBEW",Float) = 18
       _NumberOfElectrodesH("NBEH",Float) = 15
       _tailleZoneW("tailleZoneW",Float) = 0.05555555555 // = 1/18 en flotant
       _tailleZoneH("tailleZoneH",Float) = 0.06666666666 // = 1/15 en flotant
       _sizeWindowSobel("SizeWindowSobel", Float) = 0.003
       _rayon("rayon", Float) = 0.02
       _k0_0("k0_0", Float) = 0
       _k0_1("k0_1", Float) = 0
       _k0_2("k0_2", Float) = 0
       _k0_3("k0_3", Float) = 0
       _k0_4("k0_4", Float) = 0
       _k0_5("k0_5", Float) = 0
       _k0_6("k0_6", Float) = 0
       _k0_7("k0_7", Float) = 0
       _k0_8("k0_8", Float) = 0
       _k0_9("k0_9", Float) = 0
       _k0_10("k0_10", Float) = 0
       _k0_11("k0_11", Float) = 0
       _k0_12("k0_12", Float) = 0
       _k0_13("k0_13", Float) = 0
       _k0_14("k0_14", Float) = 0
       _k0_15("k0_15", Float) = 0
       _k0_16("k0_16", Float) = 0
       _k0_17("k0_17", Float) = 0
       _k1_0("k1_0", Float) = 0
       _k1_1("k1_1", Float) = 0
       _k1_2("k1_2", Float) = 0
       _k1_3("k1_3", Float) = 0
       _k1_4("k1_4", Float) = 0
       _k1_5("k1_5", Float) = 0
       _k1_6("k1_6", Float) = 0
       _k1_7("k1_7", Float) = 0
       _k1_8("k1_8", Float) = 0
       _k1_9("k1_9", Float) = 0
       _k1_10("k1_10", Float) = 0
       _k1_11("k1_11", Float) = 0
       _k1_12("k1_12", Float) = 0
       _k1_13("k1_13", Float) = 0
       _k1_14("k1_14", Float) = 0
       _k1_15("k1_15", Float) = 0
       _k1_16("k1_16", Float) = 0
       _k1_17("k1_17", Float) = 0
       _k2_0("k2_0", Float) = 0
       _k2_1("k2_1", Float) = 0
       _k2_2("k2_2", Float) = 0
       _k2_3("k2_3", Float) = 0
       _k2_4("k2_4", Float) = 0
       _k2_5("k2_5", Float) = 0
       _k2_6("k2_6", Float) = 0
       _k2_7("k2_7", Float) = 0
       _k2_8("k2_8", Float) = 0
       _k2_9("k2_9", Float) = 0
       _k2_10("k2_10", Float) = 0
       _k2_11("k2_11", Float) = 0
       _k2_12("k2_12", Float) = 0
       _k2_13("k2_13", Float) = 0
       _k2_14("k2_14", Float) = 0
       _k2_15("k2_15", Float) = 0
       _k2_16("k2_16", Float) = 0
       _k2_17("k2_17", Float) = 0
       _k3_0("k3_0", Float) = 0
       _k3_1("k3_1", Float) = 0
       _k3_2("k3_2", Float) = 0
       _k3_3("k3_3", Float) = 0
       _k3_4("k3_4", Float) = 0
       _k3_5("k3_5", Float) = 0
       _k3_6("k3_6", Float) = 0
       _k3_7("k3_7", Float) = 0
       _k3_8("k3_8", Float) = 0
       _k3_9("k3_9", Float) = 0
       _k3_10("k3_10", Float) = 0
       _k3_11("k3_11", Float) = 0
       _k3_12("k3_12", Float) = 0
       _k3_13("k3_13", Float) = 0
       _k3_14("k3_14", Float) = 0
       _k3_15("k3_15", Float) = 0
       _k3_16("k3_16", Float) = 0
       _k3_17("k3_17", Float) = 0
       _k4_0("k4_0", Float) = 0
       _k4_1("k4_1", Float) = 0
       _k4_2("k4_2", Float) = 0
       _k4_3("k4_3", Float) = 0
       _k4_4("k4_4", Float) = 0
       _k4_5("k4_5", Float) = 0
       _k4_6("k4_6", Float) = 0
       _k4_7("k4_7", Float) = 0
       _k4_8("k4_8", Float) = 0
       _k4_9("k4_9", Float) = 0
       _k4_10("k4_10", Float) = 0
       _k4_11("k4_11", Float) = 0
       _k4_12("k4_12", Float) = 0
       _k4_13("k4_13", Float) = 0
       _k4_14("k4_14", Float) = 0
       _k4_15("k4_15", Float) = 0
       _k4_16("k4_16", Float) = 0
       _k4_17("k4_17", Float) = 0
       _k5_0("k5_0", Float) = 0
       _k5_1("k5_1", Float) = 0
       _k5_2("k5_2", Float) = 0
       _k5_3("k5_3", Float) = 0
       _k5_4("k5_4", Float) = 0
       _k5_5("k5_5", Float) = 0
       _k5_6("k5_6", Float) = 0
       _k5_7("k5_7", Float) = 0
       _k5_8("k5_8", Float) = 0
       _k5_9("k5_9", Float) = 0
       _k5_10("k5_10", Float) = 0
       _k5_11("k5_11", Float) = 0
       _k5_12("k5_12", Float) = 0
       _k5_13("k5_13", Float) = 0
       _k5_14("k5_14", Float) = 0
       _k5_15("k5_15", Float) = 0
       _k5_16("k5_16", Float) = 0
       _k5_17("k5_17", Float) = 0
       _k6_0("k6_0", Float) = 0
       _k6_1("k6_1", Float) = 0
       _k6_2("k6_2", Float) = 0
       _k6_3("k6_3", Float) = 0
       _k6_4("k6_4", Float) = 0
       _k6_5("k6_5", Float) = 0
       _k6_6("k6_6", Float) = 0
       _k6_7("k6_7", Float) = 0
       _k6_8("k6_8", Float) = 0
       _k6_9("k6_9", Float) = 0
       _k6_10("k6_10", Float) = 0
       _k6_11("k6_11", Float) = 0
       _k6_12("k6_12", Float) = 0
       _k6_13("k6_13", Float) = 0
       _k6_14("k6_14", Float) = 0
       _k6_15("k6_15", Float) = 0
       _k6_16("k6_16", Float) = 0
       _k6_17("k6_17", Float) = 0
       _k7_0("k7_0", Float) = 0
       _k7_1("k7_1", Float) = 0
       _k7_2("k7_2", Float) = 0
       _k7_3("k7_3", Float) = 0
       _k7_4("k7_4", Float) = 0
       _k7_5("k7_5", Float) = 0
       _k7_6("k7_6", Float) = 0
       _k7_7("k7_7", Float) = 0
       _k7_8("k7_8", Float) = 0
       _k7_9("k7_9", Float) = 0
       _k7_10("k7_10", Float) = 0
       _k7_11("k7_11", Float) = 0
       _k7_12("k7_12", Float) = 0
       _k7_13("k7_13", Float) = 0
       _k7_14("k7_14", Float) = 0
       _k7_15("k7_15", Float) = 0
       _k7_16("k7_16", Float) = 0
       _k7_17("k7_17", Float) = 0
       _k8_0("k8_0", Float) = 0
       _k8_1("k8_1", Float) = 0
       _k8_2("k8_2", Float) = 0
       _k8_3("k8_3", Float) = 0
       _k8_4("k8_4", Float) = 0
       _k8_5("k8_5", Float) = 0
       _k8_6("k8_6", Float) = 0
       _k8_7("k8_7", Float) = 0
       _k8_8("k8_8", Float) = 0
       _k8_9("k8_9", Float) = 0
       _k8_10("k8_10", Float) = 0
       _k8_11("k8_11", Float) = 0
       _k8_12("k8_12", Float) = 0
       _k8_13("k8_13", Float) = 0
       _k8_14("k8_14", Float) = 0
       _k8_15("k8_15", Float) = 0
       _k8_16("k8_16", Float) = 0
       _k8_17("k8_17", Float) = 0
       _k9_0("k9_0", Float) = 0
       _k9_1("k9_1", Float) = 0
       _k9_2("k9_2", Float) = 0
       _k9_3("k9_3", Float) = 0
       _k9_4("k9_4", Float) = 0
       _k9_5("k9_5", Float) = 0
       _k9_6("k9_6", Float) = 0
       _k9_7("k9_7", Float) = 0
       _k9_8("k9_8", Float) = 0
       _k9_9("k9_9", Float) = 0
       _k9_10("k9_10", Float) = 0
       _k9_11("k9_11", Float) = 0
       _k9_12("k9_12", Float) = 0
       _k9_13("k9_13", Float) = 0
       _k9_14("k9_14", Float) = 0
       _k9_15("k9_15", Float) = 0
       _k9_16("k9_16", Float) = 0
       _k9_17("k9_17", Float) = 0
       _k10_0("k10_0", Float) = 0
       _k10_1("k10_1", Float) = 0
       _k10_2("k10_2", Float) = 0
       _k10_3("k10_3", Float) = 0
       _k10_4("k10_4", Float) = 0
       _k10_5("k10_5", Float) = 0
       _k10_6("k10_6", Float) = 0
       _k10_7("k10_7", Float) = 0
       _k10_8("k10_8", Float) = 0
       _k10_9("k10_9", Float) = 0
       _k10_10("k10_10", Float) = 0
       _k10_11("k10_11", Float) = 0
       _k10_12("k10_12", Float) = 0
       _k10_13("k10_13", Float) = 0
       _k10_14("k10_14", Float) = 0
       _k10_15("k10_15", Float) = 0
       _k10_16("k10_16", Float) = 0
       _k10_17("k10_17", Float) = 0
       _k11_0("k11_0", Float) = 0
       _k11_1("k11_1", Float) = 0
       _k11_2("k11_2", Float) = 0
       _k11_3("k11_3", Float) = 0
       _k11_4("k11_4", Float) = 0
       _k11_5("k11_5", Float) = 0
       _k11_6("k11_6", Float) = 0
       _k11_7("k11_7", Float) = 0
       _k11_8("k11_8", Float) = 0
       _k11_9("k11_9", Float) = 0
       _k11_10("k11_10", Float) = 0
       _k11_11("k11_11", Float) = 0
       _k11_12("k11_12", Float) = 0
       _k11_13("k11_13", Float) = 0
       _k11_14("k11_14", Float) = 0
       _k11_15("k11_15", Float) = 0
       _k11_16("k11_16", Float) = 0
       _k11_17("k11_17", Float) = 0
       _k12_0("k12_0", Float) = 0
       _k12_1("k12_1", Float) = 0
       _k12_2("k12_2", Float) = 0
       _k12_3("k12_3", Float) = 0
       _k12_4("k12_4", Float) = 0
       _k12_5("k12_5", Float) = 0
       _k12_6("k12_6", Float) = 0
       _k12_7("k12_7", Float) = 0
       _k12_8("k12_8", Float) = 0
       _k12_9("k12_9", Float) = 0
       _k12_10("k12_10", Float) = 0
       _k12_11("k12_11", Float) = 0
       _k12_12("k12_12", Float) = 0
       _k12_13("k12_13", Float) = 0
       _k12_14("k12_14", Float) = 0
       _k12_15("k12_15", Float) = 0
       _k12_16("k12_16", Float) = 0
       _k12_17("k12_17", Float) = 0
       _k13_0("k13_0", Float) = 0
       _k13_1("k13_1", Float) = 0
       _k13_2("k13_2", Float) = 0
       _k13_3("k13_3", Float) = 0
       _k13_4("k13_4", Float) = 0
       _k13_5("k13_5", Float) = 0
       _k13_6("k13_6", Float) = 0
       _k13_7("k13_7", Float) = 0
       _k13_8("k13_8", Float) = 0
       _k13_9("k13_9", Float) = 0
       _k13_10("k13_10", Float) = 0
       _k13_11("k13_11", Float) = 0
       _k13_12("k13_12", Float) = 0
       _k13_13("k13_13", Float) = 0
       _k13_14("k13_14", Float) = 0
       _k13_15("k13_15", Float) = 0
       _k13_16("k13_16", Float) = 0
       _k13_17("k13_17", Float) = 0
       _k14_0("k14_0", Float) = 0
       _k14_1("k14_1", Float) = 0
       _k14_2("k14_2", Float) = 0
       _k14_3("k14_3", Float) = 0
       _k14_4("k14_4", Float) = 0
       _k14_5("k14_5", Float) = 0
       _k14_6("k14_6", Float) = 0
       _k14_7("k14_7", Float) = 0
       _k14_8("k14_8", Float) = 0
       _k14_9("k14_9", Float) = 0
       _k14_10("k14_10", Float) = 0
       _k14_11("k14_11", Float) = 0
       _k14_12("k14_12", Float) = 0
       _k14_13("k14_13", Float) = 0
       _k14_14("k14_14", Float) = 0
       _k14_15("k14_15", Float) = 0
       _k14_16("k14_16", Float) = 0
       _k14_17("k14_17", Float) = 0

   }

      SubShader
       {
           Tags { "RenderType" = "Opaque" }
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
               float _NumberOfElectrodesW;
               float _NumberOfElectrodesH;
               float _tailleZoneH;
               float _tailleZoneW;
               float _sizeWindowSobel;
               float _rayon;

               float _k0_0;
               float _k0_1;
               float _k0_2;
               float _k0_3;
               float _k0_4;
               float _k0_5;
               float _k0_6;
               float _k0_7;
               float _k0_8;
               float _k0_9;
               float _k0_10;
               float _k0_11;
               float _k0_12;
               float _k0_13;
               float _k0_14;
               float _k0_15;
               float _k0_16;
               float _k0_17;
               float _k1_0;
               float _k1_1;
               float _k1_2;
               float _k1_3;
               float _k1_4;
               float _k1_5;
               float _k1_6;
               float _k1_7;
               float _k1_8;
               float _k1_9;
               float _k1_10;
               float _k1_11;
               float _k1_12;
               float _k1_13;
               float _k1_14;
               float _k1_15;
               float _k1_16;
               float _k1_17;
               float _k2_0;
               float _k2_1;
               float _k2_2;
               float _k2_3;
               float _k2_4;
               float _k2_5;
               float _k2_6;
               float _k2_7;
               float _k2_8;
               float _k2_9;
               float _k2_10;
               float _k2_11;
               float _k2_12;
               float _k2_13;
               float _k2_14;
               float _k2_15;
               float _k2_16;
               float _k2_17;
               float _k3_0;
               float _k3_1;
               float _k3_2;
               float _k3_3;
               float _k3_4;
               float _k3_5;
               float _k3_6;
               float _k3_7;
               float _k3_8;
               float _k3_9;
               float _k3_10;
               float _k3_11;
               float _k3_12;
               float _k3_13;
               float _k3_14;
               float _k3_15;
               float _k3_16;
               float _k3_17;
               float _k4_0;
               float _k4_1;
               float _k4_2;
               float _k4_3;
               float _k4_4;
               float _k4_5;
               float _k4_6;
               float _k4_7;
               float _k4_8;
               float _k4_9;
               float _k4_10;
               float _k4_11;
               float _k4_12;
               float _k4_13;
               float _k4_14;
               float _k4_15;
               float _k4_16;
               float _k4_17;
               float _k5_0;
               float _k5_1;
               float _k5_2;
               float _k5_3;
               float _k5_4;
               float _k5_5;
               float _k5_6;
               float _k5_7;
               float _k5_8;
               float _k5_9;
               float _k5_10;
               float _k5_11;
               float _k5_12;
               float _k5_13;
               float _k5_14;
               float _k5_15;
               float _k5_16;
               float _k5_17;
               float _k6_0;
               float _k6_1;
               float _k6_2;
               float _k6_3;
               float _k6_4;
               float _k6_5;
               float _k6_6;
               float _k6_7;
               float _k6_8;
               float _k6_9;
               float _k6_10;
               float _k6_11;
               float _k6_12;
               float _k6_13;
               float _k6_14;
               float _k6_15;
               float _k6_16;
               float _k6_17;
               float _k7_0;
               float _k7_1;
               float _k7_2;
               float _k7_3;
               float _k7_4;
               float _k7_5;
               float _k7_6;
               float _k7_7;
               float _k7_8;
               float _k7_9;
               float _k7_10;
               float _k7_11;
               float _k7_12;
               float _k7_13;
               float _k7_14;
               float _k7_15;
               float _k7_16;
               float _k7_17;
               float _k8_0;
               float _k8_1;
               float _k8_2;
               float _k8_3;
               float _k8_4;
               float _k8_5;
               float _k8_6;
               float _k8_7;
               float _k8_8;
               float _k8_9;
               float _k8_10;
               float _k8_11;
               float _k8_12;
               float _k8_13;
               float _k8_14;
               float _k8_15;
               float _k8_16;
               float _k8_17;
               float _k9_0;
               float _k9_1;
               float _k9_2;
               float _k9_3;
               float _k9_4;
               float _k9_5;
               float _k9_6;
               float _k9_7;
               float _k9_8;
               float _k9_9;
               float _k9_10;
               float _k9_11;
               float _k9_12;
               float _k9_13;
               float _k9_14;
               float _k9_15;
               float _k9_16;
               float _k9_17;
               float _k10_0;
               float _k10_1;
               float _k10_2;
               float _k10_3;
               float _k10_4;
               float _k10_5;
               float _k10_6;
               float _k10_7;
               float _k10_8;
               float _k10_9;
               float _k10_10;
               float _k10_11;
               float _k10_12;
               float _k10_13;
               float _k10_14;
               float _k10_15;
               float _k10_16;
               float _k10_17;
               float _k11_0;
               float _k11_1;
               float _k11_2;
               float _k11_3;
               float _k11_4;
               float _k11_5;
               float _k11_6;
               float _k11_7;
               float _k11_8;
               float _k11_9;
               float _k11_10;
               float _k11_11;
               float _k11_12;
               float _k11_13;
               float _k11_14;
               float _k11_15;
               float _k11_16;
               float _k11_17;
               float _k12_0;
               float _k12_1;
               float _k12_2;
               float _k12_3;
               float _k12_4;
               float _k12_5;
               float _k12_6;
               float _k12_7;
               float _k12_8;
               float _k12_9;
               float _k12_10;
               float _k12_11;
               float _k12_12;
               float _k12_13;
               float _k12_14;
               float _k12_15;
               float _k12_16;
               float _k12_17;
               float _k13_0;
               float _k13_1;
               float _k13_2;
               float _k13_3;
               float _k13_4;
               float _k13_5;
               float _k13_6;
               float _k13_7;
               float _k13_8;
               float _k13_9;
               float _k13_10;
               float _k13_11;
               float _k13_12;
               float _k13_13;
               float _k13_14;
               float _k13_15;
               float _k13_16;
               float _k13_17;
               float _k14_0;
               float _k14_1;
               float _k14_2;
               float _k14_3;
               float _k14_4;
               float _k14_5;
               float _k14_6;
               float _k14_7;
               float _k14_8;
               float _k14_9;
               float _k14_10;
               float _k14_11;
               float _k14_12;
               float _k14_13;
               float _k14_14;
               float _k14_15;
               float _k14_16;
               float _k14_17;


               //////////////////////////////////////////////////////////////////////////////////////////////
               // Fonction : vert VERTEX SHADER                                                            //
               //////////////////////////////////////////////////////////////////////////////////////////////
               v2f vert(appdata v)
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
               // Fonction : sobel                                                                         //
               // Sémantique : applique un filtrage de sobel au pixel courant considéré                    //
               // Param : - sampler2D tex, échantillonneur de texture                                      //
               //         - float2 uv, coordoonnée de textures considérés                                  //
               // Retour : valeur de sobel aux coordonnées de texture uv                                   //
               //////////////////////////////////////////////////////////////////////////////////////////////
               float sobel(sampler2D tex, float2 uv) {
                   float2 delta = float2(_sizeWindowSobel, _sizeWindowSobel);

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

               //////////////////////////////////////////////////////////////////////////////////////////////
               // Fonction : getZoneFromUV                                                                 //
               // Sémantique : récupère le centre de la zone la plus proche étant données des coordonnées  //
               //              de texture dans i.uv                                                        // /// DOC A CHANGER
               // Param : - v2f i, le pixel considéré                                                      //
               // Retour : float2, coordonnée de texture du pixel central de la zone correspondante        //
               //////////////////////////////////////////////////////////////////////////////////////////////
               float4 getZoneFromUV(v2f i)
               {
                  // Zone trouvée ?
                  bool zoneTrouve = false;
                  // Coord texture
                  float uvX = i.uv.x;
                  float uvY = i.uv.y;
                  // resultat
                  float4 res = (-1,-1,-1,-1);

                  // Pour toutes les zones, est-ce la bonne ?
                  for (int w = 0; w < _NumberOfElectrodesW; w++) {
                      for (int h = 0; h < _NumberOfElectrodesH; h++) {
                         // Si dans la boite englobante 
                         if (h * _tailleZoneH < uvY && uvY < (h + 1) * _tailleZoneH
                         && w * _tailleZoneW < uvX  && uvX < (w + 1) * _tailleZoneW) {
                            // Mettre à jour les valeurs UNE PAR UNE ET PAS LES DEUX A LA FOIS
                            res.x = _tailleZoneW * (w + 0.5);
                            res.y = _tailleZoneH * (h + 0.5);
                            res.z = w;
                            res.w = h;
                            return res;
                         }
                      }
                  }
                  return res;
               }

               //////////////////////////////////////////////////////////////////////////////////////////////
               // Fonction : getPhosphenesADeclancher                                                      //
               //////////////////////////////////////////////////////////////////////////////////////////////
               float getPhosphenesADeclancher(float zoneX, float zoneY) {
                  if (zoneX == 1 && zoneY == 1 && _k0_0) { return 1; }
                  else if (zoneX == 1 && zoneY == 2 && _k0_1) { return 1; }
                  else if (zoneX == 1 && zoneY == 3 && _k0_2) { return 1; }
                  else if (zoneX == 1 && zoneY == 4 && _k0_3) { return 1; }
                  else if (zoneX == 1 && zoneY == 5 && _k0_4) { return 1; }
                  else if (zoneX == 1 && zoneY == 6 && _k0_5) { return 1; }
                  else if (zoneX == 1 && zoneY == 7 && _k0_6) { return 1; }
                  else if (zoneX == 1 && zoneY == 8 && _k0_7) { return 1; }
                  else if (zoneX == 1 && zoneY == 9 && _k0_8) { return 1; }
                  else if (zoneX == 1 && zoneY == 10 && _k0_9) { return 1; }
                  else if (zoneX == 1 && zoneY == 11 && _k0_10) { return 1; }
                  else if (zoneX == 1 && zoneY == 12 && _k0_11) { return 1; }
                  else if (zoneX == 1 && zoneY == 13 && _k0_12) { return 1; }
                  else if (zoneX == 1 && zoneY == 14 && _k0_13) { return 1; }
                  else if (zoneX == 1 && zoneY == 15 && _k0_14) { return 1; }
                  else if (zoneX == 1 && zoneY == 16 && _k0_15) { return 1; }
                  else if (zoneX == 1 && zoneY == 17 && _k0_16) { return 1; }
                  else if (zoneX == 1 && zoneY == 18 && _k0_17) { return 1; }
                  else if (zoneX == 2 && zoneY == 1 && _k1_0) { return 1; }
                  else if (zoneX == 2 && zoneY == 2 && _k1_1) { return 1; }
                  else if (zoneX == 2 && zoneY == 3 && _k1_2) { return 1; }
                  else if (zoneX == 2 && zoneY == 4 && _k1_3) { return 1; }
                  else if (zoneX == 2 && zoneY == 5 && _k1_4) { return 1; }
                  else if (zoneX == 2 && zoneY == 6 && _k1_5) { return 1; }
                  else if (zoneX == 2 && zoneY == 7 && _k1_6) { return 1; }
                  else if (zoneX == 2 && zoneY == 8 && _k1_7) { return 1; }
                  else if (zoneX == 2 && zoneY == 9 && _k1_8) { return 1; }
                  else if (zoneX == 2 && zoneY == 10 && _k1_9) { return 1; }
                  else if (zoneX == 2 && zoneY == 11 && _k1_10) { return 1; }
                  else if (zoneX == 2 && zoneY == 12 && _k1_11) { return 1; }
                  else if (zoneX == 2 && zoneY == 13 && _k1_12) { return 1; }
                  else if (zoneX == 2 && zoneY == 14 && _k1_13) { return 1; }
                  else if (zoneX == 2 && zoneY == 15 && _k1_14) { return 1; }
                  else if (zoneX == 2 && zoneY == 16 && _k1_15) { return 1; }
                  else if (zoneX == 2 && zoneY == 17 && _k1_16) { return 1; }
                  else if (zoneX == 2 && zoneY == 18 && _k1_17) { return 1; }
                  else if (zoneX == 3 && zoneY == 1 && _k2_0) { return 1; }
                  else if (zoneX == 3 && zoneY == 2 && _k2_1) { return 1; }
                  else if (zoneX == 3 && zoneY == 3 && _k2_2) { return 1; }
                  else if (zoneX == 3 && zoneY == 4 && _k2_3) { return 1; }
                  else if (zoneX == 3 && zoneY == 5 && _k2_4) { return 1; }
                  else if (zoneX == 3 && zoneY == 6 && _k2_5) { return 1; }
                  else if (zoneX == 3 && zoneY == 7 && _k2_6) { return 1; }
                  else if (zoneX == 3 && zoneY == 8 && _k2_7) { return 1; }
                  else if (zoneX == 3 && zoneY == 9 && _k2_8) { return 1; }
                  else if (zoneX == 3 && zoneY == 10 && _k2_9) { return 1; }
                  else if (zoneX == 3 && zoneY == 11 && _k2_10) { return 1; }
                  else if (zoneX == 3 && zoneY == 12 && _k2_11) { return 1; }
                  else if (zoneX == 3 && zoneY == 13 && _k2_12) { return 1; }
                  else if (zoneX == 3 && zoneY == 14 && _k2_13) { return 1; }
                  else if (zoneX == 3 && zoneY == 15 && _k2_14) { return 1; }
                  else if (zoneX == 3 && zoneY == 16 && _k2_15) { return 1; }
                  else if (zoneX == 3 && zoneY == 17 && _k2_16) { return 1; }
                  else if (zoneX == 3 && zoneY == 18 && _k2_17) { return 1; }
                  else if (zoneX == 4 && zoneY == 1 && _k3_0) { return 1; }
                  else if (zoneX == 4 && zoneY == 2 && _k3_1) { return 1; }
                  else if (zoneX == 4 && zoneY == 3 && _k3_2) { return 1; }
                  else if (zoneX == 4 && zoneY == 4 && _k3_3) { return 1; }
                  else if (zoneX == 4 && zoneY == 5 && _k3_4) { return 1; }
                  else if (zoneX == 4 && zoneY == 6 && _k3_5) { return 1; }
                  else if (zoneX == 4 && zoneY == 7 && _k3_6) { return 1; }
                  else if (zoneX == 4 && zoneY == 8 && _k3_7) { return 1; }
                  else if (zoneX == 4 && zoneY == 9 && _k3_8) { return 1; }
                  else if (zoneX == 4 && zoneY == 10 && _k3_9) { return 1; }
                  else if (zoneX == 4 && zoneY == 11 && _k3_10) { return 1; }
                  else if (zoneX == 4 && zoneY == 12 && _k3_11) { return 1; }
                  else if (zoneX == 4 && zoneY == 13 && _k3_12) { return 1; }
                  else if (zoneX == 4 && zoneY == 14 && _k3_13) { return 1; }
                  else if (zoneX == 4 && zoneY == 15 && _k3_14) { return 1; }
                  else if (zoneX == 4 && zoneY == 16 && _k3_15) { return 1; }
                  else if (zoneX == 4 && zoneY == 17 && _k3_16) { return 1; }
                  else if (zoneX == 4 && zoneY == 18 && _k3_17) { return 1; }
                  else if (zoneX == 5 && zoneY == 1 && _k4_0) { return 1; }
                  else if (zoneX == 5 && zoneY == 2 && _k4_1) { return 1; }
                  else if (zoneX == 5 && zoneY == 3 && _k4_2) { return 1; }
                  else if (zoneX == 5 && zoneY == 4 && _k4_3) { return 1; }
                  else if (zoneX == 5 && zoneY == 5 && _k4_4) { return 1; }
                  else if (zoneX == 5 && zoneY == 6 && _k4_5) { return 1; }
                  else if (zoneX == 5 && zoneY == 7 && _k4_6) { return 1; }
                  else if (zoneX == 5 && zoneY == 8 && _k4_7) { return 1; }
                  else if (zoneX == 5 && zoneY == 9 && _k4_8) { return 1; }
                  else if (zoneX == 5 && zoneY == 10 && _k4_9) { return 1; }
                  else if (zoneX == 5 && zoneY == 11 && _k4_10) { return 1; }
                  else if (zoneX == 5 && zoneY == 12 && _k4_11) { return 1; }
                  else if (zoneX == 5 && zoneY == 13 && _k4_12) { return 1; }
                  else if (zoneX == 5 && zoneY == 14 && _k4_13) { return 1; }
                  else if (zoneX == 5 && zoneY == 15 && _k4_14) { return 1; }
                  else if (zoneX == 5 && zoneY == 16 && _k4_15) { return 1; }
                  else if (zoneX == 5 && zoneY == 17 && _k4_16) { return 1; }
                  else if (zoneX == 5 && zoneY == 18 && _k4_17) { return 1; }
                  else if (zoneX == 6 && zoneY == 1 && _k5_0) { return 1; }
                  else if (zoneX == 6 && zoneY == 2 && _k5_1) { return 1; }
                  else if (zoneX == 6 && zoneY == 3 && _k5_2) { return 1; }
                  else if (zoneX == 6 && zoneY == 4 && _k5_3) { return 1; }
                  else if (zoneX == 6 && zoneY == 5 && _k5_4) { return 1; }
                  else if (zoneX == 6 && zoneY == 6 && _k5_5) { return 1; }
                  else if (zoneX == 6 && zoneY == 7 && _k5_6) { return 1; }
                  else if (zoneX == 6 && zoneY == 8 && _k5_7) { return 1; }
                  else if (zoneX == 6 && zoneY == 9 && _k5_8) { return 1; }
                  else if (zoneX == 6 && zoneY == 10 && _k5_9) { return 1; }
                  else if (zoneX == 6 && zoneY == 11 && _k5_10) { return 1; }
                  else if (zoneX == 6 && zoneY == 12 && _k5_11) { return 1; }
                  else if (zoneX == 6 && zoneY == 13 && _k5_12) { return 1; }
                  else if (zoneX == 6 && zoneY == 14 && _k5_13) { return 1; }
                  else if (zoneX == 6 && zoneY == 15 && _k5_14) { return 1; }
                  else if (zoneX == 6 && zoneY == 16 && _k5_15) { return 1; }
                  else if (zoneX == 6 && zoneY == 17 && _k5_16) { return 1; }
                  else if (zoneX == 6 && zoneY == 18 && _k5_17) { return 1; }
                  else if (zoneX == 7 && zoneY == 1 && _k6_0) { return 1; }
                  else if (zoneX == 7 && zoneY == 2 && _k6_1) { return 1; }
                  else if (zoneX == 7 && zoneY == 3 && _k6_2) { return 1; }
                  else if (zoneX == 7 && zoneY == 4 && _k6_3) { return 1; }
                  else if (zoneX == 7 && zoneY == 5 && _k6_4) { return 1; }
                  else if (zoneX == 7 && zoneY == 6 && _k6_5) { return 1; }
                  else if (zoneX == 7 && zoneY == 7 && _k6_6) { return 1; }
                  else if (zoneX == 7 && zoneY == 8 && _k6_7) { return 1; }
                  else if (zoneX == 7 && zoneY == 9 && _k6_8) { return 1; }
                  else if (zoneX == 7 && zoneY == 10 && _k6_9) { return 1; }
                  else if (zoneX == 7 && zoneY == 11 && _k6_10) { return 1; }
                  else if (zoneX == 7 && zoneY == 12 && _k6_11) { return 1; }
                  else if (zoneX == 7 && zoneY == 13 && _k6_12) { return 1; }
                  else if (zoneX == 7 && zoneY == 14 && _k6_13) { return 1; }
                  else if (zoneX == 7 && zoneY == 15 && _k6_14) { return 1; }
                  else if (zoneX == 7 && zoneY == 16 && _k6_15) { return 1; }
                  else if (zoneX == 7 && zoneY == 17 && _k6_16) { return 1; }
                  else if (zoneX == 7 && zoneY == 18 && _k6_17) { return 1; }
                  else if (zoneX == 8 && zoneY == 1 && _k7_0) { return 1; }
                  else if (zoneX == 8 && zoneY == 2 && _k7_1) { return 1; }
                  else if (zoneX == 8 && zoneY == 3 && _k7_2) { return 1; }
                  else if (zoneX == 8 && zoneY == 4 && _k7_3) { return 1; }
                  else if (zoneX == 8 && zoneY == 5 && _k7_4) { return 1; }
                  else if (zoneX == 8 && zoneY == 6 && _k7_5) { return 1; }
                  else if (zoneX == 8 && zoneY == 7 && _k7_6) { return 1; }
                  else if (zoneX == 8 && zoneY == 8 && _k7_7) { return 1; }
                  else if (zoneX == 8 && zoneY == 9 && _k7_8) { return 1; }
                  else if (zoneX == 8 && zoneY == 10 && _k7_9) { return 1; }
                  else if (zoneX == 8 && zoneY == 11 && _k7_10) { return 1; }
                  else if (zoneX == 8 && zoneY == 12 && _k7_11) { return 1; }
                  else if (zoneX == 8 && zoneY == 13 && _k7_12) { return 1; }
                  else if (zoneX == 8 && zoneY == 14 && _k7_13) { return 1; }
                  else if (zoneX == 8 && zoneY == 15 && _k7_14) { return 1; }
                  else if (zoneX == 8 && zoneY == 16 && _k7_15) { return 1; }
                  else if (zoneX == 8 && zoneY == 17 && _k7_16) { return 1; }
                  else if (zoneX == 8 && zoneY == 18 && _k7_17) { return 1; }
                  else if (zoneX == 9 && zoneY == 1 && _k8_0) { return 1; }
                  else if (zoneX == 9 && zoneY == 2 && _k8_1) { return 1; }
                  else if (zoneX == 9 && zoneY == 3 && _k8_2) { return 1; }
                  else if (zoneX == 9 && zoneY == 4 && _k8_3) { return 1; }
                  else if (zoneX == 9 && zoneY == 5 && _k8_4) { return 1; }
                  else if (zoneX == 9 && zoneY == 6 && _k8_5) { return 1; }
                  else if (zoneX == 9 && zoneY == 7 && _k8_6) { return 1; }
                  else if (zoneX == 9 && zoneY == 8 && _k8_7) { return 1; }
                  else if (zoneX == 9 && zoneY == 9 && _k8_8) { return 1; }
                  else if (zoneX == 9 && zoneY == 10 && _k8_9) { return 1; }
                  else if (zoneX == 9 && zoneY == 11 && _k8_10) { return 1; }
                  else if (zoneX == 9 && zoneY == 12 && _k8_11) { return 1; }
                  else if (zoneX == 9 && zoneY == 13 && _k8_12) { return 1; }
                  else if (zoneX == 9 && zoneY == 14 && _k8_13) { return 1; }
                  else if (zoneX == 9 && zoneY == 15 && _k8_14) { return 1; }
                  else if (zoneX == 9 && zoneY == 16 && _k8_15) { return 1; }
                  else if (zoneX == 9 && zoneY == 17 && _k8_16) { return 1; }
                  else if (zoneX == 9 && zoneY == 18 && _k8_17) { return 1; }
                  else if (zoneX == 10 && zoneY == 1 && _k9_0) { return 1; }
                  else if (zoneX == 10 && zoneY == 2 && _k9_1) { return 1; }
                  else if (zoneX == 10 && zoneY == 3 && _k9_2) { return 1; }
                  else if (zoneX == 10 && zoneY == 4 && _k9_3) { return 1; }
                  else if (zoneX == 10 && zoneY == 5 && _k9_4) { return 1; }
                  else if (zoneX == 10 && zoneY == 6 && _k9_5) { return 1; }
                  else if (zoneX == 10 && zoneY == 7 && _k9_6) { return 1; }
                  else if (zoneX == 10 && zoneY == 8 && _k9_7) { return 1; }
                  else if (zoneX == 10 && zoneY == 9 && _k9_8) { return 1; }
                  else if (zoneX == 10 && zoneY == 10 && _k9_9) { return 1; }
                  else if (zoneX == 10 && zoneY == 11 && _k9_10) { return 1; }
                  else if (zoneX == 10 && zoneY == 12 && _k9_11) { return 1; }
                  else if (zoneX == 10 && zoneY == 13 && _k9_12) { return 1; }
                  else if (zoneX == 10 && zoneY == 14 && _k9_13) { return 1; }
                  else if (zoneX == 10 && zoneY == 15 && _k9_14) { return 1; }
                  else if (zoneX == 10 && zoneY == 16 && _k9_15) { return 1; }
                  else if (zoneX == 10 && zoneY == 17 && _k9_16) { return 1; }
                  else if (zoneX == 10 && zoneY == 18 && _k9_17) { return 1; }
                  else if (zoneX == 11 && zoneY == 1 && _k10_0) { return 1; }
                  else if (zoneX == 11 && zoneY == 2 && _k10_1) { return 1; }
                  else if (zoneX == 11 && zoneY == 3 && _k10_2) { return 1; }
                  else if (zoneX == 11 && zoneY == 4 && _k10_3) { return 1; }
                  else if (zoneX == 11 && zoneY == 5 && _k10_4) { return 1; }
                  else if (zoneX == 11 && zoneY == 6 && _k10_5) { return 1; }
                  else if (zoneX == 11 && zoneY == 7 && _k10_6) { return 1; }
                  else if (zoneX == 11 && zoneY == 8 && _k10_7) { return 1; }
                  else if (zoneX == 11 && zoneY == 9 && _k10_8) { return 1; }
                  else if (zoneX == 11 && zoneY == 10 && _k10_9) { return 1; }
                  else if (zoneX == 11 && zoneY == 11 && _k10_10) { return 1; }
                  else if (zoneX == 11 && zoneY == 12 && _k10_11) { return 1; }
                  else if (zoneX == 11 && zoneY == 13 && _k10_12) { return 1; }
                  else if (zoneX == 11 && zoneY == 14 && _k10_13) { return 1; }
                  else if (zoneX == 11 && zoneY == 15 && _k10_14) { return 1; }
                  else if (zoneX == 11 && zoneY == 16 && _k10_15) { return 1; }
                  else if (zoneX == 11 && zoneY == 17 && _k10_16) { return 1; }
                  else if (zoneX == 11 && zoneY == 18 && _k10_17) { return 1; }
                  else if (zoneX == 12 && zoneY == 1 && _k11_0) { return 1; }
                  else if (zoneX == 12 && zoneY == 2 && _k11_1) { return 1; }
                  else if (zoneX == 12 && zoneY == 3 && _k11_2) { return 1; }
                  else if (zoneX == 12 && zoneY == 4 && _k11_3) { return 1; }
                  else if (zoneX == 12 && zoneY == 5 && _k11_4) { return 1; }
                  else if (zoneX == 12 && zoneY == 6 && _k11_5) { return 1; }
                  else if (zoneX == 12 && zoneY == 7 && _k11_6) { return 1; }
                  else if (zoneX == 12 && zoneY == 8 && _k11_7) { return 1; }
                  else if (zoneX == 12 && zoneY == 9 && _k11_8) { return 1; }
                  else if (zoneX == 12 && zoneY == 10 && _k11_9) { return 1; }
                  else if (zoneX == 12 && zoneY == 11 && _k11_10) { return 1; }
                  else if (zoneX == 12 && zoneY == 12 && _k11_11) { return 1; }
                  else if (zoneX == 12 && zoneY == 13 && _k11_12) { return 1; }
                  else if (zoneX == 12 && zoneY == 14 && _k11_13) { return 1; }
                  else if (zoneX == 12 && zoneY == 15 && _k11_14) { return 1; }
                  else if (zoneX == 12 && zoneY == 16 && _k11_15) { return 1; }
                  else if (zoneX == 12 && zoneY == 17 && _k11_16) { return 1; }
                  else if (zoneX == 12 && zoneY == 18 && _k11_17) { return 1; }
                  else if (zoneX == 13 && zoneY == 1 && _k12_0) { return 1; }
                  else if (zoneX == 13 && zoneY == 2 && _k12_1) { return 1; }
                  else if (zoneX == 13 && zoneY == 3 && _k12_2) { return 1; }
                  else if (zoneX == 13 && zoneY == 4 && _k12_3) { return 1; }
                  else if (zoneX == 13 && zoneY == 5 && _k12_4) { return 1; }
                  else if (zoneX == 13 && zoneY == 6 && _k12_5) { return 1; }
                  else if (zoneX == 13 && zoneY == 7 && _k12_6) { return 1; }
                  else if (zoneX == 13 && zoneY == 8 && _k12_7) { return 1; }
                  else if (zoneX == 13 && zoneY == 9 && _k12_8) { return 1; }
                  else if (zoneX == 13 && zoneY == 10 && _k12_9) { return 1; }
                  else if (zoneX == 13 && zoneY == 11 && _k12_10) { return 1; }
                  else if (zoneX == 13 && zoneY == 12 && _k12_11) { return 1; }
                  else if (zoneX == 13 && zoneY == 13 && _k12_12) { return 1; }
                  else if (zoneX == 13 && zoneY == 14 && _k12_13) { return 1; }
                  else if (zoneX == 13 && zoneY == 15 && _k12_14) { return 1; }
                  else if (zoneX == 13 && zoneY == 16 && _k12_15) { return 1; }
                  else if (zoneX == 13 && zoneY == 17 && _k12_16) { return 1; }
                  else if (zoneX == 13 && zoneY == 18 && _k12_17) { return 1; }
                  else if (zoneX == 14 && zoneY == 1 && _k13_0) { return 1; }
                  else if (zoneX == 14 && zoneY == 2 && _k13_1) { return 1; }
                  else if (zoneX == 14 && zoneY == 3 && _k13_2) { return 1; }
                  else if (zoneX == 14 && zoneY == 4 && _k13_3) { return 1; }
                  else if (zoneX == 14 && zoneY == 5 && _k13_4) { return 1; }
                  else if (zoneX == 14 && zoneY == 6 && _k13_5) { return 1; }
                  else if (zoneX == 14 && zoneY == 7 && _k13_6) { return 1; }
                  else if (zoneX == 14 && zoneY == 8 && _k13_7) { return 1; }
                  else if (zoneX == 14 && zoneY == 9 && _k13_8) { return 1; }
                  else if (zoneX == 14 && zoneY == 10 && _k13_9) { return 1; }
                  else if (zoneX == 14 && zoneY == 11 && _k13_10) { return 1; }
                  else if (zoneX == 14 && zoneY == 12 && _k13_11) { return 1; }
                  else if (zoneX == 14 && zoneY == 13 && _k13_12) { return 1; }
                  else if (zoneX == 14 && zoneY == 14 && _k13_13) { return 1; }
                  else if (zoneX == 14 && zoneY == 15 && _k13_14) { return 1; }
                  else if (zoneX == 14 && zoneY == 16 && _k13_15) { return 1; }
                  else if (zoneX == 14 && zoneY == 17 && _k13_16) { return 1; }
                  else if (zoneX == 14 && zoneY == 18 && _k13_17) { return 1; }
                  else if (zoneX == 15 && zoneY == 1 && _k14_0) { return 1; }
                  else if (zoneX == 15 && zoneY == 2 && _k14_1) { return 1; }
                  else if (zoneX == 15 && zoneY == 3 && _k14_2) { return 1; }
                  else if (zoneX == 15 && zoneY == 4 && _k14_3) { return 1; }
                  else if (zoneX == 15 && zoneY == 5 && _k14_4) { return 1; }
                  else if (zoneX == 15 && zoneY == 6 && _k14_5) { return 1; }
                  else if (zoneX == 15 && zoneY == 7 && _k14_6) { return 1; }
                  else if (zoneX == 15 && zoneY == 8 && _k14_7) { return 1; }
                  else if (zoneX == 15 && zoneY == 9 && _k14_8) { return 1; }
                  else if (zoneX == 15 && zoneY == 10 && _k14_9) { return 1; }
                  else if (zoneX == 15 && zoneY == 11 && _k14_10) { return 1; }
                  else if (zoneX == 15 && zoneY == 12 && _k14_11) { return 1; }
                  else if (zoneX == 15 && zoneY == 13 && _k14_12) { return 1; }
                  else if (zoneX == 15 && zoneY == 14 && _k14_13) { return 1; }
                  else if (zoneX == 15 && zoneY == 15 && _k14_14) { return 1; }
                  else if (zoneX == 15 && zoneY == 16 && _k14_15) { return 1; }
                  else if (zoneX == 15 && zoneY == 17 && _k14_16) { return 1; }
                  else if (zoneX == 15 && zoneY == 18 && _k14_17) { return 1; }
                  return 0;
               }

               //////////////////////////////////////////////////////////////////////////////////////////////
               // Fonction : frag FRAGMENT SHADER                                                          //
               //////////////////////////////////////////////////////////////////////////////////////////////
               fixed4 frag(v2f i) : SV_Target
               {
                   float4 milieuZone = getZoneFromUV(i);
                   fixed4 col = fixed4(0,0,0,1);
                   float distance = sqrt((milieuZone.x - i.uv.x)*(milieuZone.x - i.uv.x) + (milieuZone.y - i.uv.y) * (milieuZone.y - i.uv.y));
                   if (distance <= _rayon && getPhosphenesADeclancher(milieuZone.z,milieuZone.w)) {
                       col = fixed4(1, 1, 1, 1); 
                       // sobel(_MainTex, milieuZone);
                       //col.rgb = dot(col.rgb, float3(0.3, 0.59, 0.11));
                   }
                   return col;
               }
               ENDCG
           }
       }
}
