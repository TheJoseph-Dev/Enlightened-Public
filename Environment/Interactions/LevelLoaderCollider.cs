using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoaderCollider : MonoBehaviour
{
    public SceneTransition transition;

    public GameObject trackingObject;
    private void Update()
    {
        bool trigger = trackingObject.transform.position.x >= 36f;

        if (trigger) transition.Play();
    }
}
