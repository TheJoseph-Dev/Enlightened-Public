using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsReset : MonoBehaviour
{
    public Material fullscreenWaveMat;
    public OptionsSettings options;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenWaveMat.SetFloat("_Radius", 0.2f);

        options.Reseter();
    }

}
