using System;
using UnityEngine;

public class AudioMN : MonoBehaviour
{
    public static AudioMN Instance;

    public AudioSource audioSource;

    void Awake()
    {
        Instance = this;
    }

    public float GetTime()
    {
        return audioSource.time;
    }

    public void PlaySong(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.time = 0f;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopSong()
    {
        audioSource.Stop();
    }
}