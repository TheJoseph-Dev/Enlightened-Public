using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class JulioShedInteractor : MonoBehaviour
{
    public GameObject UIInteractor;
    public LayerMask playerLayerMask;

    public Interactor playerInteractor;
    public PlayerState playerState;

    public AudioSource julioAudioSource;
    public AudioClip[] julioAudios;
    public AudioClip julioSong;

    public Transform playerPos;

    private int voiceLineIndex = 0;
    private bool isActive = false;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleState();
        UpdateUI();

        if (isActive) PlayVoiceLine();
        isPlaying = julioAudioSource.isPlaying;
    }

    private void PlayVoiceLine()
    {
        julioAudioSource.PlayOneShot( julioAudios[voiceLineIndex] );
        if (voiceLineIndex < julioAudios.Length - 1) voiceLineIndex++; else voiceLineIndex = 0;

        isActive = false;
    }

    private bool shouldPlaySong = false;
    private void PlaySong()
    {
        float threshold = 5f;
        float distance = Vector3.Distance(transform.position, playerPos.position);

        shouldPlaySong = (distance < threshold && !isPlaying); 
        
        // f(x) = -x + 1
        if (!julioAudioSource.isPlaying && shouldPlaySong) { julioAudioSource.volume = -(distance / 5) + 1; julioAudioSource.PlayOneShot(julioSong); }
    }

    private void UpdateUI()
    {
        // UI Interactor
        bool isPlayerNear = Physics.CheckSphere(transform.position, 3f, playerLayerMask);
        if (isPlayerNear)
        {
            var interactor = UIInteractor.GetComponent<SpriteRenderer>();
            var selectInteractor = UIInteractor.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

            float alpha = Mathf.Lerp(0.0f, 1.0f, 5) * Time.deltaTime;

            if (interactor.color.a < 1)
                interactor.color = new Color(255, 255, 255, interactor.color.a + alpha);

            if (selectInteractor.color.a < 1 && isPlaying)
                selectInteractor.color = new Color(selectInteractor.color.r, selectInteractor.color.g, selectInteractor.color.b, selectInteractor.color.a + (2 * alpha));

            if (selectInteractor.color.a > 0 && !isPlaying)
                selectInteractor.color = new Color(selectInteractor.color.r, selectInteractor.color.g, selectInteractor.color.b, selectInteractor.color.a - (2 * alpha));

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
        }
    }

    private void HandleState()
    {
        bool isPlayerNear = Physics.CheckSphere(transform.position, 3f, playerLayerMask);
        bool isParticle = playerState.isParticle;

        if (isPlayerNear && isParticle)
        {
            HandleKeyPressed();
        }
    }

    private void HandleKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPlaying)
        {
            isActive = true; // Swap
            playerInteractor.interaction = new InteractionData(InteractionType.JulioShed, gameObject);

        }

    }
}
