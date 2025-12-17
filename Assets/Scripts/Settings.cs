using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public Slider soundSlider;
    public AudioMixer masterMixer;
    public TMP_Dropdown graphicsDropdown;
    public Toggle fullscreenToggle;

    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
        SetQuality(PlayerPrefs.GetInt("SavedQuality"));
        bool isFullscreen = (PlayerPrefs.GetInt("SavedFullscreen") != 0);
        RefreshFullScreen(isFullscreen);
    }

    public void SetVolume(float soundvalue)
    {
        if (soundvalue < 1)
        {
            soundvalue = .001f;
        }

        RefreshSlider(soundvalue);
        PlayerPrefs.SetFloat("SavedMasterVolume", soundvalue);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(soundvalue / 100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void RefreshSlider(float soundvalue)
    {
        soundSlider.value = soundvalue;
    }

    public void SetQuality(int qualityInt)
    {
        RefreshQuality(qualityInt);
        PlayerPrefs.SetInt("SavedQuality", qualityInt);
    }

    public void RefreshQuality(int qualityInt)
    {
        graphicsDropdown.value = qualityInt;
    }

    public void SetFullScreen(bool isFullscreen)
    {
        RefreshFullScreen(isFullscreen);
        PlayerPrefs.SetInt("SavedFullscreen", isFullscreen ? 1 : 0);
    }

    public void RefreshFullScreen(bool isFullscreen)
    {
        fullscreenToggle.SetIsOnWithoutNotify(isFullscreen);
        Screen.fullScreen = isFullscreen;

    }
}
