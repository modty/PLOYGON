using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get => _instance;
        set => _instance = value;
    }

    private void Awake()
    {
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip audio;
    
    [SerializeField]
    private AudioClip arrowImpactflesh;
    [SerializeField]
    private AudioClip arrowFlyingPast;
    [SerializeField]
    private AudioClip pullingStringBack;
    [SerializeField]
    private AudioClip releasingStringBow;
    
    // Start is called before the first frame update
    public void Play()
    {
        _audioSource.clip = audio;
        _audioSource.Play();
    }

    public void PlayArrowImpactflesh()
    {
        _audioSource.clip = arrowImpactflesh;
        _audioSource.Play();
    }
    
    
    public void PlayArrowFlyingPast()
    {
        _audioSource.clip = arrowFlyingPast;
        _audioSource.Play();
    }
    
    public void PlayPullingStringBack()
    {
        _audioSource.clip = pullingStringBack;
        _audioSource.Play();
    }
    
    public void PlayReleasingStringBow()
    {
        _audioSource.clip = releasingStringBow;
        _audioSource.Play();
    }

}
