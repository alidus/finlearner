using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
    private HouseManager houseManager;
    private StatusEffectsController statusEffectsManager;
    private HintsManager hintsManager;

    private MusicPlayer playMusicComponent;

    // Action delegates and events
    public event Action OnLevelLoaded;
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

        UpdateReferences();
    }


    private void OnEnable()
    {
        GameMode = (GameMode)Resources.Load("ScriptableObjects/GameModes/FreePlayGM");
        GameState = (GameState)SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        Init();
        if (GameState == GameState.InGame)
        {
            // TODO: Once again make initialization check
            Init();
            GameplayLoadedActions();
        }
    }


    private void Init()
    {
        uiManager = UIManager.instance;
        gameDataManager = GameDataManager.instance;
        inventoryManager = InventoryManager.instance;
        houseManager = HouseManager.instance;
        statusEffectsManager = StatusEffectsController.instance;
        hintsManager = HintsManager.instance;
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
        var gameplayScene = SceneManager.LoadSceneAsync(sceneName);
        while (!gameplayScene.isDone)
        {
            yield return 1;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        GameState = (GameState)level;        
        switch (GameState)
        {
            case GameState.MainMenu:
                MainMenuLoadedActions();
                break;
            case GameState.InGame:
                GameplayLoadedActions();
                break;
            default:
                break;
        }

        uiManager.AdaptUIToScene();
        uiManager.UpdateUI();
        uiManager.HideLoadingScreen();
        OnLevelLoaded();
    }

    private void MainMenuLoadedActions()
    {
        playMusicComponent.Play(playMusicComponent.MainMenuMusicPlaylist);
    }
    private void GameplayLoadedActions()
    {
        // TODO: implement observer pattern
        gameDataManager.LoadGamemodeData(GameMode);
        if (playMusicComponent)
            playMusicComponent.Play(playMusicComponent.GameplayMusicPlaylist);
        inventoryManager.UpdateReferences();
        houseManager.UpdateReferences();
        statusEffectsManager.UpdateReferences();
        houseManager.UpdateFlatAppearance();
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
