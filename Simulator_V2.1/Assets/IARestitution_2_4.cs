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

public class IARestitution_2_4 : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    private float m_TimeSinceDecision;

    // Le WorldBuilder pour les OnEpisodeBegin
    public WorldBuilderScript worldBuilder;

    // Les deux types de rendus
    public Shader ShaderAllBlack;
    public Shader ShaderWireframe;

    // La surface de rendu
    public GameObject RenderPlane;
    private Renderer renderer;

    // Objectif
    // public GameObject Objectif;

    // Actions
    const int k_NoAction = 0;
    const int k_ActivateAllBlackRender = 1;
    const int k_ActivateWireframeRender = 2;

    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    void Start()
    {
        renderer = RenderPlane.GetComponent<Renderer>();
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
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
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    public override void OnActionReceived(float[] vectorAction)
    {
        var action = Mathf.FloorToInt(vectorAction[0]);
        if (action == k_NoAction)
        {
            AddReward(0.0f);
        }
        else if (action == k_ActivateAllBlackRender)
        {
            renderer.material.shader = ShaderAllBlack;
            AddReward(-1.0f);
        }
        else if (action == k_ActivateWireframeRender)
        {
            renderer.material.shader = ShaderWireframe;
            AddReward(1.0f);
        }
        else
        {
            throw new ArgumentException("Invalid action value");
        }
    }

    public override void OnEpisodeBegin()
    {
        Destroy(worldBuilder.Ligne);
        worldBuilder.build_trajectoire();
        worldBuilder.DestroyAndResetPreviousWorld();
        transform.position = new Vector3(0.0f, 0.0f, 0.0f); 
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { k_ActivateWireframeRender };
        }
        else if (Input.GetKey(KeyCode.B))
        {
            return new float[] { k_ActivateAllBlackRender };
        }
        else
        {
            return new float[] { k_NoAction };
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            Destroy(worldBuilder.Ligne);
            EndEpisode();
        }
    }
}

