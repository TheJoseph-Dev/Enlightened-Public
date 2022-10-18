using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public bool playOnAwake = true;

    public int gameScene = -1;

    private void Awake()
    {
        if (gameScene == -1)
            this.gameScene = NextScene();

        if (SceneManager.GetActiveScene().buildIndex != gameScene && !playOnAwake)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        print("Play");
        this.gameObject.SetActive(true);
        StartCoroutine(LoadScene(gameScene));
    }

    private IEnumerator LoadScene(int scene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        print("Loading");
        SceneManager.LoadScene(scene);
    }


    private int NextScene() { return SceneManager.GetActiveScene().buildIndex + 1; }
}
