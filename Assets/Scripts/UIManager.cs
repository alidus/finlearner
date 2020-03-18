using System;
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
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private InventoryManager inventoryManager;
    private StatusEffectsManager statusEffectsManager;
    private LaborExchangeManager laborExchangeManager;

    // UI elements
    private GameObject mainMenuPanel;
    private GameObject cardSelectionPanel;
    private CanvasGroup loadingScreenCanvasGroup;
    private GameObject infoPanel;
    private GameObject statusEffectsPanel;
    private GameObject moneyPanel;
    private GameObject moodPanel;
    private GameObject weekProgressBar;
    private GameObject dayProgressBar;
    private GameObject uiCanvas;
    private GameObject loadingCanvas;
    private GameObject gameplayHUDPanel;
    private GameObject overlaysContainerPanel;
    private GameObject moneyStatusEffectsContentPanel;
    private GameObject moodStatusEffectsContentPanel;
    // Pause menu
    private GameObject pauseMenuPanel;

    // Store
    [HideInInspector]
    public GameObject storeContainer;
    // Labor exchange
    private GameObject laborExchangeContainer;
    private GameObject jobsShowcasePanel;
    private GameObject jobCategoriesPanel;

    // Buttons
    private Button storeButton;
    private Button infoPanelButton;
    private Button getCreditButtonTEST;

    private Button laborExchangeButton;
    private Button pauseMenuResumeButton;
    private Button pauseMenuSettingsButton;
    private Button pauseMenuMainMenuButton;

    private Button mainMenuCampaignButton;
    private Button mainMenuFreePlayButton;
    // Prefabs
    private GameObject statusEffectPanelPrefab;
    private GameObject jobCategoryPanelPrefab;
    private GameObject jobPanelPrefab;

    private Image dayProgressBarFillImage;


    // Colors
    [SerializeField]
    Color dayLightColor = new Color(1, 1, 1, 0.3f);
    [SerializeField]
    Color dayWeekendLightColor = new Color(1, 1, 1, 0.3f);

    Color emptyDayColor = new Color(1, 1, 1, .0f);

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

    private void UpdateReferences()
    {
        loadingCanvas = GameObject.Find("LoadingCanvas");
        DontDestroyOnLoad(loadingCanvas);
        loadingScreenCanvasGroup = loadingCanvas?.transform.Find("LoadingPanel")?.GetComponent<CanvasGroup>() ?? null;
        uiCanvas = GameObject.Find("UICanvas");
        // Update references to UI elements based on game state
        if (gameManager.GameState == GameState.MainMenu)
        {
            mainMenuPanel = uiCanvas.transform.Find("MainMenuPanel").gameObject;
            cardSelectionPanel = uiCanvas.transform.Find("CardSelectionPanel").gameObject;
            Transform mainMenuButtonsContainer = mainMenuPanel.transform.Find("ButtonsContainer").transform;
            mainMenuCampaignButton = mainMenuButtonsContainer.transform.Find("CampaignButton").GetComponent<Button>();
            mainMenuFreePlayButton = mainMenuButtonsContainer.transform.Find("FreePlayButton").GetComponent<Button>();
        }
        else if (gameManager.GameState == GameState.InGame)
        {
            gameplayHUDPanel = GameObject.Find("GameplayHUDPanel");
            overlaysContainerPanel = gameplayHUDPanel.transform.Find("OverlaysContainerPanel").gameObject;
            storeContainer = overlaysContainerPanel.transform.Find("StoreContainer").gameObject;
            if (storeContainer)
            {
                // TODO: Implement initialization check
                inventoryManager = InventoryManager.instance;
                inventoryManager.InstantiateHomeStore(storeContainer.transform);
            }
            laborExchangeContainer = overlaysContainerPanel.transform.Find("LaborExchangeContainer").gameObject;
            statusEffectsPanel = overlaysContainerPanel.transform.Find("StatusEffectsPanel").gameObject;
            pauseMenuPanel = overlaysContainerPanel.transform.Find("PauseMenuPanel").gameObject;
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
            infoPanel = GameObject.Find("InfoPanel");
            statusEffectsPanel = overlaysContainerPanel.transform.Find("StatusEffectsPanel")?.gameObject;
            moneyPanel = GameObject.Find("MoneyPanel");
            moodPanel = GameObject.Find("MoodPanel");
            weekProgressBar = GameObject.Find("WeekProgressBar");
            dayProgressBar = GameObject.Find("DayProgressBar");

            dayProgressBarFillImage = GameObject.Find("DayProgressBarFillImage")?.GetComponent<Image>();
            // Prefabs references
            statusEffectPanelPrefab = Resources.Load("Prefabs/StatusEffects/StatusEffectPanel") as GameObject;
            jobCategoryPanelPrefab = Resources.Load("Prefabs/Jobs/JobCategoryPanel") as GameObject;
            jobPanelPrefab = Resources.Load("Prefabs/Jobs/JobPanel") as GameObject;
            storeButton = GameObject.Find("StoreButton")?.GetComponent<Button>();
            laborExchangeButton = GameObject.Find("LaborExchangeButton")?.GetComponent<Button>();
            infoPanelButton = GameObject.Find("InfoPanel")?.GetComponent<Button>();
            getCreditButtonTEST = GameObject.Find("GetCreditButton")?.GetComponent<Button>();
            Transform pauseMenuButtonsContainerTrans = pauseMenuPanel.transform.Find("ButtonsContainer");
            pauseMenuResumeButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_Resume")?.GetComponent<Button>();
            pauseMenuSettingsButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_Settings")?.GetComponent<Button>();
            pauseMenuMainMenuButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_MainMenu")?.GetComponent<Button>();
        }

    }

    internal void OpenCardSelection()
    {
        SetUIState(UIState.CardSelection);
    }

    private void UpdateReferencesAndButtonMappings()
    {
        UpdateReferences();
        
        switch (GameManager.instance.GameState)
        {
            case GameState.MainMenu:
                SetUIState(UIManager.UIState.MainMenu);
                break;
            case GameState.InGame:
                SetUIState(UIManager.UIState.House);
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
        inventoryManager = InventoryManager.instance;
        statusEffectsManager = StatusEffectsManager.instance;
        laborExchangeManager = LaborExchangeManager.instance;

        gameDataManager.OnMoneyValueChanged += UpdateMoneyPanel;
        gameDataManager.OnMoodValueChanged += UpdateMoodPanel;
        gameDataManager.OnDayProgressChanged += UpdateDayProgressBar;
        statusEffectsManager.OnStatusEffectsChanged += UpdateStatusEffectsView;
        gameDataManager.OnDayStarted += UpdateInfoPanel;
        gameDataManager.OnDayStarted += UpdateWeekProgressBar;

        SceneManager.sceneLoaded += SceneLoadedHandling;
        UpdateReferencesAndButtonMappings();
        gameManager.LevelLoadedAndInitialized();

        // Scene fully loaded and managers are initialized, notify game manager about this
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        UpdateReferencesAndButtonMappings();
        Debug.Log(this.GetType().ToString() + "scene loaded handled");
        gameManager.LevelLoadedAndInitialized();
    }


    /// <summary>
    /// Remap every button on new scene (Expensive, optimization needed)
    /// </summary>
    private void UpdateButtonsClickActionsAfterSceneLoad()
    {
        if (GameManager.instance.GameState == GameState.MainMenu)
        {
            mainMenuCampaignButton.onClick.RemoveAllListeners();
            mainMenuCampaignButton.onClick.AddListener(gameManager.OpenCardSelection);

            mainMenuFreePlayButton.onClick.RemoveAllListeners();
            mainMenuFreePlayButton.onClick.AddListener(gameManager.StartGame);
        } else if (GameManager.instance.GameState == GameState.InGame)
        {
            laborExchangeButton.onClick.RemoveAllListeners();
            laborExchangeButton.onClick.AddListener(gameManager.ToggleLaborExchange);

            storeButton.onClick.RemoveAllListeners();
            storeButton.onClick.AddListener(gameManager.ToggleStoreMenu);

            infoPanelButton.onClick.RemoveAllListeners();
            infoPanelButton.onClick.AddListener(gameManager.ToggleModifiersInformation);

            pauseMenuResumeButton.onClick.RemoveAllListeners();
            pauseMenuResumeButton.onClick.AddListener(gameManager.UnPause);

            pauseMenuSettingsButton.onClick.RemoveAllListeners();
            pauseMenuSettingsButton.onClick.AddListener(delegate { print("Open settings..."); });

            pauseMenuMainMenuButton.onClick.RemoveAllListeners();
            pauseMenuMainMenuButton.onClick.AddListener(gameManager.OpenMainMenu);
        }
        
    }

    /// <summary>
    /// Set UI state and manipulate related UI elements accordingly (like close mod info upon store opening, etc)
    /// </summary>
    /// <param name="state"></param>
    private void SetUIState(UIState state)
    {
        // TODO: improve alg
        switch (state)
        {
            case UIState.MainMenu:
                mainMenuPanel.SetActive(true);
                cardSelectionPanel.SetActive(false);

                break;
            case UIState.CardSelection:
                mainMenuPanel.SetActive(false);
                cardSelectionPanel.SetActive(true);
                break;
            case UIState.House:

                overlaysContainerPanel.SetActive(true);
                statusEffectsPanel.SetActive(false);
                laborExchangeContainer.SetActive(false);
                UpdateStoreView();
                break;
            case UIState.Store:
                UpdateStoreView();
                overlaysContainerPanel.SetActive(true);
                statusEffectsPanel.SetActive(false);
                laborExchangeContainer.SetActive(false);
                break;
            case UIState.ModifiersInfo:

                statusEffectsPanel.SetActive(true);
                laborExchangeContainer.SetActive(false);
                break;
            case UIState.LaborExchange:

                statusEffectsPanel.SetActive(false);
                laborExchangeContainer.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void UpdateUI()
    {
        UpdateMoneyPanel();
        UpdateMoodPanel();
        UpdateInfoPanel();
        UpdateStoreView();

        UpdateWeekProgressBar();
    }

    private void UpdateStoreView()
    {
        if (inventoryManager.Store != null)
        {
            float time1 = Time.time;
            inventoryManager.Store.UpdateAll();
            Debug.Log("Store update took " + (Time.time - time1).ToString() + " seconds");
        }
    }

    public void UpdateInfoPanel()
    {
        if (infoPanel)
        {
            infoPanel.transform.Find("Date").GetComponent<Text>().text = gameDataManager.CurrentDateTime.ToString("dd/MM/yyyy");
            infoPanel.transform.Find("Age").GetComponent<Text>().text = gameDataManager.Age.ToString() + " лет";
            infoPanel.transform.Find("Day").GetComponent<Text>().text = "День года: " + gameDataManager.CurrentDateTime.DayOfYear.ToString();
            infoPanel.transform.Find("Income").GetComponent<Text>().text = "Доход:\n" +
               "$" + gameDataManager.DailyIncome.ToString() + " за день\n" +
              "$" + gameDataManager.WeeklyIncome.ToString() + " за неделю\n" +
               "$" + gameDataManager.MonthlyIncome.ToString() + " за месяц\n" +
               "$" + gameDataManager.YearlyIncome.ToString() + " за год\n";
        }
    }

    public void UpdateMoneyPanel()
    {
        if (moneyPanel)
        {
            moneyPanel.GetComponentInChildren<Text>().text = "$" + gameDataManager.Money.ToString();
        }
    }

    public void UpdateMoodPanel()
    {
        if (moodPanel)
        {
            moodPanel.GetComponentInChildren<Text>().text = gameDataManager.Mood.ToString();
            if (gameDataManager.Mood > 0)
            {
                float coef = (float)gameDataManager.Mood / 100;
                moodPanel.GetComponentInChildren<Text>().color = new Color(1, coef, coef, 1);
            }
        }
    }

    public void ShowModifiersInfoPanel(bool state)
    {
        if (state)
        {
            SetUIState(UIState.ModifiersInfo);
        }
        else
        {
            SetUIState(UIState.House);
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



    public void UpdateWeekProgressBar()
    {
        if (weekProgressBar)
        {
            // Day of week index ranged from 0 to 6
            int normalDayOfWeekIndex = ((int)gameDataManager.CurrentDateTime.DayOfWeek == 0) ? 6 : (int)gameDataManager.CurrentDateTime.DayOfWeek - 1;

            foreach (Transform weekDayIndicatorTransfrom in weekProgressBar.transform)
            {
                int weekDayIndicatorIndex = weekDayIndicatorTransfrom.GetSiblingIndex();
                Image fillImage = weekDayIndicatorTransfrom.Find("DayFill").GetComponent<Image>();
                Light2D light = weekDayIndicatorTransfrom.GetComponentInChildren<Light2D>();
                Animator animator = weekDayIndicatorTransfrom.GetComponent<Animator>();

                if (weekDayIndicatorIndex > 4)
                {
                    light.color = dayWeekendLightColor;
                    fillImage.color = dayWeekendLightColor;
                } else
                {
                    light.color = dayLightColor;
                    fillImage.color = dayLightColor;
                }
                if (fillImage)
                {
                    if (weekDayIndicatorIndex < normalDayOfWeekIndex)
                    {
                        // Passed day
                        animator.SetBool("IsPassedDay", true);
                        animator.SetBool("IsPresentDay", false);
                        animator.SetBool("IsFutureDay", false);
                    }
                    else if (weekDayIndicatorIndex == normalDayOfWeekIndex)
                    {
                        // Present day
                        animator.SetBool("IsPassedDay", false);
                        animator.SetBool("IsPresentDay", true);
                        animator.SetBool("IsFutureDay", false);
                    }
                    else
                    {
                        // Future day
                        animator.SetBool("IsPassedDay", false);
                        animator.SetBool("IsPresentDay", false);
                        animator.SetBool("IsFutureDay", true);
                    }
                }
            }
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

    public void UpdateDayProgressBar()
    {
        if (dayProgressBarFillImage)
        {
            dayProgressBarFillImage.fillAmount = gameDataManager.DayProgress;
        }
    }

    

    public void ShowLaborExchangePanel(bool state)
    {
        if (state)
        {
            SetUIState(UIState.LaborExchange);
        }
        else
        {
            SetUIState(UIState.House);
        }
    }

    public void ToggleLaborExchange()
    {
        ShowLaborExchangePanel(!laborExchangeContainer.activeSelf);
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
