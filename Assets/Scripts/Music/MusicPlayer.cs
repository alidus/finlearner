using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

/// <summary>
/// Component that play music with Playlists
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    private AudioSource audioSource;
    private AudioClip playingClip;
    private MusicPlaylist playingPlaylist;


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
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        if (!GetComponent<AudioSource>())
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (playingPlaylist)
            {
                Play(playingPlaylist);
            }
        }
    }

    public void Play(MusicPlaylist playlist, AudioClip clip = null)
    {
        if (playlist)
        {
            playingPlaylist = playlist;
            StartCoroutine(PlayCoroutine(clip != null ? clip : playlist.GetRandomClip()));
        }
    }

    private IEnumerator PlayCoroutine(AudioClip clip)
    {
        if (audioSource)
        {
            yield return PauseCoroutine();

            playingClip = clip;
            audioSource.clip = playingClip;
            audioSource.Play();
            yield return FadeInVolume();
        }
    }

    public void Pause(MusicPlaylist musicPlaylist)
    {
        StartCoroutine(PauseCoroutine());
    }

    private IEnumerator PauseCoroutine()
    {
        if (audioSource)
        {
            yield return FadeOutVolume();
            audioSource.Pause();
        }
    }


    private IEnumerator FadeOutVolume(float rate = 1.3f, float newVolume = 0)
    {
        if (audioSource)
        {
            while (audioSource.volume > newVolume)
            {
                audioSource.volume -= rate * Time.deltaTime;
                yield return null;
            }
        }
    }
    private IEnumerator FadeInVolume(float rate = 0.2f, float newVolume = 1)
    {
        if (audioSource)
        {
            while (audioSource.volume < newVolume)
            {
                audioSource.volume += rate * Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator SetVolume(float rate, float newVolume)
    {
        if (audioSource)
        {
            if (audioSource.volume < newVolume)
            {
                yield return FadeInVolume(rate, newVolume);
            }
            else
            {
                yield return FadeOutVolume(rate, newVolume);
            }
        }
    }
}
