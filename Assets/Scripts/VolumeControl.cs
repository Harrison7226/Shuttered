using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the Audio Mixer
    public Slider volumeSlider;  // Reference to the UI Slider

    private static VolumeControl instance;

    void Awake()
    {
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

    void Start()
    {
        // Set slider value to match current volume at start
        float currentVolume;
        audioMixer.GetFloat("Master", out currentVolume);
        volumeSlider.value = Mathf.Pow(10, currentVolume / 20); // Convert dB to linear scale

        // Add listener to update volume when slider changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

public void SetVolume(float value)
{
    // Clamp the value to avoid zero (use a small value like 0.0001)
    value = Mathf.Clamp(value, 0.0001f, 1f);

    // Convert slider value to decibels
    float volume = Mathf.Log10(value) * 20;

    // Set the volume in the Audio Mixer
    audioMixer.SetFloat("Master", volume);
}

}