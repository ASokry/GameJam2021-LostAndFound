using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] protected GameObject StartMenu;
    [SerializeField] protected GameObject SettingMenu;

    [Header("Setting values")]
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;

    [Header("Sound")]
    [SerializeField] AudioClip clickButtonSound;
    [SerializeField] AudioClip hoverButtonSound;
    [SerializeField] AudioClip startMenuSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        InitiateMenus();
        InitiateSettings();
        Time.timeScale = 1;
    }

    protected virtual void OnEnable()
    {
        //audioSource.PlayOneShot(startMenuSound);
    }    

    protected virtual void InitiateMenus()
    {
        StartMenu.SetActive(true);
        SettingMenu.SetActive(false);
    }    

    protected virtual void InitiateSettings()
    {
        masterVolume.minValue = 0;
        masterVolume.maxValue = 1;

        musicVolume.minValue = 0;
        musicVolume.maxValue = 1;

        sfxVolume.minValue = 0;
        sfxVolume.maxValue = 1;
    }

    public void ChangeMasterVolume(float value)
    {
        SoundManager.Instance.SetMasterVolume(ValueToDecibels(value));
    }

    public void ChangeMusicVolume(float value)
    {
        SoundManager.Instance.SetMusicVolume(ValueToDecibels(value));
    }

    public void ChangeSFXVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(ValueToDecibels(value));
    }

    public void PlayGame()
    {

        GameManager.Instance.LoadScene("Level_test_1", GameManager.GameState.Playing);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        GameManager.Instance.LoadScene("MainMenu", GameManager.GameState.MainMenu);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickButtonSound);
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverButtonSound);
    }

    float ValueToDecibels(float value)
    {
        return Mathf.Clamp(10 * Mathf.Log10(value), -192, 0);
    }
}
