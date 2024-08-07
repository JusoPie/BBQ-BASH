using UnityEngine;
using System.Collections;

/// <summary>
/// This script is in charge of playing music in the game
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    /// The clip to play in a menu.
    /// This field is private because it's not designed to be directly
    /// modified by other scripts, and tagged with [SerializeField] so that
    /// you can still modify it using the Inspector and so that Unity
    /// saves its value.
    [SerializeField] private AudioClip menuMusic;

    /// The clip to play outside menus.
    [SerializeField] private AudioClip levelMusic;

    /// The component that plays the music
    [SerializeField] private AudioSource source;

    /// This class follows the singleton pattern and this is its instance
    static private MusicPlayer instance;

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

    protected virtual void Start()
    {
        // If the game starts in a menu scene, play the appropriate music
        PlayMenuMusic();
    }

    /// Plays the music designed for the menus
    /// This method is static so that it can be called from anywhere in the code.
    static public void PlayMenuMusic()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                instance.source.Stop();
                instance.source.clip = instance.menuMusic;
                instance.source.Play();
            }
        }
        else
        {
            Debug.LogError("Unavailable MusicPlayer component");
        }
    }

    /// Plays the music designed for outside menus
    /// This method is static so that it can be called from anywhere in the code.
    static public void PlayGameMusic()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                instance.source.Stop();
                instance.source.clip = instance.levelMusic;
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
    static public void SetVolume(float volume)
    {
        if (instance != null && instance.source != null)
        {
            instance.source.volume = volume;
        }
    }
}
