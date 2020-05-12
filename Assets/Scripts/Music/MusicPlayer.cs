using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

/// <summary>
/// Component that plays music using Playlists
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    public AudioSource AudioSource;
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
    private float targetVolume = 0.8f;
    Coroutine adjustVolumeCoroutine;

    /// <summary>
    /// Change this property to automatically smooth adjust audio source volume
    /// </summary>
    public float TargetVolume { get => targetVolume; set
        {
            if (value != targetVolume)
            {
                targetVolume = value;
                if (targetVolume != AudioSource.volume)
                {
                    if (adjustVolumeCoroutine != null)
                    {
                        StopCoroutine(adjustVolumeCoroutine);
                    }
                    adjustVolumeCoroutine = StartCoroutine(AdjustVolume(targetVolume));
                }
            }
        }
    }



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
        if (AudioSource == null)
        {
            AudioSource = gameObject.GetComponent<AudioSource>();
        }
        if (AudioSource)
            AudioSource.volume = 0;
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
        if (AudioSource)
        {
            yield return PauseCoroutine();

            playingClip = clip;
            AudioSource.clip = playingClip;
            AudioSource.Play();
            yield return AdjustVolume(targetVolume);
        }
    }

    public void Pause(MusicPlaylist musicPlaylist)
    {
        StartCoroutine(PauseCoroutine());
    }

    private IEnumerator PauseCoroutine()
    {
        if (AudioSource)
        {
            yield return AdjustVolume(0);
            AudioSource.Pause();
        }
    }


    IEnumerator AdjustVolume(float targetVolume, float rate = 1.1f)
    {
        
        if (AudioSource.volume > targetVolume)
        {
            while (AudioSource.volume > targetVolume)
            {
                AudioSource.volume -= rate * Time.deltaTime;
                yield return 0;
            }
        }
        else
        {
            while (AudioSource.volume < targetVolume)
            {
                AudioSource.volume += rate * Time.deltaTime;
            yield return 1;
            }
        }
    }
}
