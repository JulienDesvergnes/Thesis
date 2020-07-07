using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class AgentRestitutionVincent : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    // Action
    const int k_NoAction = 0;  
    const int k_ChangeRender = 1;

    public GameObject goal;

    public Light l1;
    public Light l2;
    public Light l3;
    public Light l4;
    public Light l5;

    private bool lightOn = true;

    public AgentDeplacementVincent AgentDeDeplacement;

    void Start() {
        newGame();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Monitor.Log("Deborde",0.0f,null);
        Monitor.Log("Gagne",0.0f,null);
        var action = Mathf.FloorToInt(vectorAction[0]);
        switch(action) {
            case k_NoAction:
                break;
            case k_ChangeRender:
                if(lightOn) {
                    SwitchLightOff();
                    SetReward(-0.2f);
                } else {
                    SwitchLightOn();
                    SetReward(0.2f);
                }
                break;
        }

        EndEpisode();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(goal.transform.position);
        sensor.AddObservation(lightOn);
    }

    public override void OnEpisodeBegin()
    {   
        // newGame();
        SwitchLightOn();
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.C))
        {
            return new float[] { k_ChangeRender };
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
        AgentDeDeplacement.updateLightOn(false);
    }

    private void SwitchLightOn() {
        l1.color = Color.white;
        l2.color = Color.white;
        l3.color = Color.white;
        l4.color = Color.white;
        l5.color = Color.white;
        lightOn = true;
        AgentDeDeplacement.updateLightOn(true);
    }
}
