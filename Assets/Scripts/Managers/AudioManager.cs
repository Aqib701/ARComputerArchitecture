using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    AudioSource _audioSource;
    
    private void Start()
    {
        instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayComponentAudio(AudioClip componentAudio )
    {
        if (componentAudio) _audioSource.PlayOneShot(componentAudio);
        
        else StopComponentAudio();
    }
    
    public void StopComponentAudio()
    {
        _audioSource.Stop();
    }
}
