using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SoundFXManager : NetworkBehaviour
{
    
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private AudioClip shieldHitSound;

    [SerializeField] private AudioClip matchEndSound;

    [SerializeField] private AudioSource source;

    /// This class follows the singleton pattern and this is its instance
    static private SoundFXManager instance;

    /// Awake is not public because other scripts have no reason to call it directly,
    /// only the Unity runtime does (and it can call protected and private methods).
    /// It is protected virtual so that possible subclasses may perform more specific
    /// tasks in their own Awake and still call this base method (It's like constructors
    /// in object-oriented languages but compatible with Unity's component-based stuff.
    protected virtual void Awake()
    {
        // Singleton enforcement
        if (instance == null)
        {
            // Register as singleton if first
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            // Self-destruct if another instance exists
            Destroy(this);
            return;
        }
    }


    
    static public void PlayHitEffect()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                instance.source.Stop();
                instance.source.clip = instance.hitSound;
                instance.source.Play();
            }
        }
        else
        {
            Debug.LogError("Unavailable SoundFXManager component");
        }
    }


    static public void PlayDingDingDing()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                instance.source.Stop();
                instance.source.clip = instance.matchEndSound;
                instance.source.Play();
            }
        }
        else
        {
            Debug.LogError("Unavailable MusicPlayer component");
        }
    }

    static public void PlayShieldHitSound()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                instance.source.Stop();
                instance.source.clip = instance.shieldHitSound;
                instance.source.Play();
            }
        }
        else
        {
            Debug.LogError("Unavailable MusicPlayer component");
        }
    }


    /// Sets the volume of the audio source.
    /// <param name="volume">Volume level between 0 and 1</param>
    static public void SetFXVolume(float volume)
    {
        if (instance != null && instance.source != null)
        {
            instance.source.volume = volume;
        }
    }
}

