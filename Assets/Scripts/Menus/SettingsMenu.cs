using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider generalSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] TMP_Dropdown  resolutionsDropDowns;

    public const string MIXER_GENERAL = "MainGeneralVolume";
    public const string MIXER_MUSIC = "MainMusicVolume";
    public const string MIXER_SOUND = "MainSoundVolume";

    Resolution[] resolutions;

    public void Awake()
    {
        resolutions = Screen.resolutions;
        resolutionsDropDowns.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionsDropDowns.AddOptions(options);
        resolutionsDropDowns.value = currentResolutionIndex;
        resolutionsDropDowns.RefreshShownValue();

        generalSlider.onValueChanged.AddListener(SetGeneralVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    public void Start()
    {
        generalSlider.value = PlayerPrefs.GetFloat(SettingsManager.GENERAL_KEY, 0.9f);
        musicSlider.value = PlayerPrefs.GetFloat(SettingsManager.MUSIC_KEY, 0.6f);
        soundSlider.value = PlayerPrefs.GetFloat(SettingsManager.SOUND_KEY, 0.8f);
    }

    public void OnDisable()
    {
        PlayerPrefs.SetFloat(SettingsManager.GENERAL_KEY, generalSlider.value);
        PlayerPrefs.SetFloat(SettingsManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(SettingsManager.SOUND_KEY, soundSlider.value);
    }

    public void SetGeneralVolume (float value)
    {
        audioMixer.SetFloat("MainGeneralVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume (float value)
    {
        //audioMixer.SetFloat("MainMusicVolume", value);
        audioMixer.SetFloat("MainMusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSoundVolume (float value)
    {
        audioMixer.SetFloat("MainSoundVolume", Mathf.Log10(value) * 20);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVsync (bool isVsync)
    {
        QualitySettings.vSyncCount = isVsync ? 1 : 0;
    }
}
