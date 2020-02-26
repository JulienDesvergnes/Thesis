using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Simulator : MonoBehaviour
{

    // Attributs
    public Canvas canvas;
    public RawImage rawImage;
    public Camera camera;
    float TimeStep;

    void Start()
    {
        rawImage.rectTransform.sizeDelta = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        rawImage.texture = new Texture2D(Screen.currentResolution.width, Screen.currentResolution.height);

        //acquisition camera

       

    }

    void Update()
    {
        // resize en cas de mise a l'echelle
        rawImage.rectTransform.sizeDelta = new Vector2(camera.pixelRect.width, camera.pixelRect.height);

        TimeStep += Time.deltaTime;
        if (TimeStep > 0.5)
        {
            TimeStep = 0;
            for (int y = 0; y < rawImage.texture.height; y++)
            {
                for (int x = 0; x < rawImage.texture.width; x++)
                {
                    Color color = ((x & y) != 0 ? Color.white : Color.gray);
                    (rawImage.texture as Texture2D).SetPixel(x, y, color);
                }
            }
            (rawImage.texture as Texture2D).Apply();
        }
    }

}
