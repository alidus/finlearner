using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public enum GameState
{
    MainMenu, InGame
}

/// <summary>
/// Manage overall game state
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Managers, Controllers
    private UIManager uiManager;
    private GameDataManager gameDataManager;
    private AbstractController activeController;
    private EnvironmentManager environmentManager;
    private StatusEffectsManager statusEffectsManager;
    private HintsManager hintsManager;
    private MusicPlayer musicPlayer;
    private MusicPlayer playMusicComponent;

    GameSettings gameSettings;

    // Misc
    [SerializeField]
    private GameMode gameMode;
    private GameState gameState = GameState.MainMenu;
    public GameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }
    public GameMode GameMode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }

    public GameSettings GameSettings { get => gameSettings; set => gameSettings = value; }

    private bool isPlaying;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        UpdateReferences();
        DontDestroyOnLoad(gameObject);
    }


    private void OnEnable()
    {
        GameState = (GameState)SceneManager.GetActiveScene().buildIndex;
    }


    private void Start()
    {
        Init();

        musicPlayer?.Play(musicPlayer?.MainMenuMusicPlaylist);
        // TODO: improve this block to support different game modes like cards
    }
    private void Init()
    {
        uiManager = UIManager.instance;
        gameDataManager = GameDataManager.instance;
        environmentManager = EnvironmentManager.instance;
        statusEffectsManager = StatusEffectsManager.instance;
        hintsManager = HintsManager.instance;
        musicPlayer = MusicPlayer.instance;
        GameSettings = GameObject.Find("PersCanvas")?.transform.Find("Settings")?.GetComponent<GameSettings>();
        if (GameMode == null)
        {
            GameMode = ScriptableObject.Instantiate(Resources.Load("ScriptableObjects/GameModes/GM_Freeplay") as GameMode);
        }
        SceneManager.sceneLoaded += SceneLoadedHandling;
        UpdateReferences();
    }

    void FixedUpdate()
    {
        if (GameState == GameState.InGame)
        {
            gameDataManager.DayProgress += Time.deltaTime * (gameDataManager.HoursPerSecond / 24);

            if (gameDataManager.DayProgress >= 1)
            {
                gameDataManager.AddDay();
            }
        }
        CheckIfPauseInput();
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        UpdateReferences();
        Debug.Log(this.GetType().ToString() + "scene loaded handled");
    }

    public void LevelLoadedAndInitialized()
    {
        uiManager.HideLoadingScreen();
        Debug.Log("Scene loaded and managers are initialized");
        gameDataManager.SetValuesToGameModeSpecified(GameMode);
        switch ((GameState)GameState)
        {
            case GameState.MainMenu:
                isPlaying = false;
                break;
            case GameState.InGame:
                isPlaying = true;
                break;
            default:
                break;
        }

        uiManager.UpdateUI();
        musicPlayer?.Play(musicPlayer?.GameplayMusicPlaylist);
        uiManager.HideLoadingScreen();
    }

    public void UpdateReferences()
    {
        playMusicComponent = GameObject.Find("MusicPlayer")?.GetComponent<MusicPlayer>() ?? null;
    }

    public void OpenMainMenu()
    {
        if (GameState == GameState.InGame)
        {
            StartCoroutine("LoadLevel", "MainMenu");
        }
        
    }

    public void StartGame(GameMode gameMode)
    {
        GameMode = ScriptableObject.Instantiate(gameMode);
        StartCoroutine(StartingGameCoroutine());
    }

    private IEnumerator StartingGameCoroutine()
    {
        yield return uiManager.ShowLoadingScreen();
        yield return LoadLevel("GameplayScene");
    }



    

    public IEnumerator LoadLevel(string sceneName)
    {
        

        GameState = GameState.InGame;
        Debug.Log("Starting scene loading with scene manager");

        var gameplayScene = SceneManager.LoadSceneAsync(sceneName);
        while (!gameplayScene.isDone)
        {
            yield return null;
        }
        Debug.Log("Scene load method from GameManager is done");

    }

    public void CheckIfPauseInput()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void PlayButtonOnClick(int gameModeIndex)
    {
        
    }

    public void ToggleStoreMenu()
    {
        uiManager.ToggleStorePanel();
    }

    public void ToggleModifiersInformation()
    {
        uiManager.ToggleModifiersInfoPanel();
    }

    public void ToggleLaborExchange()
    {
        uiManager.ToggleLaborExchange();
    }

    public void Pause()
    {
        uiManager.ShowPauseMenu();
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        uiManager.HidePauseMenu();
        Time.timeScale = 1;
    }
}
