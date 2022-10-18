using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireProgressController : MonoBehaviour
{
    private Material[] materials;

    public float fillProgress = 0.5f;
    public float emptyRate = -0.1f;

    private void Start()
    {
        this.materials = this.GetComponent<Renderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        materials[0].SetFloat("_FillRate", fillProgress);
        materials[0].SetFloat("_EmptyRate", emptyRate);
    }
}

/*
 --- Legacy:

    public GameObject ActiveWire;
    public GameObject InactiveWire;

    public float fillProgress = 0.5f;


    // Update is called once per frame
    void Update()
    {
        float fixedFillProgress = Mathf.Min(Mathf.Max(0.01f, fillProgress), 0.99f);

        float activeWireScale = (transform.localScale.z * fixedFillProgress) / transform.localScale.z;
        ActiveWire.transform.localScale = new Vector3(1, 1, activeWireScale);

        float inactiveWireScale = (transform.localScale.z * (1 - fixedFillProgress)) / transform.localScale.z;
        InactiveWire.transform.localScale = new Vector3(1, 1, inactiveWireScale);
    }
 */ 