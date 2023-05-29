using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource pressSource;

    public const string GENERAL_KEY = "MainGeneralVolume";
    public const string MUSIC_KEY = "MainMusicVolume";
    public const string SOUND_KEY = "MainSoundVolume";

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
        LoadVolume();
    }

    public void PressSound()
    {
        
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
}
