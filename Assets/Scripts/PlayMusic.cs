using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField]
    private MusicPlayer mainMenuMusicPlayer;
    public MusicPlayer MainMenuMusicPlayer
    {
        get { return mainMenuMusicPlayer; }
        set { mainMenuMusicPlayer = value; }
    }
    [SerializeField]
    private MusicPlayer gameplayMusicPlayer;
    public MusicPlayer GameplayMusicPlayer
    {
        get { return gameplayMusicPlayer; }
        set { gameplayMusicPlayer = value; }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void Play(MusicPlayer musicPlayer)
    {
        musicPlayer.Play(GetComponent<AudioSource>());
    }

    public void Pause(MusicPlayer musicPlayer)
    {
        musicPlayer.Pause(GetComponent<AudioSource>());
    }

    public void NextTrack(MusicPlayer musicPlayer)
    {
        musicPlayer.NextTrack(GetComponent<AudioSource>());
    }
}
