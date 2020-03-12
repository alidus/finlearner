using System.Collections;
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
    private StoreManager storeManager;
    private HouseManager houseManager;
    private StatusEffectsController statusEffectsManager;
    private HintsManager hintsManager;

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
        storeManager = StoreManager.instance;
        houseManager = HouseManager.instance;
        gameController = GameController.instance;
        statusEffectsManager = StatusEffectsController.instance;
        hintsManager = HintsManager.instance;

        uiManager.SetUIState(UIManager.UIState.MainMenu);
    }

    public void UpdateReferences()
    {

    }

    public void OpenMainMenu()
    {
        uiManager.SetUIState(UIManager.UIState.MainMenu);
    }

    public void OpenCardSelection()
    {
        hintsManager.ShowHint("Выбор уровня кампании", "В игре будет представлен режим кампании с набором отдельных миссий, в каждой из которых перед игроком будет стоять определенная задача в рамках сложившихся обстоятельств.", new HoveringMessageHintPresenter(true, true));
        uiManager.SetUIState(UIManager.UIState.CardSelection);
    }

    public void StartGame()
    {
        GameStateP = GameState.InGame;

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

        uiManager.UpdateStoreView();
        uiManager.SetUIState(UIManager.UIState.House);
        hintsManager.ShowHint("Свободная игра", "В этом режиме игры вам предстоит прожить жизнь по собственному желанию", new HoveringMessageHintPresenter(true, true));
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

    public void ToggleLaborExchange()
    {
        uiManager.ToggleLaborExchange();
    }

   

    


    

}
