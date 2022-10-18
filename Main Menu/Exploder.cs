using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Exploder : MonoBehaviour
{
    public bool activate;
    public float force = 2f;

    public float range = 5f;
    public float radius = 4f;

    public float delay = 1f;

    private Collider[] colliders;

    // Update is called once per frame
    void Update()
    {
        this.colliders = Physics.OverlapSphere(transform.position, range);
        if (activate)
        {
            Delayer();

            foreach(Collider c in colliders)
            {
                Rigidbody rb = c.GetComponent<Rigidbody>();
                if ( rb != null )
                {
                    rb.AddExplosionForce(force, transform.position, radius);
                } 

            }

        }

        activate = false;

        if (colliders.Length == 0) { return; }
        // Deactivates falling objects
        foreach(Collider c in colliders)
        {
            if (c == null) { continue; }

            if(c.gameObject.transform.position.y < -10)
            {
                Destroy(c.gameObject);
            }
        }

    }


    private void Delayer() 
    {
        float f = 0;
        while (f < delay) f += Time.deltaTime * 0.1f;
    }
}
