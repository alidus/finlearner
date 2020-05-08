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
    private EnvironmentManager environmentManager;
    private StatusEffectsManager statusEffectsManager;
    private HintsManager hintsManager;
    private MusicPlayer musicPlayer;

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
            GameMode = Resources.Load("ScriptableObjects/GameModes/GM_Freeplay") as GameMode;
        }
        SceneManager.sceneLoaded += SceneLoadedHandling;
        UpdateReferences();
    }

    void Update()
    {
        if (GameState == GameState.InGame)
        {
            gameDataManager.DayProgress += gameDataManager.GetDeltaDay();

            if (gameDataManager.DayProgress >= 1)
            {
                gameDataManager.AddDay();
            }
            // Debug
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                gameDataManager.Money += 50;
                gameDataManager.Mood += 5;

            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                gameDataManager.Money -= 50;
                gameDataManager.Mood -= 5;
            }
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleConsole();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiManager.State != GameplayUIState.Home)
            {
                uiManager.SetGamplayUIState(GameplayUIState.Home);
            } else
            {
                Pause();
            }
        }
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
        gameMode.SubscribeConditions();
        switch ((GameState)GameState)
        {
            case GameState.MainMenu:
                isPlaying = false;
                break;
            case GameState.InGame:
                isPlaying = true;
                Milestones.instance.OnGameWin -= WinGame;
                Milestones.instance.OnGameWin += WinGame;
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
    }

    public void WinGame()
    {
        HintsManager.instance.ShowHint("Победа!", "Вы выполнили все поставленные задачи");
        GameMode.IsCompleted = true;
        StartCoroutine(GameEndBackToMenuTransition(1f));
    }

    IEnumerator GameEndBackToMenuTransition(float delay)
    {
        yield return new WaitForSeconds(delay);
        OpenMainMenu();
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
        GameMode = gameMode;
        StartCoroutine(StartingGameCoroutine());
    }

    private IEnumerator StartingGameCoroutine()
    {
        yield return uiManager.ShowLoadingScreen();
        yield return LoadLevel("GameplayScene");
    }


    public void ToggleConsole()
    {
        Console.Toggle();
    }
    

    public IEnumerator LoadLevel(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                GameState = GameState.MainMenu;
                break;
            case "GameplayScene":
                GameState = GameState.InGame;
                break;
            default:
                break;
        }
        Debug.Log("Starting scene loading with scene manager");

        var gameplayScene = SceneManager.LoadSceneAsync(sceneName);
        while (!gameplayScene.isDone)
        {
            yield return null;
        }
        Debug.Log("Scene load method from GameManager is done");

    }


    public void PlayButtonOnClick(int gameModeIndex)
    {
        
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
