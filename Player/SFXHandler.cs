using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    WAVEMODE = 0,
    JUMP = 1,
    TP1 = 2,
    TP2 = 3,
    TP3 = 4,
    SOLARMOD = 5
}

public class SFXHandler : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip[] clips;

    public void Play(AudioClip clip, float volume = 0.25f) {
        this.audioSource.volume = volume;
        this.audioSource.PlayOneShot(clip);
    }

    public void Play(AudioSource source, AudioClip clip, float volume = 0.25f) {

        source.volume = volume;
        source.PlayOneShot(clip);
    }

    public void Stop() {
        this.audioSource.Stop();
    }

    public void Stop(AudioSource source)
    {
        source.Stop();
    }
}
