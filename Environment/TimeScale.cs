using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public PlayerState playerState;

    public float value { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (playerState.isParticle && value < 1) {
            this.value += Time.unscaledDeltaTime * 0.22f;
        }
        
        if(!playerState.isParticle && value > 0.1f)
        {
            this.value += -Time.unscaledDeltaTime * 0.9f;
        }
    }

}
