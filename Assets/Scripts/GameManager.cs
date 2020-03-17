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

    private MusicPlayer playMusicComponent;

    // Action delegates and events
    //public delegate void GameStartedAction(GameMode gameMode);
    //public event GameStartedAction OnGameStarted;
    public Action OnLevelInitialized;

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
        if (GameDataManager.instance.DEBUG)
            Debug.Log("GameManager awake");
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

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
        SceneManager.sceneLoaded += SceneLoadedHandling;
        SceneManager.activeSceneChanged += delegate (Scene s1, Scene s2) { Debug.Log("Scene shanged"); };
        SceneManager.sceneLoaded += delegate (Scene s1, LoadSceneMode lsm) { Debug.Log("Scene loaded"); };

        OnLevelInitialized += OnLevelLoadedAndInitialized;
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        UpdateReferences();
        Debug.Log(this.GetType().ToString() + "scene loaded handled");
    }

    private void OnLevelLoadedAndInitialized()
    {
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
        StartCoroutine("LoadLevel", "GameplayScene");
    }

    public IEnumerator LoadLevel(string sceneName)
    {
        uiManager.ShowLoadingScreen();
        GameState = GameState.InGame;
        var gameplayScene = SceneManager.LoadSceneAsync(sceneName);
        while (!gameplayScene.isDone)
        {
            yield return 1;
        }
    }

    

    public void ActivateFreeplayController()
    {
        // TODO: implement observer pattern
        ActiveController = GetComponent<FreeplayController>() ?? gameObject.AddComponent<FreeplayController>();
        ActiveController.Init();
        ActiveController.IsPlayerController = true;
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

    public void Unpause()
    {
        uiManager.HidePauseMenu();
        Time.timeScale = 1;
    }
}
