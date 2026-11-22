using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public SoundLibrary soundLibrary;
    public AudioMixerGroup sfxMixerGroup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound2D(string soundName)
    {
        AudioClip clip = soundLibrary.GetClipFromName(soundName);
        if (clip != null)
        {
            AudioSource source = AudioSourcePool.Instance.GetAudioSource();
            source.spatialBlend = 0f; // 2D Sound
            source.clip = clip;
            source.outputAudioMixerGroup = sfxMixerGroup;
            source.Play();
            StartCoroutine(ReturnToPoolAfterPlayback(source, clip.length));
        }
    }

    public void PlaySound3D(string soundName, Vector3 position)
    {
        AudioClip clip = soundLibrary.GetClipFromName(soundName);
        if (clip != null)
        {
            AudioSource source = AudioSourcePool.Instance.GetAudioSource();
            source.transform.position = position;
            source.spatialBlend = 1f; // 3D Sound
            source.clip = clip;
            source.outputAudioMixerGroup = sfxMixerGroup;
            source.Play();
            StartCoroutine(ReturnToPoolAfterPlayback(source, clip.length));
        }
    }

    private IEnumerator ReturnToPoolAfterPlayback(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        AudioSourcePool.Instance.ReturnAudioSource(source);
    }
}