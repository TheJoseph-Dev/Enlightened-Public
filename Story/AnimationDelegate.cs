using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelegate : MonoBehaviour
{

    public StoryController controller;

    public void AnimationObeserverAlert(int alert)
    {
        if (alert == 1)
        {
            // Animation has ended
            controller.animationEnded = true;
        }
    }
}
