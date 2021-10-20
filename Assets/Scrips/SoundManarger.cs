using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManarger : MonoBehaviour
{
    public static SoundManarger instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public AudioSource efxSource, musicSource;
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    } 
    public void ChangerMusicSource(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
}
