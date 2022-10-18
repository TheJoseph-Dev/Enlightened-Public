using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public Animator cameraAnimation;
    public int animationIndex = 1;

    public AudioSource storySongSource;
    //public AudioClip storySongClip;

    public SceneTransition transition;

    public CanvasGroup skipUI;


    public bool animationEnded = false;

    private void Start()
    {
        //storySongSource.volume = 1; //(100 * GameSettings.volume) - 80; // f(x) = 100x -80
        //storySongSource.PlayOneShot(storySongClip);

        cameraAnimation.Play("Story Camera " + animationIndex.ToString());
    }

    // Update is called once per frame
    void Update() // TODO: Song is taking too long to end, so you must cut the final silence from the original audio file
    {
        if (PauseAnimation()) return;

        StartCoroutine(ShouldSkipStory(2));


        if (shouldSkip) {
            storySongSource.Stop();
            animationEnded = true;
        }

        //bool shouldTrigger = !storySongSource.isPlaying; // Checks if audio ends

        if (animationEnded) StartCoroutine( TransitionTrigger(1) );
    }

    bool PauseAnimation()
    {
        cameraAnimation.speed = PauseMenu.isPaused ? 0 : 1;
        if(PauseMenu.isPaused) storySongSource.Pause(); else storySongSource.UnPause();
        
        return PauseMenu.isPaused;
    }

    private IEnumerator TransitionTrigger(float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        transition.Play();
    }

    private bool btPressed = false;
    private bool shouldSkip = false;
    private IEnumerator ShouldSkipStory(float delay = 2)
    {
        if (Input.anyKeyDown)
        {
            while (skipUI.alpha < 1)
            {
                skipUI.alpha += Time.deltaTime;
                //yield return null;
            }

            shouldSkip = Input.GetKeyDown(KeyCode.S);

            yield return new WaitForSeconds(delay);

            btPressed = true;
        }

        while (skipUI.alpha > 0 && btPressed)
        {
            skipUI.alpha -= Time.deltaTime;
            yield return null;
        }

        btPressed = false;
    }


    
}
