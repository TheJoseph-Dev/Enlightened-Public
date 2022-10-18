using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public Transform targetTransform;


    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = targetTransform.rotation;
    }
}
