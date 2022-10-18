using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAnimator : MonoBehaviour
{
    public List<Vector3> positions;
    public List<Quaternion> rotations;

    public float[] positionDurations;
    public float[] rotationDurations;

    public bool startAtCurrentTransform = true;
    // Maybe use serializible structs 
    //public Dictionary<Vector3[], float[]> positions = new Dictionary<Vector3[], float[]>();
    //public Dictionary<Quaternion[], float[]> rotations = new Dictionary<Quaternion[], float[]>();
    //public List<float[]> durations;


    // Start is called before the first frame update
    void Start()
    {
        if (startAtCurrentTransform)
        {
            positions.Insert(0, transform.position);

            rotations.Insert(0, transform.rotation);
        }


        if (positions.Count > 1)
        {
            for (int i = 0; i < positions.Count - 1; i++)
            {
                StartCoroutine(MoveWithDelay(positions.ToArray()[i], positions.ToArray()[i + 1], positionDurations[i]));
            }
        }

        if (rotations.Count > 1)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator MoveWithDelay(Vector3 posA, Vector3 posB, float delay)
    {

        float counter = 0f;
        while (counter < delay)
        {
            Vector3 move = Vector3.Lerp(posA, posB, counter / delay);
            transform.position = move;

            counter += Time.deltaTime;


            yield return null;
        }

        transform.position = posB;

        yield return null;
    }

    // Quaternions > Euler
    private IEnumerator RotateWithDelay(Quaternion rotA, Quaternion rotB, float delay)
    {

        float counter = 0f;
        while (counter < delay)
        {

            Quaternion delta = Quaternion.Lerp(rotA, rotB, counter / delay);
            transform.rotation = delta;

            counter += Time.deltaTime;


            yield return null;
        }

        transform.rotation = rotB;

        yield return null;
    }
}
