using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] protected GameObject StartMenu;
    [SerializeField] protected GameObject SettingMenu;

    [Header("Setting values")]
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;

    protected virtual void Start()
    {
        InitiateMenus();
        InitiateSettings();
    }

    protected virtual void InitiateMenus()
    {
        StartMenu.SetActive(true);
        SettingMenu.SetActive(false);
    }    

    protected virtual void InitiateSettings()
    {
        masterVolume.minValue = -20;
        masterVolume.maxValue = 0;

        musicVolume.minValue = -20;
        musicVolume.maxValue = 0;

        sfxVolume.minValue = -20;
        sfxVolume.maxValue = 0;
    }

    public void ChangeMasterVolume(float value)
    {
       SoundManager.Instance.SetMasterVolume(value);
    }

    public void ChangeMusicVolume(float value)
    {
       SoundManager.Instance.SetMusicVolume(value);
    }

    public void ChangeSFXVolume(float value)
    {
       SoundManager.Instance.SetSFXVolume(value);
    }

    public void PlayGame()
    {
        GameManager.Instance.LoadScene("Level_test_1", GameManager.GameState.Playing);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
