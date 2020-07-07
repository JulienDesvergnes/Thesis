using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// A simple free camera to be added to a Unity game object.
/// 
/// Keys:
///	wasd / arrows	- movement
///	q/e 			- up/down (local space)
///	r/f 			- up/down (world space)
///	pageup/pagedown	- up/down (world space)
///	hold shift		- enable fast movement mode
///	right mouse  	- enable free look
///	mouse			- free look / rotation
///     
/// </summary>
public class MovePlayer : MonoBehaviour
{
    /// <summary>
    /// Normal speed of camera movement.
    /// </summary>
    public float movementSpeed = 10f;

    /// <summary>
    /// Speed of camera movement when shift is held down,
    /// </summary>
    public float fastMovementSpeed = 100f;

    /// <summary>
    /// Sensitivity for free look.
    /// </summary>
    public float freeLookSensitivity = 3f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel.
    /// </summary>
    public float zoomSensitivity = 10f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel (fast mode).
    /// </summary>
    public float fastZoomSensitivity = 50f;

    /// <summary>
    /// Set to true when free looking (on right mouse button).
    /// </summary>
    private bool looking = false;

    public bool marcheAutonome = true;
    public bool deplacementLearning = true;
    public GameObject renderPlane;
    private Renderer renderer;

    // Affiche truc touchés
    private Collider toucheDevant;
    private Collider toucheDroite;
    private Collider toucheGauche;
    public bool cibleVue = false;
    public bool cibleTouchee = false;
    public bool murTouche = false;
    public GameObject cible;
    public int nbMurTouche = 0;
    
