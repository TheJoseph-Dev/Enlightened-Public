using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCutscene : MonoBehaviour
{
    private Camera cam;

    public Transform mainCamera;
    public Animator anim;

    public CutsceneHandler uiHandler;

    public SceneTransition transition;

    public bool trigger { get; private set; }

    public PlayerState playerState;

    public Material crystalsMat;
    public Material sunGlowMat;
    public GameObject sunEffect;

    private void Start()
    {
        this.trigger = false;
        this.cam = transform.GetComponent<Camera>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (trigger)
        {
            print("Triggered");

            transform.position = mainCamera.transform.position;
            transform.rotation = mainCamera.transform.rotation;

            cam.enabled = true;
            Vector3 startPosition = new Vector3(0, 10, 65);
            Quaternion startRotation = Quaternion.Euler( new Vector3(12, 0, 0) );

            float delay = 1.2f;
            StartCoroutine(MoveWithDelay(transform.position, startPosition, delay));
            StartCoroutine(RotateWithDelay(transform.rotation, startRotation, delay));

            StartCoroutine(PlayAnimation(delay + 0.1f));

            uiHandler.SetCamera(cam);
            uiHandler.SetHUDOpacity(0);

            this.trigger = false;
        }
    }

    private IEnumerator PlayAnimation(float delay)
    {
        playerState.swapable = false;

        yield return new WaitForSeconds(delay);

        anim.enabled = true; // Takes 5 seconds
        
        yield return new WaitForSeconds(5);

        sunEffect.SetActive(true);

        float power = 60;
        while (power < 90) {

            ChangeMatPower(power);
            power += Time.deltaTime * 4;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transition.Play();

        yield return new WaitForSeconds(0.25f);

        power = 60;
        ChangeMatPower(power);

        sunEffect.SetActive(false);

        playerState.swapable = true;
    }

    private void ChangeMatPower(float power)
    {
        crystalsMat.SetFloat("_FinalPower", power);

        Color yellowGlow = new Color(1.5f, 1.02f, 0.25f);
        sunGlowMat.SetColor("_EmissionColor", yellowGlow * (0.06f * power - 1.9f)); // f(x) = (0.06f * power - 1.9f)
    }

    private IEnumerator MoveWithDelay(Vector3 posA, Vector3 posB, float delay)
    {

        float counter = 0f;
        while (counter < delay)
        {
            Vector3 move = Vector3.Lerp(posA, posB, counter / delay);
            transform.position = move;

            counter += Time.deltaTime;


            yield return null;
        }

        transform.position = posB;

        yield return null;
    }

    // Quaternions > Euler
    private IEnumerator RotateWithDelay(Quaternion rotA, Quaternion rotB, float delay)
    {

        float counter = 0f;
        while (counter < delay)
        {
            
            Quaternion delta = Quaternion.Lerp(rotA, rotB, counter / delay);
            transform.rotation = delta;

            counter += Time.deltaTime;


            yield return null;
        }

        transform.rotation = rotB;

        yield return null;
    }


    public void Trigger()
    {
        this.trigger = true;
    }
}
