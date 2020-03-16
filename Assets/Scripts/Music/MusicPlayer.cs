using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that play music with Playlists
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private MusicPlaylist mainMenuMusicPlaylist;
    public MusicPlaylist MainMenuMusicPlaylist
    {
        get { return mainMenuMusicPlaylist; }
        set { mainMenuMusicPlaylist = value; }
    }
    [SerializeField]
    private MusicPlaylist gameplayMusicPlaylist;
    public MusicPlaylist GameplayMusicPlaylist
    {
        get { return gameplayMusicPlaylist; }
        set { gameplayMusicPlaylist = value; }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void Play(MusicPlaylist musicPlaylist)
    {
        musicPlaylist.Play(GetComponent<AudioSource>());
    }

    public void Pause(MusicPlaylist musicPlaylist)
    {
        musicPlaylist.Pause(GetComponent<AudioSource>());
    }

    public void NextTrack(MusicPlaylist musicPlaylist)
    {
        musicPlaylist.NextTrack(GetComponent<AudioSource>());
    }
}
