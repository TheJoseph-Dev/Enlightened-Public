using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=4HpC--2iowE -- Camera tutorial

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
        //Rotate();
    }

    private void Movement()
    {
        Vector3 offset = new Vector3(0.0f, 1.5f, -5f);
        Vector3 position = new Vector3(player.position.x, player.position.y, player.position.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, position, 2);
        transform.position = smoothedPosition;

        //Orbit Object
        /*
        Vector3 axis = Vector3.up;
        float radius = 2.0f;
        float radiusSpeed = 0.5f;
        float rotationSpeed = 80.0f;
          
        transform.RotateAround(player.position, axis, rotationSpeed * Time.deltaTime);
        var desiredPosition = (transform.position - player.position).normalized * radius + player.position;

        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
        */

    }

    private void Rotate()
    {
        transform.LookAt(player.position);
        

        /*Vector3 point = player.position;
        Vector3 axis = new Vector3(0.0f, point.y, 0.0f);
        float angle = Mathf.Atan2(axis.x, axis.z);
        transform.RotateAround(point, axis, angle);*/

        /*Quaternion rotation = Quaternion.Euler(10.0f, player.rotation.eulerAngles.y, 0.0f);
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, rotation, 2);

        transform.rotation = smoothedRotation;*/

        //Vector3 rotation = new Vector3(Input.GetAxisRaw("Mouse Y") * 2, Input.GetAxisRaw("Mouse X") * 8, 0.0f);
        //Vector3 smoothRotation = Vector3.Lerp(transform.rotation.eulerAngles, rotation, 2);
        //float angle = 10.0f;

    }
}
