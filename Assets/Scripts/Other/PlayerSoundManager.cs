using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip shieldHitSound;
    [SerializeField] private AudioClip matchEndSound;
    [SerializeField] private AudioSource source;

    // Method to play the hit sound
    public void PlayHitSound()
    {
        PlaySound(hitSound);
    }

    // Method to play the shield hit sound
    public void PlayShieldHitSound()
    {
        PlaySound(shieldHitSound);
    }

    // Method to play the match end sound
    public void PlayMatchEndSound()
    {
        PlaySound(matchEndSound);
    }

    // Private method to play a given sound clip
    private void PlaySound(AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
        }
    }
}
