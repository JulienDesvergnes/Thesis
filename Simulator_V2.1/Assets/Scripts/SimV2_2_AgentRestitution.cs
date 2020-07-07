using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class SimV2_2_AgentRestitution : Agent
{

    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    // Les deux types de rendus
    public Shader shaderAllBlack;
    public Shader shaderWireFrame;

    // La surface de rendu
    public GameObject RenderPlane;
    private Renderer renderer;

    public bool lightOn = true;

    // Action
    const int k_NoAction = 0;  
    const int k_ChangeRender = 1;

    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    void Start() {
        renderer = RenderPlane.GetComponent<Renderer>();
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

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(lightOn);
    }

    public override void OnActionReceived(float[] vectorAction) {
        var action = Mathf.FloorToInt(vectorAction[0]);
        if(!lightOn) {
            switch(action) {
                case k_NoAction:
                    AddReward(-1.0f/maxStep);
                    break;
                case k_ChangeRender:
                    AddReward(1.0f/maxStep);
                    switchLightOn();
                    break;
                default :
                    throw new ArgumentException("Invalid action value");
            }
        } else {
            switch(action) {
                case k_NoAction:
                    AddReward(1.0f/maxStep);
                    break;
                case k_ChangeRender:
                    AddReward(-1.0f/maxStep);
                    switchLightOff();
                    break;
                default :
                    throw new ArgumentException("Invalid action value");
            }
        }
    }

    public override void OnEpisodeBegin() {
        switchLightOn();
    }

    public void switchLightOff() {
        renderer.material.shader = shaderAllBlack;
        lightOn = false;
    }

    public void switchLightOn() {
        renderer.material.shader = shaderWireFrame;
        lightOn = true;
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.J))
        {
            return new float[] { k_ChangeRender };
        }
        return new float[] { k_NoAction };
    }
}

