using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour
{
    public Slider soundSlider;

    private void Start()
    {
        // Load saved volume level
        float savedVolume = PlayerPrefs.GetFloat("GameFXVolume", 1f);
        soundSlider.value = savedVolume;
        SoundFXManager.SetFXVolume(savedVolume);

        // Add a listener to handle changes to the slider's value
        soundSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        SoundFXManager.SetFXVolume(volume);
        PlayerPrefs.SetFloat("GameFXVolume", volume);
    }
}
