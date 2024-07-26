using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip shieldHitSound;
    [SerializeField] private AudioClip matchEndSound;
    [SerializeField] private AudioSource source;

    private static SoundFXManager instance;

    protected virtual void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.Stop();
            source.clip = clip;
            source.Play();
        }
    }

    public static void PlayHitEffect()
    {
        if (instance != null)
        {
            instance.PlayClip(instance.hitSound);
        }
    }

    public static void PlayShieldHitSound()
    {
        if (instance != null)
        {
            instance.PlayClip(instance.shieldHitSound);
        }
    }

    public static void PlayMatchEndSound()
    {
        if (instance != null)
        {
            instance.PlayClip(instance.matchEndSound);
        }
    }

    public static void SetFXVolume(float volume)
    {
        if (instance != null && instance.source != null)
        {
            instance.source.volume = volume;
        }
    }
}
