using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas canvas;
    public GameObject PauseMenuObject;
    public GameObject optionsMenu;

    public HideCursor hideCursor;
    public SceneTransition transition;

    public static bool isPaused { get; private set; }

    private bool goingToMenu = false;

    private void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private bool HandlePauseMenu()
    {
        bool isActive = PauseMenuObject.activeInHierarchy;
        PauseMenuObject.SetActive(!isActive);

        canvas.renderMode = (!isActive ? RenderMode.ScreenSpaceCamera : RenderMode.ScreenSpaceOverlay);
        canvas.gameObject.transform.GetChild(0).gameObject.SetActive(isActive); // Disable HUD

        hideCursor.activate = !isActive;

        return !isActive;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool onOptions = optionsMenu.activeInHierarchy;
            optionsMenu.SetActive(false);
            PauseMenuObject.SetActive(onOptions ? true : PauseMenuObject.activeInHierarchy);

            isPaused = HandlePauseMenu();

            // When it is pressed, timeScale = 1, but it can be stopped by other interactions
            if (!isPaused) Time.timeScale = 1f;
        }

        // While it is active, time must be stopped
        if (isPaused && !goingToMenu) Time.timeScale = 0f;
    }


    public void Resume()
    {
        isPaused = HandlePauseMenu();

        if (!isPaused) Time.timeScale = 1f;
    }

    
    public void Options()
    {
        optionsMenu.SetActive(true);

        bool isActive = PauseMenuObject.activeInHierarchy;
        PauseMenuObject.SetActive(!isActive);
    }
    

    public void Menu()
    {
        this.goingToMenu = true;
        transition.gameScene = 1;
        Time.timeScale = 1f;
        transition.Play();
    }
}
