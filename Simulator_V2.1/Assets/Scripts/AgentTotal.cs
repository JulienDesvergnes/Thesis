using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MLAgents;
using MLAgents.Sensors;
using UnityEngine.Serialization;

public class AgentTotal : Agent
{
    // Times pour l'inference
    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    // Action
    const int k_NoAction = 0;  
    const int k_ChangeRender = 1;
    const int k_MoveFront = 2;
    const int k_MoveBack = 3;
    const int k_MoveRight = 4;
    const int k_MoveLeft = 5;
    const int k_RotateRight = 6;
    const int k_RotateLeft = 7;


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
    public float rewardMovement = 0.002f;
    public float rewardRotation = 0.001f;

    void Start() {
        renderer = RenderPlane.GetComponent<Renderer>();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var action = Mathf.FloorToInt(vectorAction[0]);
        var speed = player.GetComponent<MovePlayer>().movementSpeed;

        if(renderer.material.shader.name == "Custom/AllBlackShader") {
            if (action != k_NoAction) {
                if (action == k_ChangeRender) {
                    renderer.material.shader = shaderWireFrame;
                    // rewardManager.addRewardFromModelLocal(0.2f);
                } else {
                    // Deplacement position
                    var rand = new System.Random();
                    int direction = 1;
                    if(rand.Next(2) == 0) {
                        direction = 2;
                    }
                    int sens = 1;
                    if(rand.Next(2) == 0) {
                        sens = -1;
                    }
                    int decideur = sens * direction;
                    // decideur = -2 -1 1 ou 2
                    if (decideur == -2) {
                        // Gauche
                        player.transform.position = player.transform.position + (-player.transform.right * speed * Time.fixedDeltaTime);
                        // rewardManager.addRewardFromModelLocal(rewardMovement);
                    } else if (decideur == -1) {
                        // Droite
                        player.transform.position = player.transform.position + (player.transform.right * speed * Time.fixedDeltaTime);
                        // rewardManager.addRewardFromModelLocal(rewardMovement);
                    } else if (decideur == 1) {
                        // Haut
                        player.transform.position = player.transform.position + (player.transform.forward * speed * Time.fixedDeltaTime);
                        // rewardManager.addRewardFromModelLocal(rewardMovement);
                    } else if (decideur == 2) {
                        // Bas
                        player.transform.position = player.transform.position + (-player.transform.forward * speed * Time.fixedDeltaTime);
                        // rewardManager.addRewardFromModelLocal(rewardMovement);
                    }
                    
                    // Deplacement rotation
                    float modifieurDeRotationSurY = rand.Next(20);
                    int sensDeRotation = 1;
                    if(rand.Next(2) == 0) {
                        sensDeRotation = -1;
                    }
                    player.transform.Rotate(0.0f,sensDeRotation * modifieurDeRotationSurY,0.0f,Space.Self);
                }
            }
        } else {
            switch (action)
            {
                case k_NoAction:
                    break;
                case k_ChangeRender:
                        renderer.material.shader = shaderAllBlack;
                        player.GetComponent<MovePlayer>().cibleVue = false;
                        // rewardManager.addRewardFromModelLocal(-0.2f);
                    break;
                case k_MoveBack:
                    player.transform.position = player.transform.position + (-player.transform.forward * speed * Time.fixedDeltaTime);
                    // rewardManager.addRewardFromModelLocal(rewardMovement);
                    break;
                case k_MoveFront:
                    player.transform.position = player.transform.position + (player.transform.forward * speed * Time.fixedDeltaTime);
                    // rewardManager.addRewardFromModelLocal(rewardMovement);
                    break;
                case k_MoveRight:
                    player.transform.position = player.transform.position + (player.transform.right * speed * Time.fixedDeltaTime);
                    // rewardManager.addRewardFromModelLocal(rewardMovement);
                    break;
                case k_MoveLeft:
                    player.transform.position = player.transform.position + (-player.transform.right * speed * Time.fixedDeltaTime);
                    // rewardManager.addRewardFromModelLocal(rewardMovement);
                    break;
                case k_RotateRight:
                    player.transform.Rotate(0.0f,10.0f,0.0f,Space.Self);
                    // rewardManager.addRewardFromModelLocal(rewardRotation);             
                    break;
                case k_RotateLeft:
                    player.transform.Rotate(0.0f,-10.0f,0.0f,Space.Self);
                    // rewardManager.addRewardFromModelLocal(rewardRotation);             
                    break;
                default:
                    throw new ArgumentException("Invalid action value");
            }
        }

        if (player.GetComponent<MovePlayer>().murTouche)
        {
            nbMursTouches += 1;
            rewardManager.addRewardFromModelLocal(rewardTouchWall);
            player.GetComponent<MovePlayer>().murTouche = false;
        }
        else if (player.GetComponent<MovePlayer>().cibleTouchee)
        {
            rewardManager.addRewardFromModelGlobal(rewardTouchGoal);
        }

        SetReward(rewardManager.getTotalReward());
        printAllInMonitor();

        //rewardManager.resetOnlyReward();

        if(nbMursTouches >= nbMaxmurs || player.GetComponent<MovePlayer>().cibleTouchee) {
            if(nbMursTouches >= nbMaxmurs) {
                nbMursTouches = 0;
            }
            EndEpisode();
        }
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(player.transform.rotation);
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

    public void FixedUpdate()
    {
        printAllInMonitor();
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

    public void printAllInMonitor(){
        Monitor.Log("rewardFromModelGlobal", rewardManager.rewardFromModelGlobal, null);
        Monitor.Log("rewardFromModelLocal", rewardManager.rewardFromModelLocal, null);
        Monitor.Log("rewardFromObs", rewardManager.rewardFromObs, null);
        Monitor.Log("rewardFromSubject", rewardManager.rewardFromSubject, null);
        Monitor.Log("CumulativeReward", rewardManager.CumulativeReward, null);
    }
}
