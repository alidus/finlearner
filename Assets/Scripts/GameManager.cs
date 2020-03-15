﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage overall game state
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Managers, Controllers
    private UIManager uiManager;
    private GameDataManager gameDataManager;
    private GameController gameController;
    private ItemManager storeManager;
    private HouseManager houseManager;
    private StatusEffectsController statusEffectsManager;
    private HintsManager hintsManager;

    private PlayMusic playMusic;

    // Action delegates and events
    public delegate void GameStartedAction(GameMode gameMode);
    public event GameStartedAction OnGameStarted;


    // Misc
    [SerializeField]
    private GameMode gameMode;
    private GameState gameState = GameState.MainMenu;
    public GameManager.GameState GameStateP
    {
        get { return gameState; }
        set { gameState = value; }
    }
    public GameMode GameMode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }
    
    public enum GameState
    {
        MainMenu, InGame
    }

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

        DontDestroyOnLoad(gameObject);

        UpdateReferences();
    }


    private void OnEnable()
    {
        GameMode = (GameMode)Resources.Load("ScriptableObjects/GameModes/FreePlayGM"); 
    }

    private void Start()
    {
        Init();
    }


    private void Init()
    {
        uiManager = UIManager.instance;
        gameDataManager = GameDataManager.instance;
        storeManager = ItemManager.instance;
        houseManager = HouseManager.instance;
        gameController = GameController.instance;
        statusEffectsManager = StatusEffectsController.instance;
        hintsManager = HintsManager.instance;

        
        uiManager.InitSceneChange(GameStateP);
    }

    public void UpdateReferences()
    {
        playMusic = GameObject.Find("MusicPlayer").GetComponent<PlayMusic>();
    }

    public void OpenMainMenu()
    {
        if (GameStateP == GameState.InGame)
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
        uiManager.SetUIState(UIManager.UIState.Loading);
        var gameplayScene = SceneManager.LoadSceneAsync(sceneName);
        while (!gameplayScene.isDone)
        {
            yield return 1;
        }
          
    }

    private void OnLevelWasLoaded(int level)
    {
        GameStateP = (GameState)level;        

        switch (GameStateP)
        {
            case GameState.MainMenu:
                playMusic.Play(playMusic.MainMenuMusicPlayer);
                break;
            case GameState.InGame:
                playMusic.Play(playMusic.GameplayMusicPlayer);
                storeManager.UpdateReferences();
                houseManager.UpdateReferences();
                statusEffectsManager.UpdateReferences();
                houseManager.UpdateFlatAppearance();
                OnGameStarted(gameMode);
                break;
            default:
                break;
        }

        uiManager.InitSceneChange(GameStateP);

        uiManager.UpdateUI();

        playMusic.Play(playMusic.MainMenuMusicPlayer);
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