    void Update()
    {
        if(!marcheAutonome && !deplacementLearning) {
            var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            var movementSpeed = fastMode ? this.fastMovementSpeed : this.movementSpeed;
            var lockHeight = 1;

            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position = transform.position + (-transform.right * movementSpeed * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position = transform.position + (transform.right * movementSpeed * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position = transform.position + (-transform.forward * movementSpeed * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                transform.position = transform.position + (transform.up * movementSpeed * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.position = transform.position + (-transform.up * movementSpeed * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.PageUp))
            {
                transform.position = transform.position + (Vector3.up * movementSpeed * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.PageDown))
            {
                transform.position = transform.position + (-Vector3.up * movementSpeed * Time.fixedDeltaTime);
            }

            if (looking)
            {
                float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
                float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
                transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
            }

            float axis = Input.GetAxis("Mouse ScrollWheel");
            if (axis != 0)
            {
                var zoomSensitivity = fastMode ? this.fastZoomSensitivity : this.zoomSensitivity;
                transform.position = transform.position + transform.forward * axis * zoomSensitivity;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StartLooking();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                StopLooking();
            }

            transform.position = new Vector3(transform.position.x,lockHeight,transform.position.z);
        }
        else if(marcheAutonome && !deplacementLearning) {
            if(renderer.material.shader.name == "Custom/AllBlackShader") {
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
                    transform.position = transform.position + (-transform.right * movementSpeed * Time.fixedDeltaTime);
                } else if (decideur == -1) {
                    // Droite
                    transform.position = transform.position + (transform.right * movementSpeed * Time.fixedDeltaTime);
                } else if (decideur == 1) {
                    // Haut
                    transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
                } else if (decideur == 2) {
                    // Bas
                    transform.position = transform.position + (-transform.forward * movementSpeed * Time.fixedDeltaTime);
                }
                
                // Deplacement rotation
                float modifieurDeRotationSurY = rand.Next(20);
                int sensDeRotation = 1;
                if(rand.Next(2) == 0) {
                    sensDeRotation = -1;
                }
                transform.Rotate(0.0f,sensDeRotation * modifieurDeRotationSurY,0.0f,Space.Self);
            } else {
                RaycastHit hitDevant;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitDevant, Mathf.Infinity))
                {
                    if (toucheDevant != hitDevant.collider) {
                        if(toucheDevant != null) {
                            toucheDevant.transform.GetComponent<Renderer>().material.color = new Color(132/255.0f,132/255.0f,132/255.0f,1.0f);
                        }
                        toucheDevant = hitDevant.collider;
                        Renderer rend = hitDevant.transform.GetComponent<Renderer>();
                        rend.material.color = Color.yellow;
                    }
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitDevant.distance, Color.yellow);
                }
                /*RaycastHit hitGauche;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + Vector3.right / 2), out hitGauche, Mathf.Infinity))
                {
                    if (toucheGauche != hitGauche.collider) {
                        if(toucheGauche != null) {
                            toucheGauche.transform.GetComponent<Renderer>().material.color = new Color(132/255.0f,132/255.0f,132/255.0f,1.0f);
                        }
                        toucheGauche = hitGauche.collider;
                        Renderer rend = hitGauche.transform.GetComponent<Renderer>();
                        rend.material.color = Color.green;
                    }
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward + Vector3.right / 2) * hitGauche.distance, Color.green);
                }*/
                RaycastHit hitDroite;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitDroite, Mathf.Infinity))
                {
                    if (toucheDroite != hitDroite.collider) {
                        if(toucheDroite != null) {
                            toucheDroite.transform.GetComponent<Renderer>().material.color = new Color(132/255.0f,132/255.0f,132/255.0f,1.0f);
                        }
                        toucheDroite = hitDroite.collider;
                        Renderer rend = hitDroite.transform.GetComponent<Renderer>();
                        rend.material.color = Color.green;
                    }
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitDroite.distance, Color.green);
                }

                // Cible vue ?
                if(!cibleVue) {
                    cibleVue = hitDevant.collider.gameObject == cible;
                }
                if (cibleVue) {
                    // GOGOGO
                    transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
                } else {
                    // Rotation
                /*if(hitDroite.distance < 0.5 && hitGauche.distance < 0.5) {
                    //Debug.Log("hitDroite.distance < 0.5 && hitGauche.distance < 0.5 === Rotate de 1");
                    transform.Rotate(0.0f,(float)previousRotation * 25,0.0f,Space.Self);
                } else if (hitGauche.distance < 0.5) {
                    //Debug.Log("hitGauche.distance < 0.5 === Rotate de -1");
                    transform.Rotate(0.0f,-1.0f,0.0f,Space.Self);
                    previousRotation = -1;
                } else if (hitDroite.distance < 0.5) {
                    //Debug.Log("hitDroite.distance < 0.5 === Rotate de 1");
                    transform.Rotate(0.0f,1.0f,0.0f,Space.Self);
                    previousRotation = 1;
                } else {
                    // Translation
                    transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
                }*/
                    if(hitDevant.distance < 0.5) {
                        var rand = new System.Random();
                        int m = 1;
                        if(rand.Next(2) == 0) {
                            m = -1;
                        }
                        if(notAligned(transform.rotation)) {
                            transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                        }
                        transform.Rotate(0.0f,m * 90.0f,0.0f,Space.Self);
                    } else {
                        //if(hitDroite.distance > 2) {
                        //    transform.Rotate(0.0f,90.0f,0.0f,Space.Self);
                        //} else {
                        transform.position = transform.position + (transform.forward * movementSpeed * Time.fixedDeltaTime);
                        //}
                    }
                }
            }
        } else if(deplacementLearning) {
        }
    }

    bool notAligned(Quaternion q) {
        bool res = false;
        if(Math.Abs(q.eulerAngles.y - 0.0f) > 0.1f && Math.Abs(q.eulerAngles.y - 90.0f) > 0.1f &&
           Math.Abs(q.eulerAngles.y - 180.0f) > 0.1f && Math.Abs(q.eulerAngles.y - 270.0f) > 0.1f) {
               res = true;
        }
        return res;
    }
    void OnDisable()
    {
        StopLooking();
    }

    /// <summary>
    /// Enable free looking.
    /// </summary>
    public void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Disable free looking.
    /// </summary>
    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Start() {
        renderer = renderPlane.GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == cible) {
            cibleTouchee = true;
        }
        else if (collision.gameObject.tag == "wall") {
            murTouche = true;
            nbMurTouche += 1;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "wall") {
            murTouche = false;
        }
    }
}