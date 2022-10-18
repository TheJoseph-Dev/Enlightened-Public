using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{
    public Canvas canvas;
    public CanvasGroup hudGroup;

    public void SetCamera(Camera cam)
    {
        canvas.worldCamera = cam;
    }

    public void SetHUDOpacity(float opacity)
    {
        hudGroup.alpha = opacity;
    }
}
