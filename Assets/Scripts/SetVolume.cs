using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{

    public AudioMixer mixer;

    public void SetVolumeLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue)*20);
    }
    public void SetVolumeToZero(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", 0);
    }
    public void SetVolumeToOne(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", 1);
    }

}
