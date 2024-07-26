using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip shieldHitSound;
    [SerializeField] private AudioClip matchEndSound;
    [SerializeField] private AudioSource source;

    private static SoundFXManager instance;
    private float pitchVariance = 0.1f; // Pitch variance range

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

    private void PlayClip(AudioClip clip, bool varyPitch = false)
    {
        if (source != null && clip != null)
        {
            if (varyPitch)
            {
                source.pitch = 1.0f + Random.Range(-pitchVariance, pitchVariance);
            }
            else
            {
                source.pitch = 1.0f; // Reset to default pitch
            }

            source.Stop();
            source.clip = clip;
            source.Play();
        }
    }

    public static void PlayHitEffect()
    {
        if (instance != null)
        {
            instance.PlayClip(instance.hitSound, true);
        }
    }

    public static void PlayShieldHitSound()
    {
        if (instance != null)
        {
            instance.PlayClip(instance.shieldHitSound, true);
        }
    }

    public static void PlayMatchEndSound()
    {
        if (instance != null)
        {
            instance.PlayClip(instance.matchEndSound, false); // No pitch variation
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
