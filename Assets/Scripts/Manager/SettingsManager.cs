using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    Resolution[] resolutions;

    [SerializeField] AudioMixer mixer;
    AudioSource audioSource;
    [SerializeField] AudioSource pressSource;
    [SerializeField] AudioClip menuAudioClip;
    [SerializeField] AudioClip[] gameAudioClips;

    public const string GENERAL_KEY = "MainGeneralVolume";
    public const string MUSIC_KEY = "MainMusicVolume";
    public const string SOUND_KEY = "MainSoundVolume";
    public const string QUALITY_KEY = "MainQuality";
    public const string RESOLUTION_INDEX_KEY = "MainResolutionIndex";
    public const string FULLSCREEN_KEY = "MainFullscreen";
    public const string VSYNC_KEY = "MainVSync";

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        LoadVolume();
        LoadSettings();
    }

    public void LoadVolume() //Volume saved in SettingsMenu.cs
    {
        float generalVolume = PlayerPrefs.GetFloat(GENERAL_KEY, 0.9f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 0.6f);
        float soundVolume = PlayerPrefs.GetFloat(SOUND_KEY, 0.8f);
        
        mixer.SetFloat(SettingsMenu.MIXER_GENERAL,Mathf.Log10(generalVolume) * 20);
        mixer.SetFloat(SettingsMenu.MIXER_MUSIC,Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(SettingsMenu.MIXER_SOUND,Mathf.Log10(soundVolume) * 20);
    }

    public void LoadSettings()
    {
        int qualityIndex = PlayerPrefs.GetInt(SettingsMenu.SET_QUALITY, QualitySettings.GetQualityLevel());
        resolutions = Screen.resolutions;
        int resolutionIndex = PlayerPrefs.GetInt(SettingsMenu.SET_RESOLUTION_INDEX, GetDefaultResolutionIndex());
        bool isFullscreen = PlayerPrefs.GetInt(SettingsMenu.SET_FULLSCREEN, Screen.fullScreen ? 1 : 0) == 1;
        bool isVSync = PlayerPrefs.GetInt(SettingsMenu.SET_VSYNC, QualitySettings.vSyncCount > 0 ? 1 : 0) == 0;
    }

    public void PlayMenuAudioClip()
    {
        if (audioSource != null && menuAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = menuAudioClip;
            audioSource.Play();
        }
    }
    public void ChangeAudioClip()
    {
        if (audioSource != null && gameAudioClips != null && gameAudioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, gameAudioClips.Length);
            audioSource.clip = gameAudioClips[randomIndex];
            audioSource.Play();
        }
    }
    
    private int GetDefaultResolutionIndex()
    {
        // Determine the index of the current resolution in the resolutions array
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                return i;
        }
        return 0;
    }
}
