using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Manager Attributes")] 
    public AudioSource bgmSource;

    [Space(10)] 
    public AudioSource sfxSource;

    [Header("SFX Array")] 
    public AudioClip[] sfxOptions;

    public void StartBGM()
    {
        bgmSource.Play();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StartSFX()
    {
        sfxSource.Stop();

        AudioClip newSFX = ReturnRandomSFX(sfxOptions);
        sfxSource.clip = newSFX;
     
        sfxSource.PlayOneShot(newSFX);
    }

    public void PauseSFX()
    {
        sfxSource.Pause();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    private AudioClip ReturnRandomSFX(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length - 1);

        return clips[randomIndex];
    }
}
