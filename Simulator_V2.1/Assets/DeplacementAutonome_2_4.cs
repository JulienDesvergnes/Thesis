using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Globalization;

public class DeplacementAutonome_2_4 : MonoBehaviour
{

    private Text t_EstSurLaLigne;
    private Text t_EstAligneAvecLaLigne;
    private Text t_Front;
    private Text t_Back;
    private Text t_Right;
    private Text t_Left;

    public float movementSpeed = 5.0f;

    public bool vientDeTourner = false;
    public bool doitTourner = false;
    //private StreamWriter sw;

    public bool changementdirectiondetecte = false;
    public Vector3 currentDirection = new Vector3(0, 0, 0);
    public Vector3 prochaineDirection = new Vector3(0, 0, 0);

    public int label = 0;
    public int memLabel = 0;
    public int derniereAction = 0;
    public TextureSaver ts;
    public float timer = 1.0f;
    private float T = 0.0f;

    public void resetDeplacementAutonome()
    {
        changementdirectiondetecte = false;
        currentDirection = new Vector3(0, 0, 0);
        prochaineDirection = new Vector3(0, 0, 0);
        doitTourner = false;
        vientDeTourner = false;
        // sw.Flush();
    }

    void Start()
    {
        Text[] texts = GameObject.FindGameObjectWithTag("Canvas").GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            if (text.name == "EstAligneAvecLaLigne")
            {
                t_EstAligneAvecLaLigne = text;
            }
            else if (text.name == "EstSurLaLigne")
            {
                t_EstSurLaLigne = text;
            }
            else if (text.name == "Front")
            {
                t_Front = text;
            }
            else if (text.name == "Back")
            {
                t_Back = text;
            }
            else if (text.name == "Right")
            {
                t_Right = text;
            }
            else if (text.name == "Left")
            {
                t_Left = text;
            }
        }

