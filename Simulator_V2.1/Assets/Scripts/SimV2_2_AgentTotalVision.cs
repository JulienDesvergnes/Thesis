using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class SimV2_2_AgentTotalVision : Agent
{

    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    // Vitesse de deplacement
    public float speed = 6.0f;

    // Les deux types de rendus
    public Shader shaderAllBlack;
    public Shader shaderWireFrame;

    // La surface de rendu
    public GameObject RenderPlane;
    private Renderer renderer;

    private bool lightOn = true;

    private Vector3 positionInitiale;
    private Quaternion rotationInitiale;

    // Action
    const int k_NoAction = 0;  
    const int k_ChangeRender = 1;
    const int k_MoveFront = 2;
    const int k_MoveBack = 3;
    const int k_MoveRight = 4;
    const int k_MoveLeft = 5;
    const int k_RotateRight = 6;
    const int k_RotateLeft = 7;

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

    /*public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(lightOn);
    }*/

    public override void OnActionReceived(float[] vectorAction) {
        var action = Mathf.FloorToInt(vectorAction[0]);
        AddReward(-1.0f/maxStep);
        if(!lightOn) {
            // On ne peut que allumer la lumière
            if(action == k_ChangeRender) {
                switchLightOn();
            } else {
            }
        } else {
            switch (action)
            {
                case k_NoAction:
                    break;
                case k_ChangeRender:
                    switchLightOff();
                    break;
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
        switchLightOn();
        transform.position = positionInitiale;
        transform.rotation = rotationInitiale;
    }

    void switchLightOff() {
        renderer.material.shader = shaderAllBlack;
        lightOn = false;
    }

    void switchLightOn() {
        renderer.material.shader = shaderWireFrame;
        lightOn = true;
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
        if (Input.GetKey(KeyCode.C))
        {
            return new float[] { k_ChangeRender };
        }
        return new float[] { k_NoAction };
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall") {
            AddReward(-2.0f/maxStep);
        } else if (collision.gameObject.tag == "goal") {
            AddReward(1.0f);
            EndEpisode();
        }
    }

}
