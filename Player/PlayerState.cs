using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public Image activated;
    public Image deactivated;

    public Material fullscreenWaveMat;
    public ParticleSystem waveEffect;

    public bool swapable = true;
    public bool isParticle = true;

    private const KeyCode key = KeyCode.LeftShift;

    private void Start()
    {
        this.isParticle = true;
        this.swapable = true;

        fullscreenWaveMat.SetFloat("_Radius", 0.2f);

        UI();
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyPressed();

        HandleActiveWave();

        Effects();
    }

    public void Swap()
    {
        if (swapable)
        {
            isParticle = !isParticle;

            UI();
        }
    }

    void HandleKeyPressed()
    {

        if (Input.GetKeyDown(key))
        {

            print(isParticle);
            if (isParticle)
            {
                bool isReady = (deactivated.materialForRendering.GetFloat("_Fill") >= 1);
                if (isReady) { Swap(); }
            }
            else
            {

                Swap();
            }

            
        }
    }

    private void UI()
    {

        deactivated.materialForRendering.SetInt("_isWave", isParticle ? 1 : 0);
        activated.materialForRendering.SetInt("_isWave", isParticle ? 0 : 1);

        float fill = activated.materialForRendering.GetFloat("_Fill");
        deactivated.materialForRendering.SetFloat("_Fill", fill);
        activated.materialForRendering.SetFloat("_Fill", 1);
    }

    private bool isEffectActive = false;
    private void Effects()
    {
        float curRadius = fullscreenWaveMat.GetFloat("_Radius");

        if (isParticle && curRadius > 0.2f)
        {
            fullscreenWaveMat.SetFloat("_Radius", curRadius - (Time.deltaTime * 0.3f));
        }
        else if (!isParticle && curRadius < 0.9f)
        {
            fullscreenWaveMat.SetFloat("_Radius", curRadius + (Time.deltaTime * 0.6f));
        }


        if (!isParticle) {
            if (!isEffectActive) { waveEffect.Play(); isEffectActive = true; print(isEffectActive); }
        }
        else { waveEffect.Stop(); isEffectActive = false; }
    }


    private void HandleActiveWave()
    {
        if (!isParticle)
        {
            float timeRemain = activated.materialForRendering.GetFloat("_Fill");

            if (timeRemain <= 0)
            {
                Swap();

                return;
            }

            float newValue = timeRemain - (Time.deltaTime * 0.15f);
            activated.materialForRendering.SetFloat("_Fill", newValue);
        }
        else
        {
            float fill = deactivated.materialForRendering.GetFloat("_Fill");

            if (fill < 1)
            {
                float newValue = fill + (Time.deltaTime * 0.15f);
                deactivated.materialForRendering.SetFloat("_Fill", newValue);
            }
        }
    }

    public void HandleTeleport()
    {
        activated.materialForRendering.SetFloat("_Fill", 0);
    }
}
