using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foto : MonoBehaviour
{

    int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeShot()
    {
        StartCoroutine("Capture");
    }

    public IEnumerator Capture()
    {
        ScreenCapture.CaptureScreenshot("Screenshot" + i + ".png");
        i++;
        yield return null;
    }
}
