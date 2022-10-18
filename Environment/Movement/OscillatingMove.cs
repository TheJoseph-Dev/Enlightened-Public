using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingMove : MonoBehaviour
{
    //public Rigidbody rb;

    public Vector3 startPoint = new Vector3(-14, -1, 10.3f);
    public float distance = 13f;

    //public Vector3 endPoint = new Vector3(-1, -1, 10.3f);

    //public AnimationCurve velocity = new AnimationCurve();
    public float speed = 200f;

    public Vector3 move { get; private set; }

    private Vector3 moveDelta = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPoint;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float amplitude = this.distance;

        TimeScale tScaler = GameObject.Find("TimeScaler").GetComponent<TimeScale>();
        float timeScale = tScaler.value;

        //float smoothSine = Mathf.Sin(Time.time * Mathf.Deg2Rad * speed * 1f);//Mathf.Lerp( Mathf.Sin(Time.time * Mathf.Deg2Rad * speed * 0.1f), Mathf.Sin(Time.time * Mathf.Deg2Rad * speed * 1f), timeScale);
        //float oscilation = ((smoothSine + 1) / 2) * amplitude; //Mathf.Abs( smoothSine ) * amplitude;

        float x = Time.time * Mathf.Deg2Rad * speed;
        float triangleWave = 2 * Mathf.Abs(x / Mathf.PI - Mathf.Floor(x / Mathf.PI) - 0.5f);
        float oscilation = triangleWave * amplitude;

        this.moveDelta = startPoint + new Vector3(oscilation, 0, 0) - transform.localPosition;
        transform.localPosition = startPoint + new Vector3(oscilation, 0, 0);
    }

    private Transform previousPlayerTransform;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            previousPlayerTransform = collision.transform;
            collision.transform.parent = transform;
            collision.transform.localScale = previousPlayerTransform.localScale;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
        collision.transform.localScale = Vector3.one / 2;
    }
}
