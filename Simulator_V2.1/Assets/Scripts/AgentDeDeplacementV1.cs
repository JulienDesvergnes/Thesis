using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class AgentDeDeplacementV1 : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    // Les deux types de rendus
    public Shader shaderAllBlack;
    public Shader shaderWireFrame;

    // Action
    const int k_NoAction = 0;
    const int k_Avance = 1;
    const int k_Recule = 2;
    const int k_Droite = 3;
    const int k_Gauche = 4;

    // const int k_RegardeADroite = 5;
    // const int k_RegardeAGauche = 6;
    // const int k_ChangeRestitution = 7;

    // Ref vers le joueur
    public GameObject player;
    // Ref vers le RewardManager
    public RewardManager rewardManager;

    // La surface de rendu
    public GameObject RenderPlane;
    private Renderer renderer;

    // Compte le nombre de collision avec les murs
    private int nbMursTouches = 0;
    private const int nbMaxmurs = 5;

    public float rewardTouchWall = -0.2f;
    public float rewardTouchGoal = 1.0f;
    public float rewardTouchNothing = 0.01f;

    void Start() {
        renderer = RenderPlane.GetComponent<Renderer>();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var action = Mathf.FloorToInt(vectorAction[0]);
        var speed = player.GetComponent<MovePlayer>().movementSpeed;
        print(action);
        switch (action)
        {
            case k_NoAction:
                break;
            case k_Avance:
                player.transform.position = player.transform.position + (player.transform.forward * speed * Time.fixedDeltaTime);
                break;
            case k_Recule:
                player.transform.position = player.transform.position + (-player.transform.forward * speed * Time.fixedDeltaTime);
                break;
            case k_Droite:
                player.transform.position = player.transform.position + (player.transform.right * speed * Time.fixedDeltaTime);
                break;
            case k_Gauche:
                player.transform.position = player.transform.position + (-player.transform.right * speed * Time.fixedDeltaTime);
                break;
            /*case k_RegardeADroite:
                player.transform.Rotate(0.0f,18.0f,0.0f,Space.Self);                
                break;
            case k_RegardeAGauche:
                player.transform.Rotate(0.0f,-18.0f,0.0f,Space.Self);
                break;
            case k_ChangeRestitution:
                if(renderer.material.shader.name == "Custom/AllBlackShader") {
                    renderer.material.shader = shaderWireFrame;
                } else if (renderer.material.shader.name == "Custom/TestShaderFromScript") {
                    renderer.material.shader = shaderAllBlack;
                } else {
                    throw new ArgumentException("PB CAST AgentDeDeplacement");
                }                
                break;*/
            default:
                throw new ArgumentException("Invalid action value");
        }

        if (player.GetComponent<MovePlayer>().murTouche)
        {
            nbMursTouches += 1;
            rewardManager.addRewardFromModelLocal(rewardTouchWall);
            player.GetComponent<MovePlayer>().murTouche = false;
        }
        else if (player.GetComponent<MovePlayer>().cibleTouchee)
        {
            rewardManager.addRewardFromModelGlobal(rewardTouchGoal);
        }

        SetReward(rewardManager.getTotalReward());
        printAllInMonitor();

        rewardManager.resetOnlyReward();

        if(nbMursTouches >= nbMaxmurs || player.GetComponent<MovePlayer>().cibleTouchee) {
            if(nbMursTouches >= nbMaxmurs) {
                nbMursTouches = 0;
            }
            EndEpisode();
        }
        
    }

    public override void OnEpisodeBegin()
    {   
        rewardManager.reset();
        player.transform.position = new Vector3(0.0f,1.0f,1.8f);

        player.GetComponent<MovePlayer>().murTouche = false;
        player.GetComponent<MovePlayer>().cibleTouchee = false;
        player.GetComponent<MovePlayer>().cibleVue = false;

    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { k_Avance };
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] { k_Gauche };
        }
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { k_Droite };
        }
        //if (Input.GetKey(KeyCode.S))
        //{
        //    return new float[] { k_Recule };
        //}
        /*if (Input.GetKey(KeyCode.E))
        {
            return new float[] { k_RegardeADroite };
        }
        if (Input.GetKey(KeyCode.Q))
        {
            return new float[] { k_RegardeAGauche };
        }
        if (Input.GetKey(KeyCode.C))
        {
            return new float[] { k_ChangeRestitution };
        }*/
        return new float[] { k_NoAction };
    }

    public void FixedUpdate()
    {
        printAllInMonitor();
        WaitTimeInference();
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

    public void Awake()
    {
        Monitor.SetActive(true);
    }

    public void printAllInMonitor(){
        Monitor.Log("rewardFromModelGlobal", rewardManager.rewardFromModelGlobal, null);
        Monitor.Log("rewardFromModelLocal", rewardManager.rewardFromModelLocal, null);
        Monitor.Log("rewardFromObs", rewardManager.rewardFromObs, null);
        Monitor.Log("rewardFromSubject", rewardManager.rewardFromSubject, null);
        Monitor.Log("CumulativeReward", rewardManager.CumulativeReward, null);
    }
}
