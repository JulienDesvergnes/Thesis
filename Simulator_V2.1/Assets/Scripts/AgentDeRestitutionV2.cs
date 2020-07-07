using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class AgentDeRestitutionV2 : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    // Action
    const int k_NoAction = 0;  
    const int k_blackRender = 1;
    const int k_wireFrameRender = 2;

    // Ref vers le joueur
    public GameObject player;
    // Ref vers le RewardManager
    public RewardManager rewardManager;

    // Les deux types de rendus
    public Shader shaderAllBlack;
    public Shader shaderWireFrame;

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

        // Monitor.Log("Action",0.5f*action,null);

        switch (action)
        {
            case k_NoAction:
                break;
            case k_blackRender:
                renderer.material.shader = shaderAllBlack;
                player.GetComponent<MovePlayer>().cibleVue = false;
                // rewardManager.addRewardFromModelLocal(-0.01f);
                break;
            case k_wireFrameRender:
                renderer.material.shader = shaderWireFrame;
                // rewardManager.addRewardFromModelLocal(0.01f);
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }

        if (player.GetComponent<MovePlayer>().murTouche)
        {
            nbMursTouches += 1;
            // rewardManager.addRewardFromModelLocal(rewardTouchWall);
        }
        else if (player.GetComponent<MovePlayer>().cibleTouchee)
        {
            // rewardManager.addRewardFromModelGlobal(rewardTouchGoal);
        }

        // SetReward(rewardManager.getTotalReward());
        //printAllInMonitor();

        // rewardManager.resetOnlyReward();

        if(/*nbMursTouches >= nbMaxmurs ||*/ player.GetComponent<MovePlayer>().cibleTouchee) {
            //if(nbMursTouches >= nbMaxmurs) {
            //    nbMursTouches = 0;
            //}
            EndEpisode();
        }
        
    }

    public override void OnEpisodeBegin()
    {   
        rewardManager.reset();
        renderer.material.shader = shaderAllBlack;
        player.transform.position = new Vector3(0.0f,1.0f,1.8f);

        player.GetComponent<MovePlayer>().murTouche = false;
        player.GetComponent<MovePlayer>().cibleTouchee = false;
        player.GetComponent<MovePlayer>().cibleVue = false;

    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.B))
        {
            return new float[] { k_blackRender };
        }
        if (Input.GetKey(KeyCode.N))
        {
            return new float[] { k_wireFrameRender };
        }
        return new float[] { k_NoAction };
    }

    public void FixedUpdate()
    {
        // printAllInMonitor();
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
        // Monitor.SetActive(true);
    }

    public void printAllInMonitor(){
        Monitor.Log("rewardFromModelGlobal", rewardManager.rewardFromModelGlobal, null);
        Monitor.Log("rewardFromModelLocal", rewardManager.rewardFromModelLocal, null);
        Monitor.Log("rewardFromObs", rewardManager.rewardFromObs, null);
        Monitor.Log("rewardFromSubject", rewardManager.rewardFromSubject, null);
        Monitor.Log("CumulativeReward", rewardManager.CumulativeReward, null);
    }
}
