using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaqueInteraction : MonoBehaviour
{
    public GameObject UIInteractor;
    public LayerMask playerLayerMask;

    private bool isInteracting = false;

    //private Interactor playerInteractor;
    //private PlayerState playerState;
    //private const InteractionType interactorType = InteractionType.Plaque;

    public uint plaqueID = 0;
    public PlaqueUI plaqueUI;

    public GameObject hologram;
    public bool interactable = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool pressed = HandleKeyPressed();

        Interactable();

        if (interactable)
            UpdateUI(pressed);
    }

    private void Interactable()
    {
        if(hologram == null) { return; }

        interactable = !hologram.activeInHierarchy;
    }

    private void UpdateUI(bool pressed)
    {
        // UI Interactor
        bool isPlayerNear = Physics.CheckSphere(transform.position, 3f, playerLayerMask);
        if (isPlayerNear)
        {
            var interactor = UIInteractor.GetComponent<SpriteRenderer>();
            var selectInteractor = UIInteractor.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

            float alpha = Mathf.Lerp(0.0f, 1.0f, 5) * Time.unscaledDeltaTime;


            if (interactor.color.a < 1)
                interactor.color = new Color(255, 255, 255, interactor.color.a + alpha);

            if (selectInteractor.color.a < 1 && isInteracting)
                selectInteractor.color = new Color(selectInteractor.color.r, selectInteractor.color.g, selectInteractor.color.b, selectInteractor.color.a + (2 * alpha));

            if (selectInteractor.color.a > 0 && !isInteracting)
                selectInteractor.color = new Color(selectInteractor.color.r, selectInteractor.color.g, selectInteractor.color.b, selectInteractor.color.a - (2 * alpha));

            UIInteractor.transform.GetChild(1).gameObject.SetActive(true); // UI Background

            if (isInteracting) Time.timeScale = 0f;
            if(pressed && !PauseMenu.isPaused) ShowTextUI();
            if (PauseMenu.isPaused) this.isInteracting = false; ShowTextUI(); // Hide when game is paused
        }
        else
        {
            var interactor = UIInteractor.GetComponent<SpriteRenderer>();
            var selectInteractor = UIInteractor.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

            float alpha = Mathf.Lerp(0.0f, 1.0f, 5) * Time.deltaTime;

            if (interactor.color.a > 0)
                interactor.color = new Color(255, 255, 255, interactor.color.a - alpha);

            if (selectInteractor.color.a > 0)
                selectInteractor.color = new Color(selectInteractor.color.r, selectInteractor.color.g, selectInteractor.color.b, selectInteractor.color.a - alpha);

            UIInteractor.transform.GetChild(1).gameObject.SetActive(false); // UI Background
        }

    }


    private void ShowTextUI()
    {
        if(isInteracting)
        {
            plaqueUI.ChangeMessage(plaqueID);
            plaqueUI.Show();
        }
        else
        {
            Time.timeScale = 1;

            plaqueUI.Show(false);
        }
    }

    private bool HandleKeyPressed()
    {
        bool pressed = Input.GetKeyDown(KeyCode.E);
        if (pressed && !PauseMenu.isPaused) this.isInteracting = !this.isInteracting;

        return pressed;
    }
}
