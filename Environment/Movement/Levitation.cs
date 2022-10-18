using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour
{

    public float velocity = 0.5f;
    public Vector3 rotationVelocity = Vector3.one;

    //public bool spawn = false;

    public int seed = 0;

    // public Vector3 around = Vector3.zero; // Orbitate another object



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Levitatate(transform);

    }

    void Levitatate(Transform t)
    {
        Random.InitState(seed);
        float r = Random.Range(-1, 1);
        Vector3 rotationValue = new Vector3(r * velocity * 1.2f * rotationVelocity.x, r * velocity * rotationVelocity.y, r * velocity * 0.8f * rotationVelocity.z);

        t.Rotate(rotationValue);

        float levitationValue = Mathf.Sin(Time.realtimeSinceStartup) * velocity * Time.deltaTime;

        t.position = new Vector3(t.position.x, t.position.y + levitationValue, t.position.z);
    }
}
