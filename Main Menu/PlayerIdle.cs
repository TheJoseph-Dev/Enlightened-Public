using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    private Vector3 lastPos;
    private float levitationCounter = 0f;
    // Update is called once per frame
    void Update()
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        float levitateValue = Mathf.Sin(levitationCounter) / 2;
        Vector3 levitatePos = lastPos + (Vector3.up * levitateValue);

        Vector3 levitationPos = Vector3.Lerp(lastPos, levitatePos, 2);
        transform.position = levitationPos;

        levitationCounter += 1f * Time.deltaTime;
    }
}
