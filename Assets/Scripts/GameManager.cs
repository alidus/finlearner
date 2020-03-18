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
    private FreeplayController activeController;
    private InventoryManager inventoryManager;
    private EnvironmentManager environmentManager;
    private StatusEffectsController statusEffectsManager;
    private HintsManager hintsManager;
    private MusicPlayer musicPlayer;

    private MusicPlayer playMusicComponent;

    // Action delegates and events
    //public delegate void GameStartedAction(GameMode gameMode);
    //public event GameStartedAction OnGameStarted;

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

    public FreeplayController ActiveController { get => activeController; private set => activeController = value; }

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
        GameMode = (GameMode)Resources.Load("ScriptableObjects/GameModes/FreePlayGM");
        GameState = (GameState)SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        Init();

        musicPlayer?.Play(musicPlayer?.MainMenuMusicPlaylist);
        // TODO: improve this block to support different game modes like cards
        if (GameState == GameState.InGame)
        {
            if (ActiveController == null)
            {
                ActiveController = this.gameObject.AddComponent<FreeplayController>();
            } 
        }
    }


    private void Init()
    {
        uiManager = UIManager.instance;
        gameDataManager = GameDataManager.instance;
        inventoryManager = InventoryManager.instance;
        environmentManager = EnvironmentManager.instance;
        statusEffectsManager = StatusEffectsController.instance;
        hintsManager = HintsManager.instance;
        musicPlayer = MusicPlayer.instance;
        SceneManager.sceneLoaded += SceneLoadedHandling;
        SceneManager.activeSceneChanged += delegate (Scene s1, Scene s2) { Debug.Log("Scene shanged"); };
        SceneManager.sceneLoaded += delegate (Scene s1, LoadSceneMode lsm) { Debug.Log("Scene loaded"); };
        UpdateReferences();
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        UpdateReferences();
        Debug.Log(this.GetType().ToString() + "scene loaded handled");
    }

    public void LevelLoadedAndInitialized()
    {
        uiManager.HideLoadingScreen();
        Debug.Log("Scene loaded and managers are initialized. Activating controller...");
        switch ((GameState)GameState)
        {
            case GameState.MainMenu:
                break;
            case GameState.InGame:
                ActivateFreeplayController();
                break;
            default:
                break;
        }

        uiManager.UpdateReferencedAndButtonMappings();
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

    public void OpenCardSelection()
    {
        hintsManager.ShowHint("Выбор уровня кампании", "В игре будет представлен режим кампании с набором отдельных миссий, в каждой из которых перед игроком будет стоять определенная задача в рамках сложившихся обстоятельств.", new HoveringMessageHintPresenter(true, true));
        uiManager.SetUIState(UIManager.UIState.CardSelection);
    }

    public void StartGame()
    {
        StartCoroutine(StartingGameCoroutine());
    }

    public IEnumerator StartingGameCoroutine()
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



    public void ActivateFreeplayController()
    {
        // TODO: implement observer pattern
        ActiveController = GetComponent<FreeplayController>() ?? gameObject.AddComponent<FreeplayController>();
        ActiveController.Init();
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
