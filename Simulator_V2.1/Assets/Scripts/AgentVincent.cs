using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class AgentVincent : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    // Pour les logs
    public MyLogger myLogger;

    // Action
    const int k_NoAction = 0;  
    const int k_ChangeRender = 1;
    const int k_MoveFront = 2;
    const int k_MoveBack = 3;
    const int k_MoveRight = 4;
    const int k_MoveLeft = 5;

    public GameObject goal;

    public Light l1;
    public Light l2;
    public Light l3;
    public Light l4;
    public Light l5;

    private bool lightOn = true;

    void Start() {
        newGame();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var action = Mathf.FloorToInt(vectorAction[0]);
        switch(action) {
            case k_NoAction:
                myLogger.setAction("No Action");
                break;
            case k_ChangeRender:
                myLogger.setAction("Change Render");
                if(lightOn) {
                    SwitchLightOff();
                    AddReward(-0.2f);
                    myLogger.SetRewardValue(-0.2f);
                    myLogger.AddRewardCumulatedValue(-0.2f);
                } else {
                    SwitchLightOn();
                    AddReward(0.2f);
                    myLogger.SetRewardValue(+0.2f);
                    myLogger.AddRewardCumulatedValue(+0.2f);
                }
                break;
            case k_MoveLeft:
                myLogger.setAction("Move Left");
                if(lightOn) {
                    transform.position += new Vector3(-1.0f,0.0f,0.0f);
                }
                break;
            case k_MoveRight:
                myLogger.setAction("Move Right");
                if(lightOn) {
                    transform.position += new Vector3(1.0f,0.0f,0.0f);
                }
                break;
            case k_MoveFront:
                myLogger.setAction("Move Front");
                if(lightOn) {
                    transform.position += new Vector3(0.0f,0.0f,1.0f);
                }
                break;
            case k_MoveBack:
                myLogger.setAction("Move Back");
                if(lightOn) {
                    transform.position += new Vector3(0.0f,0.0f,-1.0f);
                }
                break;
        }

        // Si déborde r = -1
        if (transform.position.x < -4.5f || transform.position.x > 4.5f || transform.position.z < -4.5f || transform.position.z > 4.5f) {
            SetReward(-1.0f);
            myLogger.AddRewardCumulatedValue(-1.0f);
            EndEpisode();
        } else if (transform.position.x == goal.transform.position.x && transform.position.z == goal.transform.position.z) {
            SetReward(1.0f);
            myLogger.AddRewardCumulatedValue(1.0f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(goal.transform.position);
        sensor.AddObservation(lightOn);
    }

    public override void OnEpisodeBegin()
    {   
        newGame();
        SwitchLightOn();
        myLogger.reset();
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { k_MoveFront };
        }
        //if (Input.GetKey(KeyCode.S))
        //{
        //    return new float[] { k_MoveBack };
        //}
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { k_MoveRight };
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] { k_MoveLeft };
        }
        if (Input.GetKey(KeyCode.C)) {
            return new float[] {k_ChangeRender};
        }
        return new float[] { k_NoAction };
    }

    public void FixedUpdate()
    {
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

    public void newGame() {
        var rand = new System.Random();
        float xg = rand.Next(9) - 4.5f;
        float yg = rand.Next(9) - 4.5f;

        float x = rand.Next(9) - 4.5f;
        float y = rand.Next(9) - 4.5f;

        while(x == xg && y == yg) {
            x = rand.Next(9) - 4.5f;
            y = rand.Next(9) - 4.5f;
        }

        transform.position = new Vector3(x,0.5f,y);
        goal.transform.position = new Vector3(xg,0.5f,yg);
    }

    private void SwitchLightOff() {
        l1.color = Color.black;
        l2.color = Color.black;
        l3.color = Color.black;
        l4.color = Color.black;
        l5.color = Color.black;
        lightOn = false;
    }

    private void SwitchLightOn() {
        l1.color = Color.white;
        l2.color = Color.white;
        l3.color = Color.white;
        l4.color = Color.white;
        l5.color = Color.white;
        lightOn = true;
    }
}
