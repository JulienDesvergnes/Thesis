using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum TypeDeplacementConstructionLigne
{
    AVANCE,
    TOURNEDROITE,
    TOURNEGAUCHE,
    KODEPLACEMENT
}

public class WorldBuilderScript : MonoBehaviour
{
    // Prefab d'une instance de ligne
    public GameObject PrefabSegment;
    // Prefab d'une instance de contour
    public GameObject PrefabContour;
    // Prefab d'objectif
    public GameObject PrefabObjectif;

    // Générateur d'aléatoire
    private System.Random RandObj = new System.Random();

    // Position de création
    private Vector3 PositionDeCreation = new Vector3(0.0f,0.0f,0.0f);
    // Taille du monde de simulation
    public Vector2 TailleDeLaMap;

    // Référence sur le monde
    public GameObject Monde;

    // Plage de choix aléatoire de componsante
    public Vector2 PlageBlocLigne;

    // Seuils de création des blocs
    private float Seuil1 = 1.0f / 3.0f;
    private float Seuil2 = 2.0f / 3.0f;
    // Decay coeff
    private float DecayCoeff = 0.9f;

    // Conteneur de la carte courante
    private GameObject Carte;

    // Conteneur des morceaux de la ligne
    public GameObject Ligne;

    // A Changer
    // Conteneur du contour
    public GameObject Contour;

    // Ref Sur les images
    public Texture fwd;
    public Texture bckw;
    public Texture rght;
    public Texture lft;

    // Rappel de la position de l'objectif
    public Vector3 PosisitonObjectif;


    public void DestroyAndResetPreviousWorld()
    {
        PositionDeCreation = new Vector3(0.0f, 0.0f, 0.0f);
        Seuil1 = 1.0f / 3.0f;
        Seuil2 = 2.0f / 3.0f;
        DecayCoeff = 0.9f;
    }

    // Récupère la prochaine direction de création
    TypeDeplacementConstructionLigne getNextTypeDeplacementConstructionLigne()
    {
        // Génère un float entre 0.0f et 1.0f
        float r = (float)RandObj.NextDouble();
        // Si la proba est telle que 0.0f | p | seuil1 on décale seuil1 vers la gauche et seuil2 vers la gauche
        if (0.0f <= r && r <= Seuil1)
        {
            // On récupère la proportion de modification
            //float modif = Seuil1 - DecayCoeff * Seuil1;
            //float demiModif = modif / 2.0f;
            // On fait les maj
            //Seuil1 -= modif;
            //Seuil2 -= demiModif;
            return TypeDeplacementConstructionLigne.AVANCE;
        }
        // Si la proba est telle que seuil1 | p | seuil2 on décale seuil1 vers la droite et seuil2 vers la gauche
        else if (Seuil1 < r && r <= Seuil2)
        {
            // On récupère la proportion de modification
            //float modif = (Seuil2 - Seuil1) - DecayCoeff * (Seuil2 - Seuil1);
            //float demiModif = modif / 2.0f;
            // On fait les maj
            //Seuil1 += demiModif;
            //Seuil2 -= demiModif;
            return TypeDeplacementConstructionLigne.TOURNEDROITE;
        }
        // Si la proba est telle que seuil2 | p | 1.0f on décale seuil2 vers la droite et seuil1 vers la droite
        else if (Seuil2 < r && r <= 1.0f)
        {
            // On récupère la proportion de modification
            //float modif = (1 - Seuil2) - DecayCoeff * (1 - Seuil1);
            //float demiModif = modif / 2.0f;
            // On fait les maj
            //Seuil2 += modif;
            //Seuil1 += demiModif;
            return TypeDeplacementConstructionLigne.TOURNEGAUCHE;
        }
        return TypeDeplacementConstructionLigne.KODEPLACEMENT;
    }

    void Start()
    {
        // Creation de l'object map
        Carte = new GameObject();
        Carte.name = "Carte";

        Carte.transform.SetParent(Monde.transform);
        Carte.transform.position = PositionDeCreation;

        // Creation du contour
        Contour = Instantiate(PrefabContour, Carte.transform, false);
        Contour.transform.localScale = new Vector3(TailleDeLaMap.x, 0.7f, TailleDeLaMap.y);
    }

    void Update()
    {
    }

    public void build_trajectoire()
    {
        // Creation du chemin
        createChemin(Carte, Contour);
    }

    // Instancie un segment de la ligne
    public GameObject InstancieSegment(Vector3 position, Vector3 scale, GameObject ligne)
    {
        GameObject InstanceDeSegment = Instantiate(PrefabSegment, ligne.transform, true);
        InstanceDeSegment.transform.position = position;
        InstanceDeSegment.transform.localScale = scale;
        InstanceDeSegment.tag = "Segment";
        return InstanceDeSegment;
    }

