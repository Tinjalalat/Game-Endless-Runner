using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Perbaikan: sceneManagement -> SceneManagement

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer; // Perbaikan: AudioMixer -> audioMixer (menghindari duplikasi nama tipe data)
    public Slider musicSlider;    // Perbaikan: MusicSlider -> musicSlider (lower camelCase standar parameter/field)
    public Slider sfxSlider;      // Perbaikan: SfxSlider -> sfxSlider

    private void Start()
    {
        LoadVolume();
        Time.timeScale = 1;

        // Memainkan music
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MusicManager.Instance.PlayMusic("MainMenu"); // Perbaikan: instance -> Instance
        }
        else if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            MusicManager.Instance.PlayMusic("GamePlay"); // Perbaikan: PlaiMusic -> PlayMusic, Instance -> Instance
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GamePlay");
        MusicManager.Instance.PlayMusic("GamePlay");
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVol", volume); // Perbaikan: AudioMixer -> audioMixer
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", volume); // Perbaikan: AudioMixer -> audioMixer, Setfloat -> SetFloat
    }

    public void SaveVolume()
    {
        float musicVolume;
        audioMixer.GetFloat("MusicVol", out musicVolume); // Perbaikan: AudioMixer -> audioMixer
        PlayerPrefs.SetFloat("MusicVol", musicVolume);

        float sfxVolume;
        audioMixer.GetFloat("SFXVol", out sfxVolume); // Perbaikan: AudioMixer.getFloat -> audioMixer.GetFloat
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol"); // Perbaikan: MusicSlider -> musicSlider
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");     // Perbaikan: SfxSlider -> sfxSlider
    }

    public void Quit()
    {
        DeletePlayerName();
        Application.Quit();
    }

    private void DeletePlayerName()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            PlayerPrefs.DeleteKey("PlayerName");
            PlayerPrefs.Save();
            Debug.Log("Player name delete from PlayerPrefs on game exit.");
        }
    }
}