using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class CalculDifference : MonoBehaviour
{
    public GameObject Sujet;
    public GameObject Lignes;
    private List<GameObject> LignesGO;
    private List<List<Vector3>> listesPositionsMarqueurs;
    public float largeurLigne = 0.5f;
    public float DISTANCESTOCKEE;

    public Text[] texts;

    private GameObject ligneCourante = null;

    void Start()
    {
        listesPositionsMarqueurs = new List<List<Vector3>>();
        LignesGO = new List<GameObject>();

        // GetMarqueursPositions
        int nombreDeLignes = Lignes.transform.childCount;
        for(int i = 0; i < nombreDeLignes; ++i) {
            List<Vector3> positions = new List<Vector3>();
            GameObject ligne = Lignes.transform.GetChild(i).gameObject;
            LignesGO.Add(ligne.transform.GetChild(2).gameObject);
            GameObject marqueur1 = ligne.transform.GetChild(0).gameObject;
            GameObject marqueur2 = ligne.transform.GetChild(1).gameObject;
            positions.Add(marqueur1.transform.position);
            positions.Add(marqueur2.transform.position);
            listesPositionsMarqueurs.Add(positions);
        }
    }

    public float calculDistance() {
        Vector3 positionSujet = Sujet.transform.position;
        float distance = Mathf.Infinity;
        GameObject LignePlusProche = null;
        float oldDistance = 0.0f;
        for(int i = 0; i < listesPositionsMarqueurs.Count; ++i) {

            oldDistance = distance;
            Vector3 positionMarqueur1 = listesPositionsMarqueurs[i][0];
            Vector3 positionMarqueur2 = listesPositionsMarqueurs[i][1];
            Vector3 AP = positionSujet - positionMarqueur1;
            Vector3 AB = positionMarqueur2 - positionMarqueur1;
            float t = Vector3.Dot(AP,AB) / (AB.magnitude * AB.magnitude);
            t = Mathf.Min(Mathf.Max(t,0.0f),1.0f);
            Vector3 Pproj = positionMarqueur1 + AB * t;
            Vector3 PPproj = Pproj - positionSujet;
            PPproj.y = 0.0f;
            float nd = /*Mathf.Max(*/PPproj.magnitude/*, 0.0f)*/;
            string s = nd.ToString("F2");
            //print(positionMarqueur1);
            //print(positionMarqueur2);
            //print(AP);
            //print(AB);
            //print(t);
            //print(Pproj);
            //print(PPproj);
            //print(nd);
            distance = Mathf.Min(distance, nd);
            texts[i].text = "DistanceLigne" + (i + 1).ToString() +  " = " + s;
            
            // Si diminution de distance
            if(oldDistance - distance > 0.0f) {
                LignePlusProche = LignesGO[i];
            }
        }
        // distance =  Mathf.Max(distance - largeurLigne, 0.0f);

        // Montre la plus proche et trace un trait
        if(LignePlusProche != null) {
            if(ligneCourante != LignePlusProche) {
                foreach(GameObject g in LignesGO) {
                    g.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                }
            }
            ligneCourante = LignePlusProche;
            LignePlusProche.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

        DISTANCESTOCKEE = distance;
        return distance;
    }

    // public float calculDistance() {
    //     Vector3 positionSujet = Sujet.transform.position;
    //     float distance = Mathf.Infinity;
    //     GameObject LignePlusProche = null;
    //     float oldDistance = 0.0f;
    //     for(int i = 0; i < listesPositionsMarqueurs.Count; ++i) {
    //         oldDistance = distance;
    //         Vector3 positionMarqueur1 = listesPositionsMarqueurs[i][0];
    //         Vector3 positionMarqueur2 = listesPositionsMarqueurs[i][1];
    //         if(positionMarqueur1.x == positionMarqueur2.x) {
    //             print(distance.ToString() + " " + Mathf.Abs(positionMarqueur1.x - positionSujet.x).ToString());
    //             float nd = Mathf.Abs(positionMarqueur1.x - positionSujet.x);
    //             string s = nd.ToString("F1");
    //             distance = Mathf.Min(distance, Mathf.Abs(positionMarqueur1.x - positionSujet.x));
    //             texts[i].text = "DistanceLigne" + (i + 1).ToString() +  " = " + s;
    //         } else {
    //             float m = (positionMarqueur2.z - positionMarqueur1.z) / (positionMarqueur2.x - positionMarqueur2.z);
    //             float p = positionMarqueur1.z - m * positionMarqueur1.x;
    //             print(distance.ToString() + " " + (Mathf.Abs(positionSujet.z - m * positionSujet.x - p) / (Mathf.Sqrt(1 + m*m))).ToString());
    //             float nd = Mathf.Abs(positionSujet.z - m * positionSujet.x - p) / (Mathf.Sqrt(1 + m*m));
    //             string s = nd.ToString("F1");
    //             texts[i].text = "DistanceLigne" + (i + 1).ToString() +  " = " + s;
    //             distance = Mathf.Min(distance, Mathf.Abs(positionSujet.z - m * positionSujet.x - p) / (Mathf.Sqrt(1 + m*m)));
    //         }
    //         // Si diminution de distance
    //         if(oldDistance - distance > 0.0f) {
    //             LignePlusProche = LignesGO[i];
    //         }
    //     }
    //     // distance =  Mathf.Max(distance - largeurLigne, 0.0f);

    //     // Montre la plus proche et trace un trait
    //     if(LignePlusProche != null) {
    //         if(ligneCourante != LignePlusProche) {
    //             foreach(GameObject g in LignesGO) {
    //                 g.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    //             }
    //         }
    //         ligneCourante = LignePlusProche;
    //         LignePlusProche.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    //     }

    //     return distance;
    // }
}
