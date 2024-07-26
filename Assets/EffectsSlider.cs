using UnityEngine;
using UnityEngine.UI;

public class EffectsSlider : MonoBehaviour
{
    public Slider effectSlider;

    private void Start()
    {
        // Load saved volume level
        float savedVolume = PlayerPrefs.GetFloat("MenuFXVolume", 1f);
        effectSlider.value = savedVolume;
        MenuSFXManager.SetMenuFXVolume(savedVolume);

        // Add a listener to handle changes to the slider's value
        effectSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        MenuSFXManager.SetMenuFXVolume(volume);
        PlayerPrefs.SetFloat("MenuFXVolume", volume);
    }
}
