using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioMixer mainAudioMixer;


    public float GetMasterVolume()
    {
        mainAudioMixer.GetFloat("masterVolume", out float value);
        return value;
    }

    public float GetMusicVolume()
    {
        mainAudioMixer.GetFloat("musicVolume", out float value);
        return value;
    }

    public float GetSFXVolume()
    {
        mainAudioMixer.GetFloat("sfxVolume", out float value);
        return value;
    }


    public void SetMasterVolume(float value)
    {
        mainAudioMixer.SetFloat("masterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        mainAudioMixer.SetFloat("musicVolume", value);

    }

    public void SetSFXVolume(float value)
    {
        mainAudioMixer.SetFloat("sfxVolume", value);
    }



}
