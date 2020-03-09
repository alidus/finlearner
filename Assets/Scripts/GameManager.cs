using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Managers, Controllers
    private UIManager uiManager;
    private GameDataManager gameDataManager;
    public GameController gameController;


    // Action delegates and events
    public delegate void GameStartedAction(GameMode gameMode);
    public static event GameStartedAction OnGameStarted;
    //public static event GameDelegate OnGameOver;
    public delegate void StoreOpenAction();
    public static event StoreOpenAction OnStoreButtonPress;

    // Misc
    public Sprite placeHolder;
    [SerializeField]
    private GameMode gameMode;
    private GameState gameState = GameState.MainMenu;

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
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
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
        gameController = GetComponent<GameController>();

        uiManager.SetUIState(UIManager.UIState.MainMenu);
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public GameState GetGameState()
    {
        return gameState;
    }



    public void OpenMainMenu()
    {
        uiManager.SetUIState(UIManager.UIState.MainMenu);
    }

    public void OpenCardSelection()
    {
        uiManager.SetUIState(UIManager.UIState.CardSelection);
    }

    public void StartGame()
    {
        SetGameState(GameState.InGame);

        StartCoroutine("LoadLevel");

        
    }

    public IEnumerator LoadLevel()
    {
        uiManager.SetUIState(UIManager.UIState.Loading);
        var gameplayScene = SceneManager.LoadSceneAsync("GameplayScene");
        while (!gameplayScene.isDone)
        {
            yield return 1;
        }
        uiManager.InitReferences();
        HouseManager.Init();
        uiManager.UpdateUI();
        HouseManager.UpdateFlatAppearance();

        uiManager.SetUIState(UIManager.UIState.House);
    }

    private void OnLevelWasLoaded(int level)
    {
        OnGameStarted(gameMode);
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

   

    


    

}
