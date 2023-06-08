using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject mainMenu;
    private SettingsMenu settingsMenu;
    void Awake()
    {
        SettingsManager.instance.PlayMenuAudioClip();
        // SettingsManager.instance.LoadVolume();
        // SettingsManager.instance.LoadSettings();
        settingsMenu = GameObject.Find("SettingsMenu").GetComponent<SettingsMenu>();
    }

    void Start()
    {
        settingsMenu.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        // SceneManager.LoadScene(1);
        // SceneManager.LoadScene("TestScene");
        SettingsManager.instance.ChangeAudioClip();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SettingsMenu()
    {
        settingsMenu.gameObject.SetActive(true); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
