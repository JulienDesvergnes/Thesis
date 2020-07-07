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

public class AgentDeRestitution23 : Agent
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

    // Action
    const int k_NoAction = 0;  
    const int k_ChangeRender = 1;

    // CalculateurDeDistance
    public GameObject CalculateurDeDistance;

    public GameObject Lumieres;

    // PointDeRelais
    public PointDeRelais[] pointDeRelais;

    public void FixedUpdate()
    {
        WaitTimeInference();
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
        if(action == k_ChangeRender) {
            if (renduCourant == 0) {
                //AddReward(1.0f/maxStep);
                switchLightOn();
            } else {
                //AddReward(-1.0f/maxStep);
                switchLightOff();
            }
        }
        else if (action == k_NoAction) {
            if (renduCourant == 0) {
                //AddReward(-1.0f/maxStep);
            } else {
                //AddReward(1.0f/maxStep);
            }
        } else {
            throw new ArgumentException("Invalid action value");     
        }

        // print(getM_Reward());
        // print(getM_CReward());
        float distanceALaLigne = CalculateurDeDistance.GetComponent<CalculDifference>().DISTANCESTOCKEE;
        Text[] texts = GameObject.FindGameObjectWithTag("Canvas").GetComponentsInChildren<Text>();
        Text recompense = null;
        foreach(Text text in texts)
        {
            if(text.name == "Recompense")
            {
                recompense = text;
                break;
            }
            /*if(text.name == "Distance")
            {
                text.text = "Distance = " + distanceALaLigne;
            }*/
        }
        // print(distanceALaLigne);
        if(distanceALaLigne <= 0.05f) {
            AddReward(1.0f);
            recompense.color = Color.green;
            recompense.text = "Recompense = " + (1.0f).ToString();
        } else {
            AddReward(-1.0f);
            recompense.color = Color.red;
            recompense.text = "Recompense = " + (-1.0f).ToString();
        }
    }

    public override void OnEpisodeBegin() {
        switchLightOn();
        transform.position = positionInitiale;
        transform.rotation = rotationInitiale;
        GetComponent<DeplacementAutonome>().DirectionEnCasDeConfusion = GetComponent<DeplacementAutonome>().generateRandomDir();

        // Remets les pdr à false
        foreach(PointDeRelais p in pointDeRelais)
        {
            p.DejaPris = false;
        }
    }

    void switchLightOff() {
        GetComponent<DeplacementAutonome>().seeSomething = false;
        renduCourant = 0;
        renderer.material.shader = shaderAllBlack;

        // Eteins vraiment les lumières
        int nombreDeLumieres = Lumieres.transform.childCount;
        for(int i = 0; i < nombreDeLumieres; ++i) {
            GameObject lum = Lumieres.transform.GetChild(i).gameObject;
            lum.GetComponent<Light>().color = Color.black;
        }

        // Remets les pdr à false
        foreach(PointDeRelais p in pointDeRelais)
        {
            p.DejaPris = false;
        }
        
    }

    void switchLightOn() {
        GetComponent<DeplacementAutonome>().seeSomething = true;
        renduCourant = 1;
        renderer.material.shader = shaderWireFrame;

        // Allume vraiment les lumières
        int nombreDeLumieres = Lumieres.transform.childCount;
        for(int i = 0; i < nombreDeLumieres; ++i) {
            GameObject lum = Lumieres.transform.GetChild(i).gameObject;
            lum.GetComponent<Light>().color = Color.white;
        }
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.C))
        {
            return new float[] { k_ChangeRender };
        }
        return new float[] { k_NoAction };
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == Objectif) {
            AddReward(1000.0f);
            EndEpisode();
        }
    }
}
