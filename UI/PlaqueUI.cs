using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor.UI;

public class PlaqueUI : MonoBehaviour
{
    private GameObject currentMessage;

    public GameObject[] plaqueMessages; //{ get; private set; }

    public void Show(bool active = true)
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = active ? 1 : 0;
        this.gameObject.SetActive(active);
    }

    public void ChangeMessage(uint id)
    {
        if(currentMessage != null) 
            currentMessage.SetActive(false);

        currentMessage = plaqueMessages[id];

        currentMessage.SetActive(true);
    }

}
