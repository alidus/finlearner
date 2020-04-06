﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Singleton object, gets requests from another objects and control UI elements
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // Managers, Controllers
    GameManager gameManager;
    GameDataManager gameDataManager;
    StatusEffectsManager statusEffectsManager;
    JobExchange laborExchangeManager;

    // UI elements
    CanvasGroup loadingScreenCanvasGroup;
    GameObject statusEffectsPanel;
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
    GameObject jobsShowcasePanel;
    GameObject jobCategoriesPanel;

    // Buttons
    Button laborExchangeButton;
    Button pauseMenuResumeButton;
    Button pauseMenuSettingsButton;
    Button pauseMenuMainMenuButton;

    // Prefabs
    GameObject statusEffectPanelPrefab;

    Image dayProgressBarFillImage;

    Store store;
    HUD hud;
    

    // Other
    Dictionary<StatusEffect, GameObject> modifiersPanelDictionary = new Dictionary<StatusEffect, GameObject>();

    public enum UIState
    {
        MainMenu,
        CardSelection,
        House,
        Store,
        ModifiersInfo,
        LaborExchange
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
            overlayCanvas = GameObject.Find("OverlayCanvas");
            storeContainer = overlayCanvas.transform.Find("StoreContainer").gameObject;
            if (storeContainer)
            {
                store = storeContainer.GetComponent<Store>();
            }
            jobExchangeContainer = overlayCanvas.transform.Find("JobExchangeContainer").gameObject;
            pauseMenuPanel = overlayCanvas.transform.Find("PauseMenuPanel").gameObject;
            statusEffectsPanel = overlayCanvas.transform.Find("StatusEffectsPanel").gameObject;

            foreach (Transform transform in statusEffectsPanel.transform.GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject.name == "MoneyStatusEffectsContentPanel")
                {
                    moneyStatusEffectsContentPanel = transform.gameObject;
                }
                else if (transform.gameObject.name == "MoodStatusEffectsContentPanel")
                {
                    moodStatusEffectsContentPanel = transform.gameObject;
                }
            }
            statusEffectsPanel = overlayCanvas.transform.Find("StatusEffectsPanel")?.gameObject;

            // Prefabs references
            statusEffectPanelPrefab = Resources.Load("Prefabs/StatusEffects/StatusEffectPanel") as GameObject;
            laborExchangeButton = GameObject.Find("LaborExchangeButton")?.GetComponent<Button>();

            Transform pauseMenuButtonsContainerTrans = pauseMenuPanel.transform.Find("ButtonsContainer");
            pauseMenuResumeButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_Resume")?.GetComponent<Button>();
            pauseMenuSettingsButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_Settings")?.GetComponent<Button>();
            pauseMenuMainMenuButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_MainMenu")?.GetComponent<Button>();
        }
        
        hud = GameObject.Find("HUDCanvas")?.GetComponent<HUD>();
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

        statusEffectsManager.OnStatusEffectsChanged += UpdateStatusEffectsView;

        SceneManager.sceneLoaded += SceneLoadedHandling;
        UpdateReferencesAndButtonMappings();
        gameManager.LevelLoadedAndInitialized();

        // Scene fully loaded and managers are initialized, notify game manager about this
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        UpdateReferencesAndButtonMappings();
        hud.Init();
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
            laborExchangeButton.onClick.RemoveAllListeners();
            laborExchangeButton.onClick.AddListener(gameManager.ToggleLaborExchange);

            pauseMenuResumeButton.onClick.RemoveAllListeners();
            pauseMenuResumeButton.onClick.AddListener(gameManager.UnPause);

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

    public void ShowModifiersInfoPanel(bool state)
    {
        if (state)
        {
        }
        else
        {
        }
        statusEffectsPanel.SetActive(state);
    }

    public void ToggleModifiersInfoPanel()
    {
        if (statusEffectsPanel.activeSelf)
        {
            ShowModifiersInfoPanel(false);
        }
        else
        {
            ShowModifiersInfoPanel(true);
        }
    }



    


    public void UpdateStatusEffectsView()
    {
        if (statusEffectsPanel)
        {
            // Clear status effect panel
            foreach (Transform transform in moneyStatusEffectsContentPanel.transform.GetComponentInChildren<Transform>())
            {
                Destroy(transform.gameObject);
            }
            foreach (Transform transform in moodStatusEffectsContentPanel.transform.GetComponentInChildren<Transform>())
            {
                Destroy(transform.gameObject);
            }
            // Iterate through status effects list and create status effects panel
            foreach (StatusEffect statusEffect in statusEffectsManager.StatusEffects)
            {
                GameObject panel = Instantiate(statusEffectPanelPrefab);
                if (statusEffect.Type == StatusEffectType.Money)
                {
                    panel.transform.SetParent(moneyStatusEffectsContentPanel.transform);
                } else if (statusEffect.Type == StatusEffectType.Mood)
                {
                    panel.transform.SetParent(moodStatusEffectsContentPanel.transform);
                }
                panel.transform.localScale = new Vector3(1, 1, 1);
                panel.transform.Find("Info").transform.Find("TopPanel").transform.Find("TitleText").GetComponent<Text>().text = statusEffect.Title;
                Transform BottomPanelTransform = panel.transform.Find("Info").transform.Find("BottomPanel");
                BottomPanelTransform.transform.Find("TypePanel").GetComponentInChildren<Text>().text = statusEffect.Freqency.ToString();
                BottomPanelTransform.transform.Find("ValuePanel").GetComponentInChildren<Text>().text = (statusEffect.Value > 0 ? "+" : "") + statusEffect.Value;
            }
        }
    }

    public void ToggleStorePanel()
    {
        Animator animator = storeContainer?.GetComponent<Animator>();
        if (animator)
        {
            animator.SetBool("IsOpened", !animator.GetBool("IsOpened"));
        }
    }

    

    

    public void ShowLaborExchangePanel(bool state)
    {
        if (state)
        {
        }
        else
        {
        }
    }

    public void ToggleLaborExchange()
    {
        ShowLaborExchangePanel(!jobExchangeContainer.activeSelf);
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
