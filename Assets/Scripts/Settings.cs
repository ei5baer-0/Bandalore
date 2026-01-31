using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider soundSlider;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Animator animatorVolumeSettings;
    [SerializeField] private Animator animatorVolumeMenu;

    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
        VolumeIcon(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
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
        VolumeIcon(soundvalue);
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

    public void VolumeIcon(float soundvalue)
    {
        if (soundvalue < 1)
        {
            animatorVolumeSettings.Play("Off");
            animatorVolumeMenu.Play("Off");
        }
        else if (soundvalue < 35)
        {
            animatorVolumeSettings.Play("Low");
            animatorVolumeMenu.Play("Low");
        }
        else if (soundvalue < 75)
        {
            animatorVolumeSettings.Play("Mid");
            animatorVolumeMenu.Play("Mid");
        }
        else
        {
            animatorVolumeSettings.Play("High");
            animatorVolumeMenu.Play("High");
        }
    }
}
