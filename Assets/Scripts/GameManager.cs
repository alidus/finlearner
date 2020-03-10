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
    private GameController gameController;
    private StoreController storeManager;
    private HouseManager houseManager;
    private StatusEffectsController statusEffectsManager;

    // Action delegates and events
    public delegate void GameStartedAction(GameMode gameMode);
    public static event GameStartedAction OnGameStarted;


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
        storeManager = StoreController.instance;
        houseManager = HouseManager.instance;
        gameController = GameController.instance;
        statusEffectsManager = StatusEffectsController.instance;

        uiManager.SetUIState(UIManager.UIState.MainMenu);
    }

    public void UpdateReferences()
    {
        
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
          
    }

    private void OnLevelWasLoaded(int level)
    {
        uiManager.UpdateReferences();
        storeManager.UpdateReferences();
        houseManager.UpdateReferences();
        statusEffectsManager.UpdateReferences();

        uiManager.UpdateUI();
        houseManager.UpdateFlatAppearance();

        storeManager.UpdateStoreView();
        uiManager.SetUIState(UIManager.UIState.House);
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
