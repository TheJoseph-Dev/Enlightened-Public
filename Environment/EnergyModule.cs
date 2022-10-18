using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MyMaterials
{
    public static Material defaultMat = Resources.Load("Materials/Floor1") as Material;
}

public class EnergyModule : MonoBehaviour
{
    private Vector3[] vertices =
    {
        new Vector3(0,0,0), new Vector3(0,1,0), new Vector3(1,0,0), new Vector3(1,1,0), // Front
        new Vector3(0,0,0), new Vector3(0,1,0), new Vector3(0,0,1), new Vector3(0,1,1), // Left
        new Vector3(0,0,1), new Vector3(0,1,1), new Vector3(1,0,1), new Vector3(1,1,1), // Back
        new Vector3(1,0,1), new Vector3(1,1,1), new Vector3(1,0,0), new Vector3(1,1,0), // Right
        new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,0,1), new Vector3(1,0,1), // Bottom
        new Vector3(0,1,0), new Vector3(0,1,1), new Vector3(1,1,0), new Vector3(1,1,1), // Top
        //new Vector3(0.2f,1,0.2f),    new Vector3(0.2f,1,0.8f),    new Vector3(0.8f,1,0.2f),    new Vector3(0.8f,1,0.8f),    // 
        //new Vector3(0.2f,0.7f,0.2f), new Vector3(0.2f,0.7f,0.8f), new Vector3(0.8f,0.7f,0.2f), new Vector3(0.8f,0.7f,0.8f), // Module
    };

    private Vector3[] vertices2 =
    {
        new Vector3(0,0,0),
        new Vector3(0,1,0), 
        new Vector3(1,0,0),
        new Vector3(1,1,0),
        new Vector3(0,0,1),
        new Vector3(0,1,1),
        new Vector3(1,0,1),
        new Vector3(1,1,1)
    };

    private int[] triangles2 =
    {
        0, 2, 3, 0, 1, 2,


    };

    private int[] triangles =
    {
        0, 1, 2,  1, 2, 3,
        4,5,6, 5,6,7,
        8,9,10, 9,10,11,
        12,13,14, 13,14,15,
        16,17,18, 17,18,19,
        20,21,22, 21, 22, 23,
        //24,25,26, 25,26,27,

    };
    private Vector2 UVs;


    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices2;
        mesh.triangles = triangles2;
        mesh.Optimize();
        //mehs.Clear();

        this.gameObject.AddComponent<MeshFilter>();
        this.gameObject.GetComponent<MeshFilter>().mesh = mesh;
        //this.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        this.gameObject.GetComponent<MeshRenderer>().material = MyMaterials.defaultMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
