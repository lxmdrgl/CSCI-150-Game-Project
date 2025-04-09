using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private AudioSource musicSource;
    private AudioSource[] sfxSources;

    void Start()
    {
        // Find the music AudioSource
        musicSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        // Find all SFX AudioSources
        GameObject[] sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
        sfxSources = new AudioSource[sfxObjects.Length];
        for (int i = 0; i < sfxObjects.Length; i++)
        {
            sfxSources[i] = sfxObjects[i].GetComponent<AudioSource>();
        }

        // Add listeners to sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        foreach (var sfx in sfxSources)
        {
            if (sfx != null)
                sfx.volume = volume;
        }
    }
}
