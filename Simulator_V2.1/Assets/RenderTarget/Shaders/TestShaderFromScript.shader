Shader "Custom/TestShaderFromScript"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NumberOfElectrodesW ("NBEW",Float) = 18
        _NumberOfElectrodesH ("NBEH",Float) = 15
        _tailleZoneW ("tailleZoneW",Float) = 0.05555555555 // = 1/18 en flotant
        _tailleZoneH ("tailleZoneH",Float) = 0.06666666666 // = 1/15 en flotant
        _sizeWindowSobel("SizeWindowSobel", Float) = 0.002
        _rayon("rayon", Float) = 0.02
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
            float _NumberOfElectrodesW;
            float _NumberOfElectrodesH;
            float _tailleZoneH;
            float _tailleZoneW;
            float _sizeWindowSobel;
            float _rayon;
            
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
            //              de texture dans i.uv                                                        //
            // Param : - v2f i, le pixel considéré                                                      //
            // Retour : float2, coordonnée de texture du pixel central de la zone correspondante        //
            //////////////////////////////////////////////////////////////////////////////////////////////
            float2 getZoneFromUV(v2f i)
            {
                // Zone trouvée ?
                bool zoneTrouve = false;
                // Coord texture
                float uvX = i.uv.x;
                float uvY = i.uv.y;
                // resultat
                float2 res = (-1,-1);

                // Pour toutes les zones, est-ce la bonne ?
                for (int w = 0; w < _NumberOfElectrodesW; w++) {
                    for (int h = 0; h < _NumberOfElectrodesH; h++) {
                        // Si dans la boite englobante 
                        if (h * _tailleZoneH < uvY && uvY < (h + 1) * _tailleZoneH
                        && w * _tailleZoneW < uvX  && uvX < (w + 1) * _tailleZoneW) {
                            // Mettre à jour les valeurs UNE PAR UNE ET PAS LES DEUX A LA FOIS
                            res.x = _tailleZoneW * (w + 0.5);
                            res.y = _tailleZoneH * (h + 0.5);
                            return res;
                        }
                    }
                }
                return res;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////
            // Fonction : frag FRAGMENT SHADER                                                          //
            //////////////////////////////////////////////////////////////////////////////////////////////
            fixed4 frag (v2f i) : SV_Target
            {
                float2 milieuZone = getZoneFromUV(i);
                fixed4 col = fixed4(0,0,0,1);
                float distance = sqrt((milieuZone.x - i.uv.x)*(milieuZone.x - i.uv.x) + (milieuZone.y - i.uv.y) * (milieuZone.y - i.uv.y));
                if (distance <= _rayon) {
                    col = sobel(_MainTex, milieuZone);
                    col.rgb = dot(col.rgb, float3(0.3, 0.59, 0.11));
                }
                return col;
            }
            ENDCG
        }
    }
}
