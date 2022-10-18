using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BetaTime
{
    public static float startTime = 0;
    public static float currentTime = 0;
    public static float endTime = 305;
}

public class BetaTimeLimit : MonoBehaviour
{
    public bool setStart = false;
    public bool ended { get; private set; }

    public CanvasGroup credits;
    public SceneTransition transition;
    public HideCursor cursorMode;

    public GameObject player;
    public GameObject mainCamera;

    private void Start()
    {
        if (setStart)
        {
            BetaTime.startTime = Mathf.FloorToInt( Time.realtimeSinceStartup );
            BetaTime.endTime = Mathf.FloorToInt( BetaTime.startTime ) + 305;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ended)
        {
            CreditsUI();
            BetaTime.startTime = 0;
            BetaTime.currentTime = 0;
            BetaTime.endTime = 305;

            if (player != null) player.SetActive( false );

            if (mainCamera != null) mainCamera.SetActive( false );

            cursorMode.activate = true;

            transition.gameScene = 1; // Menu Scene

            return;
        }

        BetaTime.currentTime = Time.realtimeSinceStartup;

        if(BetaTime.currentTime >= BetaTime.endTime) ended = true;

        //print((BetaTime.currentTime / 60).ToString() + " min");

    }


    private void CreditsUI()
    {
        credits.gameObject.SetActive(true);
        credits.alpha += Time.deltaTime * 0.8f;
    }
}
