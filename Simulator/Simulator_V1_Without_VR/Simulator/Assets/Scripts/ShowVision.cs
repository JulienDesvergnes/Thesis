using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVision : MonoBehaviour
{

    public Camera camera;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 position = camera.transform.position;
        Vector3 fwd = camera.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, fwd * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }

    }
}