        //string path = @"D:/Dataset/Labels/labels.txt";
        //if (!File.Exists(path))
        //{
            // Create a file to write to.
            //sw = File.CreateText(path);  
        //}
    }

    void TestAndSet(bool val, Text text)
    {
        if(val)
        {
            text.color = Color.green;
        } else
        {
            text.color = Color.red;
        }
    }

    /*void Update()
    {
        // Draw Forward
        // LanceRayon(transform.position, transform.forward, Color.cyan, true);

        bool b_EstSurLaLigne = EstSurLaLigne();
        bool b_EstAligneAvecLaLigne = EstAligneAvecLaLigne();
        bool[] raycastsDirectionels = RaycastsDirectionelsAbsolus();
        bool b_front = raycastsDirectionels[0]; bool b_back = raycastsDirectionels[1]; bool b_right = raycastsDirectionels[2]; bool b_left = raycastsDirectionels[3];

        TestAndSet(b_EstSurLaLigne, t_EstSurLaLigne);
        TestAndSet(b_EstAligneAvecLaLigne, t_EstAligneAvecLaLigne);
        TestAndSet(b_front, t_Front);
        TestAndSet(b_back, t_Back);
        TestAndSet(b_right, t_Right);
        TestAndSet(b_left, t_Left);

        if (b_EstSurLaLigne && b_EstAligneAvecLaLigne && b_front && b_back && b_right && b_left)
        {
            transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
        }

        if(!b_back)
        {
            transform.position = transform.position + (-transform.right * movementSpeed * Time.fixedDeltaTime);
        }
        else if (!b_right)
        {
            transform.position = transform.position + (-transform.right * movementSpeed * Time.fixedDeltaTime);
        }
        else if (!b_left)
        {
            transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
        }
        else if (!b_front)
        {
            transform.position = transform.position + (-transform.forward * movementSpeed * Time.fixedDeltaTime);
        }
        else if(!b_EstAligneAvecLaLigne)
        {
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
    }*/

    void Update()
    {
        // ts.CamCapture();
        RaycastHit RaycastDessous = LanceRayon(transform.position, (-transform.up), Color.yellow, true);
        if (RaycastDessous.collider != null)
        {
            if (RaycastDessous.collider.gameObject != null)
            {
                if (RaycastDessous.collider.gameObject.GetComponent<SensDeParcours>() != null)
                {
                    if (currentDirection == new Vector3(0, 0, 0))
                    {
                        currentDirection = RaycastDessous.collider.gameObject.GetComponent<SensDeParcours>().Direction;
                    }
                }
            }
        }

        RaycastHit RaycastDiagonaleBasDroite =  LanceRayon(transform.position, 2.0f * Vector3.Normalize(-transform.up + transform.right), Color.blue, false);
        RaycastHit RaycastDiagonaleBasGauche =  LanceRayon(transform.position, 2.0f * Vector3.Normalize(-transform.up - transform.right), Color.blue, false);

        if (currentDirection == new Vector3(0, 0, 1))
        {
            RaycastDiagonaleBasDroite = LanceRayon(transform.position, -transform.up + new Vector3(1.5f, 0, 0), Color.red, true);
            RaycastDiagonaleBasGauche = LanceRayon(transform.position, -transform.up + new Vector3(-1.5f, 0, 0), Color.blue, true);
        }
        if (currentDirection == new Vector3(0, 0, -1))
        {
            RaycastDiagonaleBasDroite = LanceRayon(transform.position, -transform.up + new Vector3(-1.5f, 0, 0), Color.red, true);
            RaycastDiagonaleBasGauche = LanceRayon(transform.position, -transform.up + new Vector3(1.5f, 0, 0), Color.blue, true);
        }
        if (currentDirection == new Vector3(1, 0, 0))
        {
            RaycastDiagonaleBasDroite = LanceRayon(transform.position, -transform.up + new Vector3(0, 0, -1.5f), Color.red, true);
            RaycastDiagonaleBasGauche = LanceRayon(transform.position, -transform.up + new Vector3(0, 0, 1.5f), Color.blue, true);
        }
        if (currentDirection == new Vector3(-1, 0, 0))
        {
            RaycastDiagonaleBasDroite = LanceRayon(transform.position, -transform.up + new Vector3(0, 0, 1.5f), Color.red, true);
            RaycastDiagonaleBasGauche = LanceRayon(transform.position, -transform.up + new Vector3(0, 0, -1.5f), Color.blue, true);
        }

        if (!doitTourner)
        {
            transform.position = transform.position + (currentDirection * movementSpeed * Time.fixedDeltaTime);
            // sw.WriteLine("0");
            label = 0;

            RaycastHit RaycastDessousApres = LanceRayon(transform.position, (-transform.up), Color.yellow, true);
            if (RaycastDessousApres.collider != null)
            {
                if (RaycastDessousApres.collider.gameObject != null)
                {
                    if (RaycastDessousApres.collider.gameObject.GetComponent<SensDeParcours>() != null)
                    {
                        prochaineDirection = RaycastDessousApres.collider.gameObject.GetComponent<SensDeParcours>().Direction;
                    }
                }
            }

            changementdirectiondetecte = currentDirection != prochaineDirection;

            
            RaycastHit RaycastEnFace = LanceRayon(transform.position, -transform.up + currentDirection, Color.green, true);
            if (RaycastEnFace.collider != null)
            {
                if (RaycastEnFace.collider.tag != "Segment" && changementdirectiondetecte)
                {
                    doitTourner = true;
                    changementdirectiondetecte = false;
                    currentDirection = prochaineDirection;
                }
            }
                    
        }
        else if (RaycastDiagonaleBasDroite.collider.tag == "Segment") {
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            // sw.WriteLine("1");
            label = 1;
            doitTourner = false;
            currentDirection = prochaineDirection;
        }
        else if (RaycastDiagonaleBasGauche.collider.tag == "Segment")
        {
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
            // sw.WriteLine("2");
            label = 2;
            doitTourner = false;
            currentDirection = prochaineDirection;
        }
        else
        {
            print("PEPITOOOOO");
        }

        T += Time.deltaTime;
        if (T >= timer || label != memLabel)
        {
            memLabel = label;
            //ts.CamCapture(label, derniereAction);
            derniereAction = label;
            T = 0.0f;
        }
        //ts.CamCapture();
    }

    RaycastHit LanceRayon(Vector3 position, Vector3 direction, Color c, bool draw = false)
    {
        RaycastHit r;
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        Physics.Raycast(position, direction, out r, Mathf.Infinity, layerMask);
        if(draw)
        {
            Debug.DrawRay(position, direction * r.distance, c);
        }
        return r;
    }

    // front back right lef
    bool[] RaycastsDirectionels()
    {
        RaycastHit RaycastDiagonaleBasDroite = LanceRayon(transform.position, (-transform.up + transform.right / 4), Color.yellow, true);
        RaycastHit RaycastDiagonaleBasGauche = LanceRayon(transform.position, (-transform.up - transform.right / 4), Color.blue, true);
        RaycastHit RaycastDiagonleFront = LanceRayon(transform.position, (-transform.up + transform.forward / 4), Color.red, true);
        RaycastHit RaycastDiagonleBack = LanceRayon(transform.position, (-transform.up - transform.forward / 4), Color.magenta, true);


        bool RaycastsOnSegment = RaycastDiagonleBack.collider.tag == "Segment" && RaycastDiagonleFront.collider.tag == "Segment"
            && RaycastDiagonaleBasDroite.collider.tag == "Segment" && RaycastDiagonaleBasGauche.collider.tag == "Segment";

        return new bool[] { RaycastDiagonleFront.collider.tag == "Segment", RaycastDiagonleBack.collider.tag == "Segment", RaycastDiagonaleBasDroite.collider.tag == "Segment", RaycastDiagonaleBasGauche.collider.tag == "Segment" };
    }

    // x - x z - z 
    bool[] RaycastsDirectionelsAbsolus()
    {
        RaycastHit RaycastX = LanceRayon(transform.position, Vector3.Normalize(new Vector3(0.25f,-1.0f,0.0f)), Color.yellow, false);
        RaycastHit Raycast_X = LanceRayon(transform.position, Vector3.Normalize(new Vector3(0.25f, -1.0f, 0.0f)), Color.blue, false);
        RaycastHit RaycastZ = LanceRayon(transform.position, Vector3.Normalize(new Vector3(0.25f, -1.0f, 0.0f)), Color.red, false);
        RaycastHit Raycast_Z = LanceRayon(transform.position, Vector3.Normalize(new Vector3(0.25f, -1.0f, 0.0f)), Color.magenta, false);

        if(RaycastX.collider == null || Raycast_X.collider == null || RaycastZ.collider == null || Raycast_Z.collider == null)
        {
            return new bool[] { false, false, false, false };
        }

        bool RaycastsOnSegment = RaycastX.collider.tag == "Segment" && Raycast_X.collider.tag == "Segment"
            && RaycastZ.collider.tag == "Segment" && Raycast_Z.collider.tag == "Segment";

        return new bool[] { RaycastX.collider.tag == "Segment", Raycast_X.collider.tag == "Segment", RaycastZ.collider.tag == "Segment", Raycast_Z.collider.tag == "Segment" };
    }

    bool EstAligneAvecLaLigne()
    {
        RaycastHit RaycastEnDessous = LanceRayon(transform.position, -Vector3.up, Color.green, false);
        if(RaycastEnDessous.collider == null)
        {
            return false;
        }
        if (RaycastEnDessous.collider.tag == "Segment")
        {
            SensDeParcours AlignementDeParcours = RaycastEnDessous.collider.gameObject.GetComponent<SensDeParcours>();
            if(AlignementDeParcours.Direction == transform.forward)
            {
                return true;
            } else
            {
                return false;
            }
        } else
        {
            return false;
        }
    }

    bool EstSurLaLigne()
    {
        RaycastHit RaycastEnDessous = LanceRayon(transform.position, -Vector3.up, Color.green, true);
        if(RaycastEnDessous.collider == null)
        {
            return false;
        }
        if (RaycastEnDessous.collider.tag == "Segment")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
