using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateurDeTrajectoire : MonoBehaviour
{
    public GameObject sol;
    public GameObject ligne;
    public GameObject mur;
    public GameObject contour;
    public Vector3 PositionDeCreation;
    public Vector2 tailleDeLaMap;

    public string name;
    public GameObject Monde;

    private float offsetHauteur = 0.0f;
    public float deltaOffsetHauteur = 0.0001f;

    public Vector2 PlageNbComposantes;

    private Vector3 prochainePositionCreation;

    private System.Random randObj = new System.Random();

    private float seuil1 = 1.0f / 3.0f;

    private float seuil2 = 2.0f / 3.0f;

    private float decayProba = 0.9f;

    private Vector3 currentDirection = new Vector3(1, 0, 0);

    private Vector3 prochainePos = new Vector3(0,0,0);
    private Vector3 prochaineRot = new Vector3(0,90,0);

    private GameObject map;

    int getNextBuild()
    {
        //int a = randObj.Next(1, 4);
        float r = (float)randObj.NextDouble();
        if(0.0f <= r && r <= seuil1)
        {
            float modif = seuil1 - decayProba * seuil1;
            float demiModif = modif / 2.0f;
            seuil1 -= modif;
            seuil2 -= demiModif;
            return 1;
        } else if (seuil1 < r  && r <= seuil2)
        {
            float modif = (seuil2 - seuil1) - decayProba * (seuil2 - seuil1);
            float demiModif = modif / 2.0f;
            seuil1 += demiModif;
            seuil2 -= demiModif;
            return 2;
        }
        else if(seuil2 < r && r <= 1.0f)
        {
            float modif = (1 - seuil2) - decayProba * (1 - seuil2);
            float demiModif = modif / 2.0f;
            seuil2 += modif;
            seuil1 += demiModif;
            return 3;
        }
        return -1;
    }



    // tmp
    private bool B_pressed = false;
    
    void Start()
    {
        map = new GameObject();

        prochainePositionCreation = PositionDeCreation;
        prochainePos = PositionDeCreation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !B_pressed)
        {
            Debug.Log("BuildMap");
            B_pressed = true;

            // Creation de l'object map
            Destroy(map);
            map = new GameObject();
            map.name = name;

            map.transform.SetParent(Monde.transform);
            map.transform.position = PositionDeCreation;

            // Creation du contour
            GameObject clone = Instantiate(contour, map.transform, false);
            clone.transform.localScale = new Vector3(tailleDeLaMap.x, 0.7f, tailleDeLaMap.y);

            // Creation du chemin
            createChemin(map,clone);


        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            B_pressed = false;
        }
    }

    public void build_trajectoire()
    {
        Debug.Log("BuildMap");
        B_pressed = true;

        // Creation de l'object map
        Destroy(map);
        map = new GameObject();
        map.name = name;

        map.transform.SetParent(Monde.transform);
        map.transform.position = PositionDeCreation;

        // Creation du contour
        GameObject clone = Instantiate(contour, map.transform, false);
        clone.transform.localScale = new Vector3(tailleDeLaMap.x, 0.7f, tailleDeLaMap.y);

        // Creation du chemin
        createChemin(map, clone);
    }

    void addOffset()
    {
        offsetHauteur += deltaOffsetHauteur;
        prochainePositionCreation.y += offsetHauteur;
    }

    void createChemin(GameObject map, GameObject contour)
    {
        GameObject lignes = new GameObject();
        lignes.name = "Lignes";

        int nbComposantes = randObj.Next((int)PlageNbComposantes.x, (int)PlageNbComposantes.y);

        lignes.transform.parent = contour.transform.parent;
        GameObject ligneInstance = Instantiate(ligne, lignes.transform, true);
        ligneInstance.transform.position = prochainePos;
        addOffset();
        ligneInstance.transform.localScale = new Vector3(0.5f, 0.0001f, 0.5f);

        int prev_move = 0;

        for (int i = 0; i < nbComposantes -1; ++i)
        {
            int a = getNextBuild();
            if(a == 1)
            {
                prochainePos += 0.50f * ligneInstance.transform.forward;
                ligneInstance = Instantiate(ligne, lignes.transform, true);
                ligneInstance.transform.position = prochainePos;
                ligneInstance.transform.localScale = new Vector3(0.5f, 0.0001f, 0.5f);
                prochainePos += 0.50f * ligneInstance.transform.forward;
                ligneInstance = Instantiate(ligne, lignes.transform, true);
                ligneInstance.transform.position = prochainePos;
                ligneInstance.transform.localScale = new Vector3(0.5f, 0.0001f, 0.5f);
                prev_move = a;
            }
            if(a == 2 && prev_move != 3)
            {
                prochainePos += 0.50f * ligneInstance.transform.right;
                ligneInstance = Instantiate(ligne, lignes.transform, true);
                ligneInstance.transform.position = prochainePos;
                ligneInstance.transform.localScale = new Vector3(0.5f, 0.0001f, 0.5f);
                prochainePos += 0.50f * ligneInstance.transform.right;
                ligneInstance = Instantiate(ligne, lignes.transform, true);
                ligneInstance.transform.position = prochainePos;
                ligneInstance.transform.localScale = new Vector3(0.5f, 0.0001f, 0.5f);
                prev_move = a;
            }
            if (a == 3 && prev_move != 2)
            {
                prochainePos -= 0.50f * ligneInstance.transform.right;
                ligneInstance = Instantiate(ligne, lignes.transform, true);
                ligneInstance.transform.position = prochainePos;
                ligneInstance.transform.localScale = new Vector3(0.5f, 0.0001f, 0.5f);
                prochainePos -= 0.50f * ligneInstance.transform.right;
                ligneInstance = Instantiate(ligne, lignes.transform, true);
                ligneInstance.transform.position = prochainePos;
                ligneInstance.transform.localScale = new Vector3(0.5f, 0.0001f, 0.5f);
                prev_move = a;
            }
        }
    }

    
}
