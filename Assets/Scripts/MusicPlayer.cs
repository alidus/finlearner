using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Music/MusicPlayer", fileName = "MusicPlayerSO")]
public class MusicPlayer : ScriptableObject
{
	[SerializeField]
    List<AudioClip> playlist = new List<AudioClip>();

    public void Play(AudioSource source)
    {
        if (playlist.Count > 0)
        {
            NextTrack(source);
            source.Play();
        }
    }

    public void Pause(AudioSource source)
    {
        source.Stop();
    }

    internal void NextTrack(AudioSource source)
    {
        source.clip = GetRandomClip();
    }

    public AudioClip GetRandomClip()
    {
        return playlist[UnityEngine.Random.Range(0, playlist.Count - 1)];
    }
}
