using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider generalSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] TMP_Dropdown  resolutionsDropDowns;
    [SerializeField] TMP_Dropdown  qualityDropDowns;
    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] Toggle vSyncToggle;

    [SerializeField] AudioSource audioSource;

    public const string MIXER_GENERAL = "MainGeneralVolume";
    public const string MIXER_MUSIC = "MainMusicVolume";
    public const string MIXER_SOUND = "MainSoundVolume";
    public const string SET_QUALITY = "MainQuality";
    public const string SET_RESOLUTION_INDEX = "MainResolutionIndex";
    public const string SET_FULLSCREEN = "MainFullscreen";
    public const string SET_VSYNC = "MainVSync";

    Resolution[] resolutions;
    int saveResolutionIndex = 0;
    int currentResolutionIndex = 0;

    int fullScreen = 1;
    int vSync = 0;

    void Awake()
    {
        resolutions = Screen.resolutions;
        resolutionsDropDowns.ClearOptions();

        List<string> options = new List<string>();

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

    void Start()
    {
        generalSlider.value = PlayerPrefs.GetFloat(SettingsManager.GENERAL_KEY, 0.9f);
        musicSlider.value = PlayerPrefs.GetFloat(SettingsManager.MUSIC_KEY, 0.6f);
        soundSlider.value = PlayerPrefs.GetFloat(SettingsManager.SOUND_KEY, 0.8f);
        
        qualityDropDowns.value = PlayerPrefs.GetInt(SettingsManager.QUALITY_KEY, QualitySettings.GetQualityLevel());

        saveResolutionIndex = PlayerPrefs.GetInt(SettingsManager.RESOLUTION_INDEX_KEY, currentResolutionIndex);
        resolutionsDropDowns.value = saveResolutionIndex;

        fullScreenToggle.isOn = PlayerPrefs.GetInt(SettingsManager.FULLSCREEN_KEY, Screen.fullScreen ? 1 : 0) == 1;
        vSyncToggle.isOn = PlayerPrefs.GetInt(SettingsManager.VSYNC_KEY, QualitySettings.vSyncCount > 0 ? 1 : 0) == 1;
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(SettingsManager.GENERAL_KEY, generalSlider.value);
        PlayerPrefs.SetFloat(SettingsManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(SettingsManager.SOUND_KEY, soundSlider.value);

        PlayerPrefs.SetInt(SettingsManager.QUALITY_KEY, QualitySettings.GetQualityLevel());
        PlayerPrefs.SetInt(SettingsManager.RESOLUTION_INDEX_KEY, saveResolutionIndex);
        PlayerPrefs.SetInt(SettingsManager.FULLSCREEN_KEY, fullScreen);
        PlayerPrefs.SetInt(SettingsManager.VSYNC_KEY, vSync);
        
        PlayerPrefs.Save();
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

    public void OnSliderEndDrag()
    {
        audioSource.Play();
    }

    public void SetQuality(int qualityIndex)
    {
        audioSource.Play();
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution (int resolutionIndex)
    {
        audioSource.Play();
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        saveResolutionIndex = resolutionIndex;
    }

    public void SetFullScreen (bool isFullscreen)
    {
        audioSource.Play();
        Screen.fullScreen = isFullscreen;
        if(isFullscreen)
            fullScreen = 1;
        else
            fullScreen = 0;
    }

    public void SetVsync (bool isVsync)
    {
        audioSource.Play();
        QualitySettings.vSyncCount = isVsync ? 1 : 0;
        if(isVsync)
            vSync = 1;
        else
            vSync = 0;
    }

    public void MainMenu()
    {
        audioSource.Play();
    }
}
