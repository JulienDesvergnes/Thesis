 
using UnityEngine;
using System.Collections;
 
public class SlowRotation : MonoBehaviour {
 
    public Vector3 RotateAmount;  // degrees per second to rotate in each axis. Set in inspector.
    private Vector3 scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
    //private int signe = 1;
   
    // Update is called once per frame
    void Update () {
        transform.Rotate(RotateAmount * Time.fixedDeltaTime);
        /*transform.localScale += signe * scaleChange;
        if (transform.localScale.x > 0.8 || transform.localScale.x < 0.05)
        {
            signe = - signe;
        }*/

    }
}