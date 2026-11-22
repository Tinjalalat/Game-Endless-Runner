using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    [System.Serializable]
    public struct MusicTrack
    {
        public string trackName;
        public AudioClip clip;
    }

    public MusicTrack[] tracks;

    public AudioClip GetClipFromName(string name)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == name)
            {
                return track.clip;
            }
        }
        return null;
    }
}