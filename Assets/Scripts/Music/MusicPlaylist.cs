using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playlists for MusicPlayer
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Music/MusicPlaylist", fileName = "MusicPlaylist")]
public class MusicPlaylist : ScriptableObject
{
	[SerializeField]
    List<AudioClip> playlist = new List<AudioClip>();


    public AudioClip GetRandomClip()
    {
        return playlist[UnityEngine.Random.Range(0, playlist.Count)];
    }
}
