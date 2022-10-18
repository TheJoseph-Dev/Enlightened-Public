using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSetter : MonoBehaviour
{
    public GameObject spawnpointObject;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            spawnpointObject.transform.position = transform.position + new Vector3(0, transform.localScale.y/2, 0);
        }
    }
}
