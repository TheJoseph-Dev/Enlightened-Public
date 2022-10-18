using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InteractionType
{
    Null = 0,
    EnergyModule = 1,
    Plaque = 2,
    PortalCutscene = 3,
    JulioShed = 4
}

public struct InteractionData
{
    public InteractionType type;
    public GameObject interactiveObject;

    public InteractionData(InteractionType type = InteractionType.Null, GameObject obj = null)
    {
        this.type = type;
        this.interactiveObject = obj;
    }
}

// Maybe should merge this class into player state
public class Interactor : MonoBehaviour
{
    public bool isInteracting { get; private set; }
    public InteractionData interaction = new InteractionData();

    // Update is called once per frame
    void Update()
    {
        isInteracting = (interaction.type != InteractionType.Null);
    }
}
