using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DeplacementAutonome : MonoBehaviour
{

    public float movementSpeed = 5.0f;
    public bool seeSomething = true;
    public Vector3 DirectionEnCasDeConfusion;
    public GameObject CalculateurDePosition;
    public Canvas canvas;
    private float oldDistance = 0.0f;
    private int sensDeRecalage = 0; // 0 si gauche 1 si droite
    private int pointDeRelaisCourant = 0;

    public PointDeRelais[] pdr;

    // Start is called before the first frame update
    void Start()
    {
        DirectionEnCasDeConfusion = generateRandomDir();
    }

    public Vector3 generateRandomDir() {
        var rand = new System.Random();
        float x = (float)rand.NextDouble() * 2 - 1;
        float z = (float)rand.NextDouble() * 2 - 1;
        int switcher = rand.Next(4);
        Vector3 res = new Vector3();
        switch (switcher)
        {
            case 0:
                res = Vector3.Normalize(new Vector3(1.0f,0.0f,0.0f));
                break;
            case 1:
                res = Vector3.Normalize(new Vector3(-1.0f,0.0f,0.0f));
                break;
            case 2:
                res = Vector3.Normalize(new Vector3(0.0f,0.0f,1.0f));
                break;
            case 3:
                res = Vector3.Normalize(new Vector3(0.0f,0.0f,-1.0f));
                break;
            default:
                break;
        }
        return res;
    }

    void suivreLigne() {
        bool aligneAvecLigne = false;
        bool bonneDirection = false;
        bool croisement = false;
        bool murEnFace = false;
        RaycastHit hitFront;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitFront, 1.0f);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1.0f, Color.green);
        RaycastHit hitDevant;
        Physics.Raycast(transform.position, transform.TransformDirection(1.0f/2.0f * Vector3.forward - Vector3.up), out hitDevant, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.TransformDirection(1.0f/2.0f * Vector3.forward - Vector3.up) * hitDevant.distance, Color.yellow);
        RaycastHit hitDerriere;
        Physics.Raycast(transform.position, transform.TransformDirection(- 1.0f/2.0f * Vector3.forward - Vector3.up), out hitDerriere, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.TransformDirection(- 1.0f/2.0f * Vector3.forward - Vector3.up) * hitDerriere.distance, Color.yellow);
        RaycastHit hitGauche;
        Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up + 1.0f/2.0f * Vector3.right), out hitGauche, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up + 1.0f/2.0f * Vector3.right) * hitDerriere.distance, Color.yellow);
        RaycastHit hitDroite;
        Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up - 1.0f/2.0f * Vector3.right), out hitDroite, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up - 1.0f/2.0f * Vector3.right) * hitDroite.distance, Color.yellow);

        if(hitDevant.collider.gameObject.tag == "Segment" || hitDerriere.collider.gameObject.tag == "Segment"
        || hitDroite.collider.gameObject.tag == "Segment" || hitGauche.collider.gameObject.tag == "Segment") {
            aligneAvecLigne = true;
        }
        
        if(hitDevant.collider.gameObject.tag == "Segment" && hitDroite.collider.gameObject.tag == "Segment" ||
           hitDevant.collider.gameObject.tag == "Segment" && hitGauche.collider.gameObject.tag == "Segment" ||
           hitDerriere.collider.gameObject.tag == "Segment" && hitDroite.collider.gameObject.tag == "Segment" ||
           hitDerriere.collider.gameObject.tag == "Segment" && hitGauche.collider.gameObject.tag == "Segment") {
               croisement = true;
           }

        if(aligneAvecLigne) {
            RaycastHit hitDessous;
            Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hitDessous, Mathf.Infinity);
            SensDeParcours sdp = hitDessous.collider.gameObject.GetComponent<SensDeParcours>(); 
            if (sdp != null) {
                Vector3 sensReel = sdp.Direction; 
                bonneDirection = Vector3.Dot(sensReel.normalized,transform.forward.normalized) == 1.0;
            }
        }

        if(hitFront.collider != null) {
            if(hitFront.collider.gameObject != null) {
                if(hitFront.collider.gameObject.tag == "Mur") {
                    murEnFace = true;
                }
            }
        }

        Text[] texts = GameObject.FindGameObjectWithTag("Canvas").GetComponentsInChildren<Text>();
        foreach(Text text in texts)
        {
            if(text.name == "Aligne")
            {
                if(aligneAvecLigne) {
                    text.color = Color.green;    
                } else {
                    text.color = Color.red;
                }
                
            }
            if(text.name == "Croisement")
            {
                if(croisement) {
                    text.color = Color.green;    
                } else {
                    text.color = Color.red;
                }
                
            }
            if(text.name == "BonneDirection")
            {
                if(bonneDirection) {
                    text.color = Color.green;    
                } else {
                    text.color = Color.red;
                }
                
            }

            /*if (murEnFace && hitDerriere.collider.gameObject.tag == "Segment" && !bonneDirection) {
                transform.Rotate(0.0f,90.0f,0.0f,Space.Self);
            } else if(croisement && !bonneDirection) {
                transform.Rotate(0.0f,90.0f,0.0f,Space.Self);
            } else if (!croisement && !bonneDirection && aligneAvecLigne) {
                transform.Rotate(0.0f,180.0f,0.0f,Space.Self);
                // transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
            } else if(aligneAvecLigne && bonneDirection) {
                transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
            }*/

            if (hitDevant.collider.gameObject.tag == "Segment" && bonneDirection) {
                transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
            } else {
                if(hitDroite.collider.gameObject.tag == "Segment") {
                    transform.Rotate(0.0f,90.0f,0.0f,Space.Self);
                } else if (hitGauche.collider.gameObject.tag == "Segment") {
                    transform.Rotate(0.0f,-90.0f,0.0f,Space.Self);
                } else if (hitDerriere.collider.gameObject.tag == "Segment") {
                    transform.Rotate(0.0f,180.0f,0.0f,Space.Self);
                }
            }

            //print(CalculateurDePosition.GetComponent<CalculDifference>().calculDistance());
        }
    }

    GameObject getLigneDevantPlusProche() {
        Vector3 debut = - Vector3.up;
        Vector3 fin = Vector3.forward;
        float nb = 180.0f;
        Vector3 pas = (fin - debut) / nb;
        for(int i = 0; i < nb; ++i) {
            RaycastHit hitDevant;
            Physics.Raycast(transform.position, transform.TransformDirection(debut + i * pas), out hitDevant, Mathf.Infinity);
            Debug.DrawRay(transform.position, transform.TransformDirection(debut + i * pas) * hitDevant.distance, Color.yellow);
            if(hitDevant.collider.gameObject.tag == "Segment") {
                return hitDevant.collider.gameObject;
            }
        }
        return null;
    }

    void recalageSurLaLigne(GameObject ligneDevantLaPlusProche) {
        /*oldDistance = CalculateurDePosition.GetComponent<CalculDifference>().calculDistance();
        if (sensDeRecalage == 0) {
            // Gauche
            transform.position = transform.position + (-transform.right * movementSpeed * Time.fixedDeltaTime);
        } else {
            transform.position = transform.position + (transform.right * movementSpeed * Time.fixedDeltaTime);
        }
        if(oldDistance >= CalculateurDePosition.GetComponent<CalculDifference>().calculDistance()) {
            sensDeRecalage = 1 - sensDeRecalage;
        }*/

        Vector3 positionLigne = ligneDevantLaPlusProche.transform.position;
        float step =  movementSpeed * Time.fixedDeltaTime; // calculate distance to move
        int x = 0;
        int z = 0;
        if(Mathf.Abs(positionLigne.x - transform.position.x) < Mathf.Abs(positionLigne.z - transform.position.z)) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(positionLigne.x,transform.position.y,transform.position.z), step);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,transform.position.y,positionLigne.z), step);
        }
        transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
    }

    void rejoindreLigne() {
        GameObject ligneDevantLaPlusProche = getLigneDevantPlusProche();
        if( ligneDevantLaPlusProche == null) {
            transform.Rotate(0.0f,90.0f,0.0f,Space.Self);
        } else {
            // recalageSurLaLigne(ligneDevantLaPlusProche);
            transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
        }
    }

    public PointDeRelais getPointDeRelais() {
        foreach(PointDeRelais p in pdr)
        {
            if(Vector3.Distance(transform.position, p.transform.position) <= 0.1f && !p.DejaPris) {
                return p;
            }
        }
        return null;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     print(CalculateurDePosition.GetComponent<CalculDifference>().calculDistance());
    //     if (seeSomething) {
    //         // Si la distance à la ligne est nulle, on peut la suivre
    //         if(CalculateurDePosition.GetComponent<CalculDifference>().calculDistance() <= 0.05f) {
    //             suivreLigne();
    //         } else {
    //             // Sinon on la rejoint le plus vite possible
    //             rejoindreLigne();
    //         }

    //         // Raycasts
    //         /*RaycastHit hitDevant;
    //         if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward - Vector3.up), out hitDevant, Mathf.Infinity))
    //         {
    //             Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward - Vector3.up) * hitDevant.distance, Color.yellow);
    //         }
    //         RaycastHit hitDroite;
    //         if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward - Vector3.up + Vector3.right), out hitDroite, Mathf.Infinity))
    //         {
    //             Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward - Vector3.up + Vector3.right) * hitDroite.distance, Color.yellow);
    //         }
    //         RaycastHit hitGauche;
    //         if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward - Vector3.up - Vector3.right), out hitGauche, Mathf.Infinity))
    //         {
    //             Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward - Vector3.up - Vector3.right) * hitGauche.distance, Color.yellow);
    //         }

    //         transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);

    //         if(hitDroite.collider.gameObject.tag == "Segment") {
    //             transform.Rotate(0.0f,18.0f,0.0f,Space.Self);
    //         } else if(hitGauche.collider.gameObject.tag == "Segment") {
    //             transform.Rotate(0.0f,-18.0f,0.0f,Space.Self);
    //         }*/
    //     } else {
    //         transform.position = transform.position + (DirectionEnCasDeConfusion * movementSpeed * Time.fixedDeltaTime);
    //     }
    //}

    void aligneAvecLigne() {
        RaycastHit hitDroite;
        if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up + Vector3.right), out hitDroite, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up + Vector3.right) * hitDroite.distance, Color.yellow);
        }
        RaycastHit hitGauche;
        if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up - Vector3.right), out hitGauche, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up - Vector3.right) * hitGauche.distance, Color.yellow);
        }

        if(hitDroite.collider.gameObject.tag == "Segment" && hitGauche.collider.gameObject.tag == "Segment") {
            transform.Rotate(0.0f,90.0f,0.0f,Space.Self);
        }
    }

    void Update() {
        float d = CalculateurDePosition.GetComponent<CalculDifference>().calculDistance();
        Text[] texts = GameObject.FindGameObjectWithTag("Canvas").GetComponentsInChildren<Text>();
        foreach(Text text in texts)
        {
            if(text.name == "Distance")
            {
                text.text = "Distance = " + d.ToString();
            }
        }

        if(seeSomething && d <= 0.05f) {
            aligneAvecLigne();
            transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
            PointDeRelais p = getPointDeRelais();
            if(p != null) {
                p.DejaPris = true;
                float ry = p.transform.rotation.eulerAngles.y;
                transform.position = p.transform.position;
                transform.Rotate(0.0f,ry,0.0f,Space.Self);
                pointDeRelaisCourant = Mathf.Min(pointDeRelaisCourant + 1, pdr.Count() - 1);
            }
        } else if (seeSomething && d > 0.05f) {
            rejoindreLigne();
        } else if (!seeSomething) {
            transform.position = transform.position + (DirectionEnCasDeConfusion * movementSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag != "Objectif") {
            if(!seeSomething) {
                DirectionEnCasDeConfusion = generateRandomDir();
            }
        }

        if(col.gameObject.tag == "Mur") {
            transform.Rotate(0.0f,180.0f,0.0f,Space.Self);
        }

        /*if(col.gameObject.tag != "PointRelais" && seeSomething) {
            float ry = col.gameObject.transform.rotation.y;
            transform.Rotate(0.0f,ry,0.0f,Space.Self);
        }*/
    }
}
