using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Text;

public class AgentDeDeplacement23 : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    private float m_TimeSinceDecision;

    // Les deux types de rendus
    public Shader shaderAllBlack;
    public Shader shaderWireFrame;

    // La surface de rendu
    public GameObject RenderPlane;
    private Renderer renderer;

    // Type de rendu courant 0 = Noir, 1 = WF
    int renduCourant = 1;

    // Position et Rotation Initiales
    private Vector3 positionInitiale;
    private Quaternion rotationInitiale;

    // Objectif
    public GameObject Objectif;

    // Vitesse de déplacement
    public float movementSpeed = 5.0f;


    // Action
    const int k_NoAction = 6;
    const int k_MoveForward = 0;  
    //const int k_MoveBackward = 1;
    //const int k_MoveLeft = 2;
    //const int k_MoveRight = 3;
    const int k_RotateRight = 1;
    const int k_RotateLeft = 2;
    

    // CalculateurDeDistance
    public GameObject CalculateurDeDistance;

    public GameObject Lumieres;

    // PointDeRelais
    public PointDeRelais[] pointDeRelais;

    // Distance Initiale
    float distanceInitiale = 0.0f;

    // prochaineLigne
    int numLigneAPasser = 1;

    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    void Update() {
        /*foreach(PointDeRelais p in pointDeRelais)
        {
            if(Vector3.Distance(transform.position, p.transform.position) <= 0.1f && !p.DejaPris /*&& aligneAvecLigneDevant()/) {
                p.DejaPris = true;
                AddReward(100.0f);
                numLigneAPasser++;
            }
        }*/
    }

    void Start() {
        renderer = RenderPlane.GetComponent<Renderer>();
        positionInitiale = transform.position;
        rotationInitiale = transform.rotation;
    }

    void WaitTimeInference()
    {
        if (Academy.Instance.IsCommunicatorOn)
        {
            RequestDecision();
        }
        else
        {
            if (m_TimeSinceDecision >= timeBetweenDecisionsAtInference)
            {
                m_TimeSinceDecision = 0f;
                RequestDecision();
            }
            else
            {
                m_TimeSinceDecision += Time.fixedDeltaTime;
            }
        }
    }

    public override void OnActionReceived(float[] vectorAction) {
        var action = Mathf.FloorToInt(vectorAction[0]);

        if (action == k_MoveForward) {
            transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
        //} else if (action == k_MoveBackward) {
        //   transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
        //} else if (action == k_MoveLeft) {
        //   transform.position = transform.position + (-transform.right * movementSpeed * Time.fixedDeltaTime);
        //} else if (action == k_MoveRight) {
        //   transform.position = transform.position + (transform.right * movementSpeed * Time.fixedDeltaTime);
        } else if (action == k_RotateRight) {
           transform.Rotate(0.0f,90.0f,0.0f,Space.Self);
        } else if (action == k_RotateLeft) {
          transform.Rotate(0.0f,-90.0f,0.0f,Space.Self);
        } else if(action == k_NoAction) {
        
        } else {
            throw new ArgumentException("Invalid action value");    
        }
        
        bool b_surLaLigne = SurLaLigne();
        bool b_aligneAvecLigneDevant = aligneAvecLigneDevant();
        float d = CalculateurDeDistance.GetComponent<CalculDifference>().calculDistance();
        bool diminution = d < distanceInitiale;

        Text[] texts = GameObject.FindGameObjectWithTag("Canvas").GetComponentsInChildren<Text>();
        Text recompense = null;
        foreach(Text text in texts)
        {
            if(text.name == "Distance")
            {
                text.text = "Distance = " + d.ToString();
            }
            if(text.name == "Recompense")
            {
                recompense = text;
                break;
            }
            if(text.name == "Aligne")
            {
                text.text = "Aligne = " + b_aligneAvecLigneDevant.ToString();
                text.color = Color.white;
            }
            if(text.name == "Distance")
            {
                text.text = "Distance = " + d.ToString();
            }
        }

        /*if(b_surLaLigne && b_aligneAvecLigneDevant && action != k_MoveForward) {
            AddReward(-10.0f);
        }

        if (b_surLaLigne || b_aligneAvecLigneDevant || diminution) {
            if(b_surLaLigne) {
                AddReward(1.0f);
            }
            if(b_aligneAvecLigneDevant) {
                AddReward(1.0f);
            }
            if(diminution) {
                AddReward(1.0f);
            }
        } else {
            AddReward(-1.0f);
        }*/

        if (b_surLaLigne && b_aligneAvecLigneDevant && action != k_MoveForward) {
            AddReward(-10.0f);
        } 
    
        else if(!b_aligneAvecLigneDevant && action == k_MoveForward) {
            AddReward(-10.0f);
        }

        else if (b_surLaLigne) {
            AddReward(1.0f);
        } 

        recompense.color = Color.green;
        recompense.text = "Recompense = " + (getM_Reward()).ToString();

        /*if(b_surLaLigne && b_aligneAvecLigneDevant) {
            AddReward(1.0f);
            recompense.color = Color.green;
            recompense.text = "Recompense = " + (1.0f).ToString();
        } else if(diminution) {
            AddReward(0.5f);
            recompense.color = Color.yellow;
            recompense.text = "Recompense = " + (0.5f).ToString();
        } else {
            AddReward(-1.0f);
            recompense.color = Color.red;
            recompense.text = "Recompense = " + (-1.0f).ToString();
        }*/

        // bool b_bonneLigne = bonneLigne();
        // AddReward(-0.5f);
        // print(distanceALaLigne);

        // // // // Deplacement
        // // // if(actionDeplacement == k_MoveForward) {
        // // //     if (b_aligneAvecLigneDevant && d <= 0.05f) {
        // // //         AddReward(1.0f);
        // // //     }
        // // //     // } else if (b_aligneAvecLigneDevant && d <= 0.05f) {
        // // //     //     AddReward(1.0f);
        // // //     // } else if (!b_aligneAvecLigneDevant && d > 0.05f) {
        // // //     //     if(diminution) {
        // // //     //         AddReward(1.0f);
        // // //     //     } else {
        // // //     //         AddReward(-1.0f);
        // // //     //     }
        // // //     // } else if (!b_aligneAvecLigneDevant && d <= 0.05f) {
        // // //     //     AddReward(-1.0f);
        // // //     // }
        // // // } else if(actionDeplacement == k_NoAction) {
        // // //     if(b_aligneAvecLigneDevant && d <= 0.05f) {
        // // //         AddReward(-1.0f);
        // // //     }
        // // // }

        // // // // Rotation
        // // // if(actionRotation == k_RotateRight || actionRotation == k_RotateLeft) {
        // // //     if(b_aligneAvecLigneDevant) {
        // // //         AddReward(1.0f);
        // // //     }
        // // // }
        // // // else if (actionRotation == k_NoAction) {
        // // //     if(b_aligneAvecLigneDevant) {
        // // //         AddReward(-1.0f);
        // // //     }
        // // // }

        
        // // if(action == k_MoveForward || action == k_MoveRight || action == k_MoveLeft) {
        // //     if (b_aligneAvecLigneDevant && d <= 0.05f) {
        // //         AddReward(1.0f);
        // //     } else if (b_aligneAvecLigneDevant && d > 0.05f) {
        // //         if(diminution) {
        // //             AddReward(1.0f);
        // //         } else {
        // //             AddReward(-1.0f);
        // //         }
        // //     } else if (!b_aligneAvecLigneDevant && d <= 0.05f) {
        // //         if(diminution) {
        // //             AddReward(1.0f);
        // //         } else {
        // //             AddReward(-1.0f);
        // //         }
        // //     } else if (!b_aligneAvecLigneDevant && d > 0.05f) {
        // //         if(diminution) {
        // //             AddReward(1.0f);
        // //         } else {
        // //             AddReward(-1.0f);
        // //         }
        // //     }
        // // } else if (action == k_RotateLeft || action == k_RotateRight) {
        // //     if (d <= 0.05f) {
        // //         if(b_aligneAvecLigneDevant) {
        // //             AddReward(0.5f);
        // //         } else {
        // //             AddReward(-2.0f);
        // //         }
        // //     } else {
        // //         // On ne sait pas
        // //     }
        // // } 

        // if(d <= 0.10f && b_aligneAvecLigneDevant /*&& b_bonneLigne*/) {
        //     AddReward(1.0f);
        //     recompense.color = Color.green;
        //     recompense.text = "Recompense = " + (1.0f).ToString();
        // } else if(diminution || b_aligneAvecLigneDevant) {
        //     AddReward(0.5f);
        //     recompense.color = Color.yellow;
        //     recompense.text = "Recompense = " + (0.5f).ToString();
        // /*} else if(!dinfOudinf && b_aligneAvecLigneDevant) {
        //     AddReward(0.5f);
        //     recompense.color = Color.yellow;
        //     recompense.text = "Recompense = " + (0.5f).ToString();
        // */} else {
        //     AddReward(-1.0f);
        //     recompense.color = Color.red;
        //     recompense.text = "Recompense = " + (-1.0f).ToString();
        // }

        // Maj distance 
        distanceInitiale = d;
    }

    public override void OnEpisodeBegin() {
        transform.position = positionInitiale;
        transform.rotation = rotationInitiale;
        numLigneAPasser = 1;

        // Remets les pdr à false
        foreach(PointDeRelais p in pointDeRelais)
        {
            p.DejaPris = false;
        }

        distanceInitiale = CalculateurDeDistance.GetComponent<CalculDifference>().calculDistance();
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { k_MoveForward };
        }
        //if (Input.GetKey(KeyCode.S))
        //{
        //    return new float[] { k_MoveBackward };
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    return new float[] { k_MoveLeft };
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    return new float[] { k_MoveRight };
        //}
        if (Input.GetKey(KeyCode.Q))
        {
            return new float[] { k_RotateLeft };
        }
        if (Input.GetKey(KeyCode.E))
        {
            return new float[] { k_RotateRight };
        }
        return new float[] { k_NoAction };
        // return new float[] { k_MoveForward };
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == Objectif) {
            //AddReward(1000.0f);
            EndEpisode();
        }
    }

    bool SurLaLigne() {
        bool res = false;
        RaycastHit hitDessous;
        if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up), out hitDessous, Mathf.Infinity)) { 
            Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up) * hitDessous.distance, Color.yellow);
        }
        if(hitDessous.collider != null) {
            if(hitDessous.collider.gameObject != null) {
                if(hitDessous.collider.gameObject.tag == "Segment") {
                    res = true;
                }
            }
        }
        return res;
    }

    bool aligneAvecLigneDevant() {
        bool res = false;
        // RaycastHit hitDroite;
        // if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up + Vector3.right), out hitDroite, Mathf.Infinity)) { 
        //     Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up + Vector3.right) * hitDroite.distance, Color.yellow);
        // }
        // RaycastHit hitGauche;
        // if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up - Vector3.right), out hitGauche, Mathf.Infinity)) { 
        //     Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up - Vector3.right) * hitGauche.distance, Color.yellow);
        // }
        // RaycastHit hitDevant;
        // if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up + Vector3.forward), out hitDevant, Mathf.Infinity)) { 
        //     Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up + Vector3.forward) * hitDevant.distance, Color.yellow);
        // }
        // RaycastHit hitDerriere;
        // if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up - Vector3.forward), out hitDerriere, Mathf.Infinity)) { 
        //     Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up - Vector3.forward) * hitDerriere.distance, Color.yellow);
        // }
        RaycastHit hitDessous;
        if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up), out hitDessous, Mathf.Infinity)) { 
            Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up) * hitDessous.distance, Color.yellow);
        }

        Vector3 fwd = transform.forward;

        if(hitDessous.collider != null) {
            if(hitDessous.collider.gameObject != null) {
                if(hitDessous.collider.gameObject.tag == "Segment") {
                    SensDeParcours sdp = hitDessous.collider.gameObject.GetComponent<SensDeParcours>(); 
                    if (sdp != null) {
                        Vector3 sensReel = sdp.Direction; 
                        if(Vector3.Angle(fwd, sensReel) < 1.0f) {
                            res = true;
                        }
                    }
                }
            }
        }

        return res;
    }

    /*bool bonneLigne() {
        bool res = false;
        RaycastHit hitDessous;
        if (Physics.Raycast(transform.position, transform.TransformDirection(- Vector3.up), out hitDessous, Mathf.Infinity)) { 
            Debug.DrawRay(transform.position, transform.TransformDirection(- Vector3.up) * hitDessous.distance, Color.yellow);
        }

        if(hitDessous.collider != null) {
            if(hitDessous.collider.gameObject != null) {
                if(hitDessous.collider.gameObject.tag == "Segment") {
                    SensDeParcours sdp = hitDessous.collider.gameObject.GetComponent<SensDeParcours>(); 
                    if (sdp != null) {
                        int numLigne = sdp.NumLigne; 
                        if(numLigne == numLigneAPasser) {
                            res = true;
                        }
                    }
                }
            }
        }
        return res;
    }*/
}
