using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class AgentDeplacementVincent : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    // Action
    const int k_NoAction = 0;  
    const int k_MoveFront = 1;
    const int k_MoveBack = 2;
    const int k_MoveRight = 3;
    const int k_MoveLeft = 4;
    const int k_ChnageRender = 5;

    public GameObject goal;

    private float cumulReward = 0.0f;

    private bool lightOn = true;

    void Start() {
        newGame();
    }

    void deplaceNimporteComment() {
        var rand = new System.Random();
        int choix = rand.Next(4);
        if(choix == 0) {
            transform.position += new Vector3(1.0f,0.0f,0.0f);
        }
        if(choix == 1) {
            transform.position += new Vector3(-1.0f,0.0f,0.0f);
        }
        if(choix == 2) {
            transform.position += new Vector3(0.0f,0.0f,1.0f);
        }
        if(choix == 3) {
            transform.position += new Vector3(0.0f,0.0f,-1.0f);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Monitor.Log("Deborde",0.0f,null);
        Monitor.Log("Gagne",0.0f,null);
        var action = Mathf.FloorToInt(vectorAction[0]);

        if(!lightOn) {
            deplaceNimporteComment();
        }

        switch(action) {
            case k_NoAction:
                break;
            case k_MoveRight:
                if(lightOn) {
                    transform.position += new Vector3(1.0f,0.0f,0.0f);
                }
                break;
            case k_MoveLeft:
                if(lightOn) {
                    transform.position += new Vector3(-1.0f,0.0f,0.0f);
                }
                break;
            case k_MoveFront:
                if(lightOn) {
                    transform.position += new Vector3(0.0f,0.0f,1.0f);
                }
                break;
            case k_MoveBack:
                if(lightOn) {
                    transform.position += new Vector3(0.0f,0.0f,-1.0f);
                }
                break;
            case k_ChnageRender:
                break;
        }

        // Si déborde r = -1
        if (transform.position.x < -4.5f || transform.position.x > 4.5f || transform.position.z < -4.5f || transform.position.z > 4.5f) {
            AddReward(-1.0f);
            Monitor.Log("Deborde",1.0f,null);
            EndEpisode();
        } else if (transform.position.x == goal.transform.position.x && transform.position.z == goal.transform.position.z) {
            AddReward(1.0f);
            Monitor.Log("Gagne",1.0f,null);
            EndEpisode();
        }

        Monitor.Log("CumulReward", cumulReward, null);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(goal.transform.position);
        sensor.AddObservation(lightOn);
    }

    public override void OnEpisodeBegin()
    {   
        cumulReward = 0.0f;
        Monitor.Log("CumulReward", cumulReward, null);
        newGame();
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

    public void updateLightOn(bool b) {
        lightOn = b;
    }
}
