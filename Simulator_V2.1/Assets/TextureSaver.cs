using System.IO;
using UnityEngine;

public class TextureSaver : MonoBehaviour
{

    public int FileCounter = 0;

    public Camera CamWireframe;
    public Camera CamR;
    public Camera CamSC;
    public float timer = 1.0f;
    private float T = 0.0f;
    public GameObject IARestitution;
    private StreamWriter sw;
    private StreamWriter sw2;

    void Start()
    {
        string path = @"D:/Dataset_Light/Labels/labels.txt";
        string path2 = @"D:/Dataset_Light/DernieresActions/da.txt";
        if (!File.Exists(path))
        {
            // Create a file to write to.
            sw = File.CreateText(path);
        } else
        {
            FileStream fs1 = File.OpenWrite(path);
            sw = new StreamWriter(fs1);
        }
        if (!File.Exists(path2))
        {
            // Create a file to write to.
            sw2 = File.CreateText(path2);
        }
        else
        {
            FileStream fs2 = File.OpenWrite(path2);
            sw2 = new StreamWriter(fs2);
        }
    }

    private void LateUpdate()
    {
        T += Time.deltaTime;
        if (T >= timer)
        {
            // CamCapture();
            //FileCounter++;
            T = 0.0f;
        }
    }

    public void CamCapture(int label, int da)
    {
        // !! Attention a retirer vite //
        FileCounter++;
        // !!!!!!!!!!!!!!!!!!!!!!!!!!! //

        // Labels
        sw.WriteLine(label);
        sw.Flush();
        sw2.WriteLine(da);
        sw2.Flush();

         // R
         RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = CamR.targetTexture;

        CamR.Render();

        Texture2D Image = new Texture2D(CamR.targetTexture.width, CamR.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, CamR.targetTexture.width, CamR.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        File.WriteAllBytes("D:/Dataset_Light/R/R_" + FileCounter.ToString() + ".png", Bytes);

        // WF
        currentRT = RenderTexture.active;
        RenderTexture.active = CamWireframe.targetTexture;

        CamWireframe.Render();

        Image = new Texture2D(CamWireframe.targetTexture.width, CamWireframe.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, CamWireframe.targetTexture.width, CamWireframe.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        Bytes = Image.EncodeToPNG();
        Destroy(Image);

        File.WriteAllBytes("D:/Dataset_Light/WF/WF_" + FileCounter.ToString() + ".png", Bytes);

        // SC
        currentRT = RenderTexture.active;
        RenderTexture.active = CamSC.targetTexture;

        CamSC.Render();

        Image = new Texture2D(CamSC.targetTexture.width, CamSC.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, CamSC.targetTexture.width, CamSC.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        Bytes = Image.EncodeToPNG();
        Destroy(Image);

        File.WriteAllBytes("D:/Dataset_Light/SC/SC_" + FileCounter.ToString() + ".png", Bytes);
    }

}
