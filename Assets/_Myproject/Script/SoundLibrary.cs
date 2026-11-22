using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    [System.Serializable]
    public struct SoundEffect
    {
        public string groupID;
        public AudioClip[] clips;
    }

    public SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string name)
    {
        foreach (var effect in soundEffects)
        {
            if (effect.groupID == name && effect.clips.Length > 0)
            {
                int randomIndex = Random.Range(0, effect.clips.Length);
                return effect.clips[randomIndex]; // Mengambil secara acak dari kelompok tertentu
            }
        }
        return null;
    }
}