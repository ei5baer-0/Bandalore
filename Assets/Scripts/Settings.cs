using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider[] soundSlider;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    
    void Start()
    {
        soundSlider[0].value = PlayerPrefs.GetFloat("MasterVolume", 100);
        soundSlider[1].value = PlayerPrefs.GetFloat("EffectVolume", 100);
        soundSlider[2].value = PlayerPrefs.GetFloat("MusicVolume", 100);

        soundSlider[0].onValueChanged.AddListener(SetMasterVolume);
        soundSlider[1].onValueChanged.AddListener(SetEffectVolume);
        soundSlider[2].onValueChanged.AddListener(SetMusicVolume);

        SetMasterVolume(soundSlider[0].value);
        SetEffectVolume(soundSlider[1].value);
        SetMusicVolume(soundSlider[2].value);

        SetQuality(PlayerPrefs.GetInt("Quality"));
        bool isFullscreen = (PlayerPrefs.GetInt("Fullscreen") != 0);
        RefreshFullScreen(isFullscreen);
    }

    public void SetMasterVolume(float _soundvalue)
    {
        if (_soundvalue < 1) _soundvalue = .001f;
        masterMixer.SetFloat("Master", Mathf.Log(_soundvalue / 100) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", _soundvalue);
        PlayerPrefs.Save();
    }

    public void SetEffectVolume(float _soundvalue)
    {
        if (_soundvalue < 1) _soundvalue = .001f;
        masterMixer.SetFloat("PlayerEffects", Mathf.Log(_soundvalue / 100) * 20f);
        masterMixer.SetFloat("PlayerSteps", Mathf.Log(_soundvalue / 100) * 20f);
        PlayerPrefs.SetFloat("EffectVolume", _soundvalue);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float _soundvalue)
    {
        if (_soundvalue < 1) _soundvalue = .001f;
        masterMixer.SetFloat("Music", Mathf.Log(_soundvalue / 100) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", _soundvalue);
        PlayerPrefs.Save();
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