    void createChemin(GameObject map, GameObject contour)
    {
        Ligne = new GameObject();
        Ligne.name = "Ligne";
        Ligne.transform.parent = map.transform.parent;

        // Nombre de composantes de la ligne
        int NombreDeComposantes = RandObj.Next((int)PlageBlocLigne.x, (int)PlageBlocLigne.y);

        // Instancie le premier segment
        GameObject DerniereInstanceDeSegment = InstancieSegment(PositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);

        // Descipteur de construction précédente
        TypeDeplacementConstructionLigne DescripteurDerniereConstruction = TypeDeplacementConstructionLigne.AVANCE;
        // Conteneur du prochain lieu de création
        Vector3 ProchainePositionDeCreation = PositionDeCreation;

        // Direction
        // SensDeParcours AlignementDeParcours = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
        // AlignementDeParcours.Direction = DerniereInstanceDeSegment.transform.right;

        // MAJ de la direction du précédent segment
        SensDeParcours AlignementDeParcoursPremier = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
        AlignementDeParcoursPremier.Direction = DerniereInstanceDeSegment.transform.forward;
        DerniereInstanceDeSegment.GetComponents<MeshRenderer>()[0].material.mainTexture = bckw;
        ProchainePositionDeCreation += 1.0f * DerniereInstanceDeSegment.transform.forward;
        DerniereInstanceDeSegment = InstancieSegment(ProchainePositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);

        // MAJ de la direction du précédent segment
        AlignementDeParcoursPremier = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
        AlignementDeParcoursPremier.Direction = DerniereInstanceDeSegment.transform.forward;
        DerniereInstanceDeSegment.GetComponents<MeshRenderer>()[0].material.mainTexture = bckw;
        ProchainePositionDeCreation += 1.0f * DerniereInstanceDeSegment.transform.forward;
        DerniereInstanceDeSegment = InstancieSegment(ProchainePositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);
        DescripteurDerniereConstruction = TypeDeplacementConstructionLigne.AVANCE;

        // On construit le reste de la ligne
        for (int i = 0; i < NombreDeComposantes - 2; ++i)
        {
            TypeDeplacementConstructionLigne ProchaineConstruction = getNextTypeDeplacementConstructionLigne();
            if (ProchaineConstruction == TypeDeplacementConstructionLigne.AVANCE) 
            {
                // MAJ de la direction du précédent segment
                SensDeParcours AlignementDeParcours = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
                AlignementDeParcours.Direction = DerniereInstanceDeSegment.transform.forward;
                DerniereInstanceDeSegment.GetComponents<MeshRenderer>()[0].material.mainTexture = bckw;
                ProchainePositionDeCreation += 1.0f * DerniereInstanceDeSegment.transform.forward;
                DerniereInstanceDeSegment = InstancieSegment(ProchainePositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);

                // MAJ de la direction du précédent segment
                AlignementDeParcours = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
                AlignementDeParcours.Direction = DerniereInstanceDeSegment.transform.forward;
                DerniereInstanceDeSegment.GetComponents<MeshRenderer>()[0].material.mainTexture = bckw;
                ProchainePositionDeCreation += 1.0f * DerniereInstanceDeSegment.transform.forward;
                DerniereInstanceDeSegment = InstancieSegment(ProchainePositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);
                DescripteurDerniereConstruction = TypeDeplacementConstructionLigne.AVANCE;
            }
            if (ProchaineConstruction == TypeDeplacementConstructionLigne.TOURNEDROITE && DescripteurDerniereConstruction != TypeDeplacementConstructionLigne.TOURNEGAUCHE)
            {
                // MAJ de la direction du précédent segment
                SensDeParcours AlignementDeParcours = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
                AlignementDeParcours.Direction = DerniereInstanceDeSegment.transform.right;
                DerniereInstanceDeSegment.GetComponents<MeshRenderer>()[0].material.mainTexture = lft;
                ProchainePositionDeCreation += 1.0f * DerniereInstanceDeSegment.transform.right;
                DerniereInstanceDeSegment = InstancieSegment(ProchainePositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);

                // MAJ de la direction du précédent segment
                AlignementDeParcours = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
                AlignementDeParcours.Direction = DerniereInstanceDeSegment.transform.right;
                DerniereInstanceDeSegment.GetComponents<MeshRenderer>()[0].material.mainTexture = lft;
                ProchainePositionDeCreation += 1.0f * DerniereInstanceDeSegment.transform.right;
                DerniereInstanceDeSegment = InstancieSegment(ProchainePositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);
                DescripteurDerniereConstruction = TypeDeplacementConstructionLigne.TOURNEDROITE;
            }
            if (ProchaineConstruction == TypeDeplacementConstructionLigne.TOURNEGAUCHE && DescripteurDerniereConstruction != TypeDeplacementConstructionLigne.TOURNEDROITE)
            {
                // MAJ de la direction du précédent segment
                SensDeParcours AlignementDeParcours = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
                AlignementDeParcours.Direction = -DerniereInstanceDeSegment.transform.right;
                DerniereInstanceDeSegment.GetComponents<MeshRenderer>()[0].material.mainTexture = rght;
                ProchainePositionDeCreation -= 1.0f * DerniereInstanceDeSegment.transform.right;
                DerniereInstanceDeSegment = InstancieSegment(ProchainePositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);

                // MAJ de la direction du précédent segment
                AlignementDeParcours = DerniereInstanceDeSegment.GetComponent<SensDeParcours>();
                AlignementDeParcours.Direction = -DerniereInstanceDeSegment.transform.right;
                DerniereInstanceDeSegment.GetComponents<MeshRenderer>()[0].material.mainTexture = rght;
                ProchainePositionDeCreation -= 1.0f * DerniereInstanceDeSegment.transform.right;
                DerniereInstanceDeSegment = InstancieSegment(ProchainePositionDeCreation, new Vector3(1.0f, 0.0001f, 1.0f), Ligne);
                DescripteurDerniereConstruction = TypeDeplacementConstructionLigne.TOURNEGAUCHE;
            }
        }

        // On ajoute l'objectif en fin de parcours
        ProchainePositionDeCreation += new Vector3(0.0f, 1.0f, 0.0f);
        GameObject InstanceDObjectif = Instantiate(PrefabObjectif, Ligne.transform, true);
        InstanceDObjectif.transform.position = ProchainePositionDeCreation;
        InstanceDObjectif.transform.localScale = new Vector3(2.0f,2.0f,2.0f);

        PosisitonObjectif = ProchainePositionDeCreation;
    }
}
