using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider generalSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;

    public const string MIXER_GENERAL = "MainGeneralVolume";
    public const string MIXER_MUSIC = "MainMusicVolume";
    public const string MIXER_SOUND = "MainSoundVolume";

    public void Awake()
    {
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
}
