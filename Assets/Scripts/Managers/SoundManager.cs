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
    // Start is called before the first frame update
    public void Play()
    {
        _audioSource.clip = audio;
        _audioSource.Play();
    }

}
