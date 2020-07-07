using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;
using System.Linq;

public class AgentDeRestitution : Agent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject RenderPlane;
    public Shader shaderAllBlack;
    public Shader shaderWireFrame;

    const int k_BlackRender = 0;
    const int k_WireFrameRender = 1;
    const int k_NoAction = 2;

    public Transform playerTransform;

    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBegin Begin");
        // On récupère le shader 
        Renderer renderer = RenderPlane.GetComponent<Renderer>();
        Shader shader = renderer.material.shader;

        // On lé réinitialise à WireFrame
        shader = shaderAllBlack;

        playerTransform.position = new Vector3(-4.0f, 1.0f, -0.3f);

        Debug.Log("OnEpisodeBegin End");

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Debug.Log("OnActionReceived");
        var action = Mathf.FloorToInt(vectorAction[0]);
        
        Renderer renderer = RenderPlane.GetComponent<Renderer>();
        Shader shader = renderer.material.shader;
        
        Debug.Log(action);

        switch (action)
        {
            case k_BlackRender:
                shader = shaderAllBlack;
                break;
            case k_WireFrameRender:
                shader = shaderWireFrame;
                break;
            case k_NoAction:
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
        var hit = Physics.OverlapSphere(playerTransform.position, 0.25f);
        int i = 0;
        while (i < hit.Length) {
            if (hit[i].tag == "wall")
            {
                Debug.Log("Touche un Mur");
                SetReward(-1f);
                EndEpisode();
            }

            else if (hit[i].tag == "goal")
            {
                Debug.Log("Touche un Goal");
                SetReward(1f);
                EndEpisode();
            }
        }
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.B))
        {
            return new float[] { k_BlackRender };
        }
        if (Input.GetKey(KeyCode.Z))
        {
            return new float[] { k_WireFrameRender };
        }
        return new float[] { k_NoAction };
    }

    public void FixedUpdate()
    {
        Debug.Log("Fixed Update Begin");
        WaitTimeInference();
        Debug.Log("Fixed Update End");
    }

    void WaitTimeInference()
    {
        Debug.Log("WaitTimeInference Begin");
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
        Debug.Log("WaitTimeInference End");
    }
}
