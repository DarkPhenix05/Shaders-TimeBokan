using System.IO;
using UnityEngine;
using System.Collections;

public class CameraCapture : MonoBehaviour
{
    public Camera _cam;
    public Texture2D _targetTexture;

    private void FixedUpdate()
    {
        Capture();
    }

    void Capture()
    {
        _cam.Render();
        RenderTexture rt = gameObject.GetComponent<Renderer>().material.mainTexture as RenderTexture;
        Texture2D captureTexture = Texture2DGetRenderTexture(rt);

        byte[] data = captureTexture.EncodeToPNG();
        FileStream fs = new FileStream(@"F:\7mo semestre\ComputoGrafico\ShaderClass4\Assets\Materials\Recursos\SAMPLE.jpg", FileMode.OpenOrCreate);
        fs.Write(data, 0, data.Length);
        fs.Close();

        Texture2D.DestroyImmediate(captureTexture, true);
    }

    public Texture2D Texture2DGetRenderTexture(RenderTexture rt)
    {
        RenderTexture.active = rt;

        Texture2D tempTexture = new Texture2D(rt.width, rt.height);
        tempTexture.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);

        tempTexture.Apply();

        return tempTexture;
    }
}