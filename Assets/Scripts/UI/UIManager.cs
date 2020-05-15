using Showcase.Bank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameplayUIState { Store, JobExchange, Home, EducationHub, Inventory, StatisticsHub, Bank }

/// <summary>
/// Control global UI state
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // Managers, Controllers
    GameManager gameManager;
    GameDataManager gameDataManager;
    StatusEffectsManager statusEffectsManager;
    Store store;
    JobExchange jobExchange;
    EducationHub educationHub;
    Inventory inventory;
    StatisticsHub statisticsHub;
    Bank bank;


    // UI elements
    CanvasGroup loadingScreenCanvasGroup;
    GameObject persCanvas;

    GameObject overlayCanvas;
    GameObject moneyStatusEffectsContentPanel;
    GameObject moodStatusEffectsContentPanel;

    // Pause menu
    GameObject pauseMenuPanel;
    
    // Settings menu
    private GameObject settingsPanel;

    // Store
    [HideInInspector]
    public GameObject storeContainer;

    // Labor exchange
    GameObject jobExchangeContainer;

    // Buttons
    Button laborExchangeButton;
    Button pauseMenuResumeButton;
    Button pauseMenuSettingsButton;
    Button pauseMenuMainMenuButton;


    Image dayProgressBarFillImage;

    GameplayHUD hud;
    
    // Other
    Dictionary<StatusEffect, GameObject> modifiersPanelDictionary = new Dictionary<StatusEffect, GameObject>();


    public GameplayUIState State = GameplayUIState.Home;

    //public enum UIState
    //{
    //    MainMenu,
    //    CardSelection,
    //    House,
    //    Store,
    //    ModifiersInfo,
    //    LaborExchange
    //}

    public void SetGamplayUIState(GameplayUIState gameplayUIState)
    {
        switch (gameplayUIState)
        {
            case GameplayUIState.Home:
                store.Hide();
                jobExchange.Hide();
                inventory.Hide();
                statisticsHub.Hide();
                bank.Hide();
                educationHub.Hide();
                break;
            case GameplayUIState.Store:
                jobExchange.Hide();
                educationHub.Hide();
                inventory.Hide();
                statisticsHub.Hide();
                bank.Hide();
                store.Show();
                break;
            case GameplayUIState.JobExchange:
                store.Hide();
                educationHub.Hide();
                inventory.Hide();
                statisticsHub.Hide();
                bank.Hide();
                jobExchange.Show();
                break;
            case GameplayUIState.EducationHub:
                store.Hide();
                jobExchange.Hide();
                inventory.Hide();
                statisticsHub.Hide();
                bank.Hide();
                educationHub.Show();
                break;
            case GameplayUIState.Inventory:
                store.Hide();
                jobExchange.Hide();
                educationHub.Hide();
                statisticsHub.Hide();
                bank.Hide();
                inventory.Show();
                break;
            case GameplayUIState.StatisticsHub:
                store.Hide();
                jobExchange.Hide();
                educationHub.Hide();
                inventory.Hide();
                bank.Hide();
                statisticsHub.Show();
                break;
            case GameplayUIState.Bank:
                store.Hide();
                jobExchange.Hide();
                inventory.Hide();
                statisticsHub.Hide();
                educationHub.Hide();
                bank.Show();
                break;
            default:
                break;
        }

        Console.Print("UI state switched to: " + gameplayUIState.ToString());

        State = gameplayUIState;
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
    }

    /// <summary>
    /// Update references to every controlled UI element (for ex. on scene transition)
    /// TODO: Break this into more scene-specific components to reduce complexity
    /// </summary>
    private void UpdateReferences()
    {
        persCanvas = GameObject.Find("PersCanvas");
        DontDestroyOnLoad(persCanvas);
        loadingScreenCanvasGroup = persCanvas?.transform.Find("LoadingPanel")?.GetComponent<CanvasGroup>() ?? null;
        settingsPanel = persCanvas?.transform.Find("Settings").gameObject;
        // Update references to UI elements based on game state
        if (gameManager.GameState == GameState.InGame)
        {
            store = Store.instance as Store;
            jobExchange = JobExchange.instance as JobExchange;
            educationHub = EducationHub.instance as EducationHub;
            inventory = Inventory.instance as Inventory;
            statisticsHub = StatisticsHub.instance as StatisticsHub;
            bank = Bank.instance as Bank;

            overlayCanvas = GameObject.Find("OverlayCanvas");
            storeContainer = overlayCanvas.transform.Find("StoreContainer").gameObject;

            jobExchangeContainer = overlayCanvas.transform.Find("JobExchangeContainer").gameObject;

            pauseMenuPanel = overlayCanvas.transform.Find("PauseMenuPanel").gameObject;

            Transform pauseMenuButtonsContainerTrans = pauseMenuPanel.transform.Find("ButtonsContainer");
            pauseMenuResumeButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_Resume")?.GetComponent<Button>();
            pauseMenuSettingsButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_Settings")?.GetComponent<Button>();
            pauseMenuMainMenuButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_MainMenu")?.GetComponent<Button>();
        }
        
        hud = GameObject.Find("HUDCanvas")?.GetComponent<GameplayHUD>();
    }

    internal void OpenCardSelection()
    {
    }

    private void UpdateReferencesAndButtonMappings()
    {
        UpdateReferences();
        
        switch (GameManager.instance.GameState)
        {
            case GameState.MainMenu:
                break;
            case GameState.InGame:
                break;
            default:
                break;
        }

        UpdateButtonsClickActionsAfterSceneLoad();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        statusEffectsManager = StatusEffectsManager.instance;


        SceneManager.sceneLoaded += SceneLoadedHandling;
        UpdateReferencesAndButtonMappings();
        gameManager.LevelLoadedAndInitialized();

        // Scene fully loaded and managers are initialized, notify game manager about this
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        UpdateReferencesAndButtonMappings();
        if (gameManager.GameState == GameState.InGame)
        {
            hud.Init();
        }
        Debug.Log(this.GetType().ToString() + "scene loaded handled");
        gameManager.LevelLoadedAndInitialized();
    }


    /// <summary>
    /// Remap every button on new scene (Expensive, optimization needed)
    /// </summary>
    private void UpdateButtonsClickActionsAfterSceneLoad()
    {
        if (gameManager.GameState == GameState.InGame)
        {
            pauseMenuResumeButton.onClick.RemoveAllListeners();
            pauseMenuResumeButton.onClick.AddListener(gameManager.Resume);

            pauseMenuSettingsButton.onClick.RemoveAllListeners();
            pauseMenuSettingsButton.onClick.AddListener(delegate { print("Open settings..."); });

            pauseMenuMainMenuButton.onClick.RemoveAllListeners();
            pauseMenuMainMenuButton.onClick.AddListener(gameManager.OpenMainMenu);
        }

    }

    public void UpdateUI()
    {
        if (gameManager.GameState == GameState.InGame)
        {
            hud.UpdateHUD();
            UpdateStore();
        }
    }

    private void UpdateStore()
    {
        if (store)
        {
            store.UpdateShowcase();
        }
    }



    public void ShowPauseMenu()
    {
        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
    }


    public void HidePauseMenu()
    {
        Time.timeScale = 1;
        pauseMenuPanel.SetActive(false);
    }

    public IEnumerator ShowLoadingScreen()
    {
        if (loadingScreenCanvasGroup)
        {
            Animator animator = loadingScreenCanvasGroup.gameObject.GetComponent<Animator>();
            if (animator)
            {
                Debug.Log("Starting loading screen animation");

                animator.Play("LoadingPanel_FadeIn");
                var t1 = Time.time;
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                var t2 = Time.time;
                Debug.Log("Loading screen animation finished. Duration: " + (t2 - t1).ToString());

            }
        }
    }

    public void HideLoadingScreen()
    {
        if (loadingScreenCanvasGroup)
        {
            Animator animator = loadingScreenCanvasGroup.gameObject.GetComponent<Animator>();
            if (animator)
            {
                animator.Play("LoadingPanel_FadeOut");
            }
        }
    }
}
