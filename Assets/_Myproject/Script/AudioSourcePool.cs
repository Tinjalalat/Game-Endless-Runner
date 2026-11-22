using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance;
    [SerializeField] private int initialPoolSize = 10;
    private List<AudioSource> pool;

    private void Awake()
    {
        if (Instance != null)
        {
            // Perbaikan typo: 'destroy' menjadi 'Destroy' (kapital)
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Perbaikan typo: 'DontDestruyOnLoad' menjadi 'DontDestroyOnLoad'
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
    }

    private void InitializePool()
    {
        // Perbaikan typo: 'AudioSourced' menjadi 'AudioSource'
        pool = new List<AudioSource>(initialPoolSize);

        // Perbaikan typo: tanda petik ganda di akhir baris dihapus, ganti dengan titik koma (;)
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    private AudioSource CreateNewAudioSource()
    {
        // Perbaikan typo: tipe data 'AudioSourced' menjadi 'AudioSource'
        AudioSource newSource = new GameObject("PooledAudioSource").AddComponent<AudioSource>();
        newSource.gameObject.SetActive(false);
        newSource.transform.SetParent(transform);
        pool.Add(newSource);

        // Perbaikan typo: 'returned' menjadi 'return'
        return newSource;
    }

    public AudioSource GetAudioSource()
    {
        // Perbaikan typo kocak: 'foreahead' menjadi 'foreach'
        foreach (var source in pool)
        {
            if (!source.gameObject.activeInHierarchy)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }

        // Jika tidak ada yang tersedia, buat yang baru
        // Perbaikan typo: 'returner' menjadi 'return'
        return CreateNewAudioSource();
    }

    public void ReturnAudioSource(AudioSource source)
    {
        // Perbaikan typo: fungsi bawaan AudioSource adalah 'Stop()' bukan 'Stoped()'
        source.Stop();
        // Perbaikan typo: 'SetActived' menjadi 'SetActive'
        source.gameObject.SetActive(false);
    }
}