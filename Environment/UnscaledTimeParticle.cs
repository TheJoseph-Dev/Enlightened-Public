using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnscaledTimeParticle : MonoBehaviour
{
    private ParticleSystem pSystem;
    private void Start()
    {
        this.pSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale < 0.3f)
        {
            pSystem.Simulate(Time.unscaledDeltaTime, true, false);
        }
        else
        {
            pSystem.Simulate(Time.deltaTime, true, false);
        }
    }
}
