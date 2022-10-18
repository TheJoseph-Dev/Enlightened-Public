using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public SceneTransition transition;

    //public Animator transition;
    //public float transitionTime = 1f;



    private void Update()
    {
        //print((int)videoPlayer.frame == (int)videoPlayer.frameCount - 2);
        bool didEnded = videoPlayer.time >= videoPlayer.length - 0.1f;
        if (didEnded)
        {
            MenuScene();
        }
    }

    private void MenuScene()
    {
        //transition.SetTrigger("Start");
        transition.Play();

        /*yield return new WaitForSeconds(transitionTime);

        print("Loading");
        
        int scene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(scene);*/
    }
}
