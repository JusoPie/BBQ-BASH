using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip matchStartSound;
    [SerializeField] private AudioSource source;

    private static MenuSFXManager instance;

    private void Awake()
    {
        // Implement singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to play button click sound
    public static void PlayButtonClickSound()
    {
        if (instance != null && instance.source != null && instance.buttonClickSound != null)
        {
            instance.source.PlayOneShot(instance.buttonClickSound);
        }
        else
        {
            Debug.LogWarning("MenuSFXManager instance or audio clip/source not set.");
        }
    }

    // Method to play match start sound
    public static void PlayMatchStartSound()
    {
        if (instance != null && instance.source != null && instance.matchStartSound != null)
        {
            instance.source.PlayOneShot(instance.matchStartSound);
        }
        else
        {
            Debug.LogWarning("MenuSFXManager instance or audio clip/source not set.");
        }
    }

    public static void SetMenuFXVolume(float volume)
    {
        if (instance != null && instance.source != null)
        {
            instance.source.volume = volume;
        }
    }
}

