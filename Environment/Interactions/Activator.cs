using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ActivationType
{
    None = 0,
    Hologram = 1,
    Rotate = 2
}


public class Activator : MonoBehaviour
{
    public bool isActive { get; private set; }

    public GameObject[] interactable;

    public int chargeTarget = 1;
    private int chargeCount = 0;

    [SerializeField]
    private ActivationType type = ActivationType.None;


    // Update is called once per frame
    void Update()
    {
        Activate();
    }

    private void Activate()
    {
        switch (type)
        {
            case ActivationType.Hologram:

                for (int i = 0; i < interactable.Length; i++)
                {
                    Material mat = interactable[i].GetComponent<Renderer>().material;
                    if (mat == null) { return; }

                    float until = isActive ? 3 : 0;

                    if (chargeCount == chargeTarget && until == 3)
                        DissolveHologram(mat, until, 1f, i);

                    
                    if (chargeCount < chargeTarget && until == 0 && mat.name != "StateHologram (Instance)")
                        DissolveHologram(mat, until, 1f, i);

                }

                break;

            case ActivationType.Rotate:

                if (!isActive) { return; }

                for (int i = 0; i < interactable.Length; i++)
                {
                    Transform obj = interactable[i].transform;
                    if (obj == null) { return; }

                    //float multiplier = Mathf.Sin(obj.rotation.eulerAngles.y * Mathf.Deg2Rad) * 90;
                    Vector3 rotation = Vector3.up * Time.deltaTime * 25;
                    obj.Rotate(rotation);

                }

                break;

            case ActivationType.None: return;
        }
    }

    private void DissolveHologram(Material mat, float until, float time, int i = 0)
    {
        float currentLevel = mat.GetFloat("_DissolveLevel");
        float delta = Mathf.Lerp(currentLevel, until, Time.deltaTime / time);

        mat.SetFloat("_DissolveLevel", delta);
        //dissolveLevel = b;

        float margin = (until == 0) ? 0.02f : -0.06f;
        if(currentLevel >= until + margin)
            interactable[i].SetActive(!isActive);
    }


    public void Set(bool state)
    {
        if (state == true && chargeCount < chargeTarget) chargeCount += 1;
        else if (state == false && chargeCount > 0) chargeCount -= 1;
        //print("Count: " + chargeCount.ToString());

        this.isActive = state;
    }

    public void Toggle()
    {
        this.isActive = !this.isActive;
    }
}
