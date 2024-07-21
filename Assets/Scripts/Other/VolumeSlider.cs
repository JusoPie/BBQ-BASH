using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        // Load saved volume level
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        MusicPlayer.SetVolume(savedVolume);

        // Add a listener to handle changes to the slider's value
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        MusicPlayer.SetVolume(volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }
}

