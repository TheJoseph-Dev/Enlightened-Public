using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimator : MonoBehaviour
{
    public Material material;

    float curFreq = 1f;
    float curSpeed = 0.3f;
    float curOpacity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float finalFrequency = 16f;
        //float finalSpeed = 0.5f;

        if (curOpacity < 5) { curOpacity += Time.deltaTime / 3.6f; }
        material.SetFloat("_Opacity", curOpacity);

        if (curFreq >= finalFrequency) { return; }
        else
        {
            curFreq += Time.deltaTime;
            curSpeed += Time.deltaTime / 1000;
        }



        material.SetFloat("_Frequency", curFreq);
        material.SetFloat("_Speed", curSpeed);
    }
}
