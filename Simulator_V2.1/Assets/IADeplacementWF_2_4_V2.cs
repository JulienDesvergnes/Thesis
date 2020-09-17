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

public class IADeplacementWF_2_4_V2 : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    private float m_TimeSinceDecision;

    // Le WorldBuilder pour les OnEpisodeBegin
    public WorldBuilderScript worldBuilder;

    // Objectif
    // public GameObject Objectif;

    // Actions
    const int k_Avance = 0;
    const int k_TourneDroite = 1;
    const int k_TourneGauche = 2;
    // const int k_TourneGauche = 3;

    private String fromIntToAction(int i)
    {
        if (i == 0) { return "NoAction"; }
        if (i == 1) { return "Avance"; }
        if (i == 2) { return "TourneDroite"; }
        if (i == 3) { return "TourneGauche"; }
        return "Error : fromIntToAction input not valid!";
    }

    // Vitesse de deplacement
    public float movementSpeed = 5.0f;

    // Texte du UI
    private Text t_ActionChoisie;
    private Text t_Recompense;

    // Conservateur de choix
    private float Recompense;
    private int ActionChoisie;

    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    void Start()
    {
        Text[] texts = GameObject.FindGameObjectWithTag("Canvas").GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            if (text.name == "ActionChoisie")
            {
                t_ActionChoisie = text;
            }
            else if (text.name == "Recompense")
            {
                t_Recompense = text;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
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
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    RaycastHit LanceRayon(Vector3 position, Vector3 direction, Color c, bool draw = false)
    {
        RaycastHit r;
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        Physics.Raycast(position, transform.TransformDirection(direction), out r, Mathf.Infinity, layerMask);
        if (draw)
        {
            Debug.DrawRay(position, transform.TransformDirection(direction) * r.distance, c);
        }
        return r;
    }

    /*bool EstSurLaLigne()
    {
        RaycastHit RaycastEnDessous = LanceRayon(transform.position, -Vector3.up, Color.green, false);
        if (RaycastEnDessous.collider == null)
        {
            return false;
        }
        if (RaycastEnDessous.collider.tag == "Segment")
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/

    /*public bool avanceDansLaBonneDirection()
    {
        RaycastHit RaycastEnDessous = LanceRayon(transform.position, -Vector3.up, Color.green, false);
        if (RaycastEnDessous.collider == null)
        {
            return false;
        }
        if (RaycastEnDessous.collider.tag == "Segment")
        {
            if (Vector3.Normalize(RaycastEnDessous.collider.gameObject.GetComponent<SensDeParcours>().Direction) == Vector3.Normalize(transform.forward))
            {
                // print("good");
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }*/

    public override void OnActionReceived(float[] vectorAction)
    {
        var action = Mathf.FloorToInt(vectorAction[0]);

        if (action == k_Avance)
        {
            transform.position = transform.position + (-transform.right * movementSpeed * Time.fixedDeltaTime);
        }
        else if (action == k_TourneDroite)
        {
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
        else if (action == k_TourneGauche)
        {
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
        else
        {
            throw new ArgumentException("Invalid action value");
        }

        AddReward(0.0f);
        Recompense += 0.0f;

        // MAJ UI
        if (Recompense > 0.0f)
        {
            t_Recompense.text = "Recompense = " + Recompense.ToString();
            t_Recompense.color = Color.green;
        }
        else if (Recompense == 0.0f)
        {
            t_Recompense.text = "Recompense = " + Recompense.ToString();
            t_Recompense.color = Color.yellow;
        }
        else
        {
            t_Recompense.text = "Recompense = " + Recompense.ToString();
            t_Recompense.color = Color.red;
        }
        t_ActionChoisie.text = "ActionChoisie = " + fromIntToAction(ActionChoisie);

    }

    public override void OnEpisodeBegin()
    {
        Destroy(worldBuilder.Ligne);
        worldBuilder.build_trajectoire();
        worldBuilder.DestroyAndResetPreviousWorld();
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            return new float[] { k_Avance };
        }
        else if (Input.GetKey(KeyCode.A))
        {
            return new float[] { k_TourneGauche };
        }
        else if (Input.GetKey(KeyCode.E))
        {
            return new float[] { k_TourneDroite };
        }
        else
        {
            return new float[] { k_Avance };
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            AddReward(1.0f);
            Recompense += 1.0f;
            Destroy(worldBuilder.Ligne);
            Destroy(worldBuilder.Mur);
            EndEpisode();
        } else if (collision.gameObject.tag == "wall")
        {
            AddReward(-1.0f);
            Recompense += -1.0f;
            Destroy(worldBuilder.Ligne);
            Destroy(worldBuilder.Mur);
            EndEpisode();
        }
    }
}


