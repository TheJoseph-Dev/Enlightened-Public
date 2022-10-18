using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;


public class OptionsSettings : MonoBehaviour
{
    public AudioMixer mainMixer;

    public Slider VolumeOption;
    public Slider MouseSensibilityOption;
    public Toggle InvertMouseOption;
    public TMP_Dropdown QualityOption;

    private void Start()
    {
        VolumeOption.value = GameSettings.volume;
        MouseSensibilityOption.value = GameSettings.mouseSensibility;
        InvertMouseOption.isOn = GameSettings.invertMouse;
        QualityOption.value = (int)GameSettings.quality;
    }

    public void OnVolume(float value)
    {
        GameSettings.volume = value;
        mainMixer.SetFloat("Volume", GameSettings.volume);
    }

    public void OnMouseSense(float value)
    {
        GameSettings.mouseSensibility = value;
    }

    public void OnInvertMouse(bool value)
    {
        GameSettings.invertMouse = value;
    }

    public void OnGraphics(int index)
    {
        GameSettings.quality = (uint)index;

        QualitySettings.SetQualityLevel(index);
    }

    /*public void OnVSyncCount(int count) {
        QualitySettings.vSyncCount = count;
    }*/


    public void Reseter()
    {
        GameSettings.volume = 0f;
        GameSettings.mouseSensibility = 0.5f;
        GameSettings.invertMouse = false;
        GameSettings.VSync = true;
        GameSettings.quality = 3;

        VolumeOption.value = GameSettings.volume;
        MouseSensibilityOption.value = GameSettings.mouseSensibility;
        InvertMouseOption.isOn = GameSettings.invertMouse;
        QualityOption.value = (int)GameSettings.quality;
    }
}
