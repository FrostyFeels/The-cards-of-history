using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{

    public int highestFPS, lowestFPS;
    public TextMeshProUGUI _FrameRate, _HighestFrameRate, _LowestFrameRate;

    public int count;
    public int samples = 100;
    public float totalTime;
    // Start is called before the first frame update

    public void Start()
    {
        count = samples;
        totalTime = 0f;
    }
    public void Update()
    {
        int _curfps = (int)(1f / Time.unscaledDeltaTime);


        if (_curfps > highestFPS)
        {
            highestFPS = _curfps;
        }

        if (_curfps < lowestFPS)
        {
            lowestFPS = _curfps;
        }


        count -= 1;
        totalTime += Time.deltaTime;

        if (count > 0)
            return;
        float fps = samples / totalTime;

        _FrameRate.text = "FPS: " + fps.ToString();
        _HighestFrameRate.text = "HighestFPS: " + highestFPS.ToString();
        _LowestFrameRate.text = "LowestFPS: " + lowestFPS.ToString();

        totalTime = 0f;
        count = samples;


   
    }
}
