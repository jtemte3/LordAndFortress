using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPanelScript : MonoBehaviour
{
    public TMP_Text fpsLabel;
    private string fpsLabelFormat;
    public float refreshRate = .5f;
    private float timer = 0;

    private void Start()
    {
        fpsLabelFormat = fpsLabel.text;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsLabel.text = string.Format(fpsLabelFormat, fps);
            timer = Time.unscaledTime + refreshRate;
        }
    }
}
