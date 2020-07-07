using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class SimV2_2_AgentDeplacement : Agent
{

    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    private Vector3 positionInitiale;
    private Quaternion rotationInitiale;

    // Vitesse de deplacement
    public float speed = 6.0f;

    // Action
    const int k_NoAction = 0;  
    const int k_MoveFront = 1;
    const int k_MoveBack = 2;
    const int k_MoveRight = 3;
    const int k_MoveLeft = 4;
    const int k_RotateRight = 5;
    const int k_RotateLeft = 6;
    // const int k_ChangeRender = 7;

    // Agent de restitution
    public GameObject agentRestitution;

    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    void Start() {
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

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(agentRestitution.GetComponent<SimV2_2_AgentRestitution>().lightOn);
    }

    public override void OnActionReceived(float[] vectorAction) {
        var action = Mathf.FloorToInt(vectorAction[0]);
        AddReward(-1.0f/maxStep);
        if(!(agentRestitution.GetComponent<SimV2_2_AgentRestitution>().lightOn)) {
            /*if(action == k_ChangeRender) {
                agentRestitution.GetComponent<SimV2_2_AgentRestitution>().switchLightOn();
            }*/
        } else {
            switch (action)
            {
                case k_NoAction:
                    break;
                /*case k_ChangeRender:
                    agentRestitution.GetComponent<SimV2_2_AgentRestitution>().switchLightOff();
                    break;*/
                case k_MoveBack:
                    transform.position = transform.position + (-transform.forward * speed * Time.fixedDeltaTime);
                    break;
                case k_MoveFront:
                    transform.position = transform.position + (transform.forward * speed * Time.fixedDeltaTime);
                    break;
                case k_MoveRight:
                    transform.position = transform.position + (transform.right * speed * Time.fixedDeltaTime);
                    break;
                case k_MoveLeft:
                    transform.position = transform.position + (-transform.right * speed * Time.fixedDeltaTime);
                    break;
                case k_RotateRight:
                    transform.Rotate(0.0f,10.0f,0.0f,Space.Self);
                    break;
                case k_RotateLeft:
                    transform.Rotate(0.0f,-10.0f,0.0f,Space.Self);
                    break;
                default:
                    throw new ArgumentException("Invalid action value");
            }
        }
    }

    public override void OnEpisodeBegin() {
        transform.position = positionInitiale;
        transform.rotation = rotationInitiale;
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { k_MoveFront };
        }
        if (Input.GetKey(KeyCode.S))
        {
            return new float[] { k_MoveBack };
        }
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { k_MoveRight };
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] { k_MoveLeft };
        }
        if (Input.GetKey(KeyCode.Q))
        {
            return new float[] { k_RotateLeft };
        }
        if (Input.GetKey(KeyCode.E))
        {
            return new float[] { k_RotateRight };
        }
        /*if (Input.GetKey(KeyCode.C))
        {
            return new float[] { k_ChangeRender };
        }*/
        return new float[] { k_NoAction };
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall") {
            AddReward(-2.0f/maxStep);
        } else if (collision.gameObject.tag == "goal") {
            AddReward(1.0f);
            agentRestitution.GetComponent<SimV2_2_AgentRestitution>().EndEpisode();
            EndEpisode();
        }
    }

}
