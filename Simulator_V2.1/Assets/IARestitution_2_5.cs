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

public class IARestitution_2_5 : Agent
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
    const int k_SwitchState = 1;

    // Etat du rendu 
    float[] renduCourant = new float[270];

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

    public void OnOneActionReceived(float action, int index)
    {
        if (action == k_NoAction)
        {
        }
        else if (action == k_SwitchState)
        {
            // allume le bon phosphene
            int indiceLigne = index / Mathf.FloorToInt(renderer.material.GetFloat("_NumberOfElectrodesW"));
            int indiceColonne = index % Mathf.FloorToInt(renderer.material.GetFloat("_NumberOfElectrodesW"));
            string s = "_k" + indiceLigne.ToString() + "_" + indiceColonne.ToString();
            if (index == 0 || index == 1)
            {
                Debug.Log("Switch : " + index.ToString() + " from " + (1-renduCourant[index]).ToString() + " to " + renduCourant[index].ToString());
                print(s);
            }
            renderer.material.SetFloat(s, renduCourant[index]);
        }
        else
        {
            throw new ArgumentException("Invalid action value");
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        int index = 0;
        for (int i = 0; i < vectorAction.Length; ++i)
        {
            var action = Mathf.FloorToInt(vectorAction[i]);
            OnOneActionReceived(action, index);
            index += 1;
        }

        // Calcule la récompense
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
        float[] res = new float[270];
        if (Input.GetKeyDown(KeyCode.W))
        {
            res[0] = 1;
            renduCourant[0] = (renduCourant[0] == 0) ? 1 : 0;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            res[1] = 1;
            renduCourant[1] = (renduCourant[1] == 0) ? 1 : 0;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            res[2] = 1;
            renduCourant[2] = (renduCourant[2] == 0) ? 1 : 0;
        }
        return res;
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

