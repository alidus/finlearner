using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private GameController gameController;
    private StoreManager storeManager;
    private StatusEffectsController statusEffectsController;
    private LaborExchangeManager laborExchangeManager;

    // UI elements
    private GameObject mainMenuPanel;
    private GameObject cardSelectionPanel;
    private GameObject loadingPanel;
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
    private GameObject storePanel;
    private GameObject storeItemsShowcasePanel;
    private GameObject storeItemCategoriesPanel;
    // Labor exchange
    private GameObject laborExchangePanel;
    private GameObject jobsShowcasePanel;
    private GameObject jobCategoriesPanel;

    private IStoreView activeStoreView;

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
    private GameObject storeItemCategoryPanelPrefab;
    private GameObject storeItemPanelPrefab;
    private GameObject jobCategoryPanelPrefab;
    private GameObject jobPanelPrefab;

    private Image dayProgressBarFillImage;


    // Colors
    Color passedDayColor = new Color(1, 1, 1, 1);
    Color presentDayColor = new Color(1, 1, 1, 0.3f);
    Color emptyDayColor = new Color(1, 1, 1, .0f);

    // Other
    Dictionary<StatusEffect, GameObject> modifiersPanelDictionary = new Dictionary<StatusEffect, GameObject>();

    public enum UIState
    {
        MainMenu,
        CardSelection,
        Loading,
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
        // TODO: not very good, try to do it another way
        gameManager = GameManager.instance;
        UpdateReferences();
    }

    public void UpdateReferences()
    {
        loadingCanvas = GameObject.Find("LoadingCanvas").gameObject;
        DontDestroyOnLoad(loadingCanvas);
        loadingPanel = loadingCanvas.transform.Find("LoadingPanel").gameObject;
        uiCanvas = GameObject.Find("UICanvas");
        // Update references to UI elements based on game state
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            mainMenuPanel = uiCanvas.transform.Find("MainMenuPanel").gameObject;
            cardSelectionPanel = uiCanvas.transform.Find("CardSelectionPanel").gameObject;
        }
        else
        {
            gameplayHUDPanel = GameObject.Find("GameplayHUDPanel");
            overlaysContainerPanel = gameplayHUDPanel.transform.Find("OverlaysContainerPanel").gameObject;
            storePanel = overlaysContainerPanel.transform.Find("StorePanel").gameObject;
            storeItemsShowcasePanel = storePanel.transform.GetChild(0).transform.Find("StoreShowcasePanel").gameObject;
            storeItemCategoriesPanel = storePanel.transform.GetChild(0).transform.Find("StoreCategoriesPanel").gameObject;

            laborExchangePanel = overlaysContainerPanel.transform.Find("LaborExchangePanel").gameObject;
            jobsShowcasePanel = laborExchangePanel.transform.GetChild(0).transform.Find("JobsShowcasePanel").gameObject;
            jobCategoriesPanel = laborExchangePanel.transform.GetChild(0).transform.Find("JobCategoriesPanel").gameObject;
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
            statusEffectsPanel = overlaysContainerPanel.transform.Find("StatusEffectsPanel").gameObject;
            moneyPanel = GameObject.Find("MoneyPanel");
            moodPanel = GameObject.Find("MoodPanel");
            weekProgressBar = GameObject.Find("WeekProgressBar");
            dayProgressBar = GameObject.Find("DayProgressBar");

            dayProgressBarFillImage = GameObject.Find("DayProgressBarFillImage") != null ? GameObject.Find("DayProgressBarFillImage").GetComponent<Image>() : null;
            // Prefabs references
            statusEffectPanelPrefab = Resources.Load("Prefabs/StatusEffects/StatusEffectPanel") as GameObject;
            storeItemCategoryPanelPrefab = Resources.Load("Prefabs/Store/StoreItemCategoryPanel") as GameObject;
            storeItemPanelPrefab = Resources.Load("Prefabs/Store/StoreItemPanel") as GameObject;
            jobCategoryPanelPrefab = Resources.Load("Prefabs/Jobs/JobCategoryPanel") as GameObject;
            jobPanelPrefab = Resources.Load("Prefabs/Jobs/JobPanel") as GameObject;

           
        }
        MapButtonsToActions();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        gameController = GameController.instance;
        storeManager = StoreManager.instance;
        statusEffectsController = StatusEffectsController.instance;
        laborExchangeManager = LaborExchangeManager.instance;

        gameDataManager.OnNewDayStarted += UpdateDayOfWeekProgressBar;
        gameDataManager.OnMoneyValueChanged += UpdateMoneyPanel;
        gameDataManager.OnMoodValueChanged += UpdateMoodPanel;
        storeManager.OnStoreStateChanged += UpdateStoreView;
        statusEffectsController.OnStatusEffectsChanged += UpdateStatusEffectsView;
        laborExchangeManager.OnLaborExchangeStateChanged += UpdateLaborExchangeView;
    }



    private void MapButtonsToActions()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Transform mainMenuButtonsContainer = mainMenuPanel.transform.Find("ButtonsContainer").transform;
            mainMenuCampaignButton = mainMenuButtonsContainer.transform.Find("CampaignButton").GetComponent<Button>();
            mainMenuFreePlayButton = mainMenuButtonsContainer.transform.Find("FreePlayButton").GetComponent<Button>();

            mainMenuCampaignButton.onClick.AddListener(gameManager.OpenCardSelection);
            mainMenuFreePlayButton.onClick.AddListener(gameManager.StartGame);
        } else
        {
            storeButton = GameObject.Find("StoreButton").GetComponent<Button>();
            laborExchangeButton = GameObject.Find("LaborExchangeButton").GetComponent<Button>();
            infoPanelButton = GameObject.Find("InfoPanel").GetComponent<Button>();
            getCreditButtonTEST = GameObject.Find("GetCreditButton").GetComponent<Button>();
            Transform pauseMenuButtonsContainerTrans = pauseMenuPanel.transform.Find("ButtonsContainer").transform;
            pauseMenuResumeButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_Resume").GetComponent<Button>();
            pauseMenuSettingsButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_Settings").GetComponent<Button>();
            pauseMenuMainMenuButton = pauseMenuButtonsContainerTrans.Find("PauseMenuButton_MainMenu").GetComponent<Button>();

            laborExchangeButton.onClick.AddListener(gameManager.ToggleLaborExchange);
            storeButton.onClick.AddListener(gameManager.ToggleStoreMenu);
            infoPanelButton.onClick.AddListener(gameManager.ToggleModifiersInformation);
            getCreditButtonTEST.onClick.AddListener(gameController.GetLoan);
            pauseMenuResumeButton.onClick.AddListener(gameManager.Unpause);
            pauseMenuSettingsButton.onClick.AddListener(delegate { print("Open settings..."); });
            pauseMenuMainMenuButton.onClick.AddListener(gameManager.OpenMainMenu);
        }
        
    }

    /// <summary>
    /// Set UI state and manipulate related UI elements accordingly (like close mod info upon store opening, etc)
    /// </summary>
    /// <param name="state"></param>
    public void SetUIState(UIState state)
    {
        // TODO: improve algo
        switch (state)
        {
            case UIState.MainMenu:
                mainMenuPanel.SetActive(true);
                cardSelectionPanel.SetActive(false);
                loadingPanel.SetActive(false);
                break;
            case UIState.CardSelection:
                mainMenuPanel.SetActive(false);
                cardSelectionPanel.SetActive(true);
                break;
            case UIState.Loading:
                loadingPanel.SetActive(true);
                break;
            case UIState.House:
                loadingPanel.SetActive(false);
                overlaysContainerPanel.SetActive(true);
                statusEffectsPanel.SetActive(false);
                laborExchangePanel.SetActive(false);
                SwitchToHouseStore();
                break;
            case UIState.Store:
                activeStoreView.Update();
                activeStoreView.Show();
                overlaysContainerPanel.SetActive(true);
                statusEffectsPanel.SetActive(false);
                storePanel.SetActive(true);
                laborExchangePanel.SetActive(false);
                break;
            case UIState.ModifiersInfo:

                statusEffectsPanel.SetActive(true);
                storePanel.SetActive(false);
                laborExchangePanel.SetActive(false);
                break;
            case UIState.LaborExchange:

                statusEffectsPanel.SetActive(false);
                storePanel.SetActive(true);
                laborExchangePanel.SetActive(true);
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
        UpdateLaborExchangeView();
        UpdateDayOfWeekProgressBar();
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



    public void UpdateDayOfWeekProgressBar()
    {
        if (weekProgressBar)
        {
            int normalDayOfWeekIndex = ((int)gameDataManager.CurrentDateTime.DayOfWeek == 0) ? 6 : (int)gameDataManager.CurrentDateTime.DayOfWeek - 1;
            // Clear each day indicator
            foreach (Image image in weekProgressBar.GetComponentsInChildren<Image>())
            {
                // Paint only fill image
                if (image.gameObject.name == "DayRBFill")
                {
                    image.color = emptyDayColor;
                }

            }

            // Fill previous days with solid color
            for (int i = 0; i < normalDayOfWeekIndex; i++)
            {
                weekProgressBar.transform.Find("DayRB (" + i.ToString() + ")").Find("DayRBFill").GetComponent<Image>().color = passedDayColor;
            }

            // Fill present day with transparent color
            weekProgressBar.transform.Find("DayRB (" + normalDayOfWeekIndex.ToString() + ")").Find("DayRBFill").GetComponent<Image>().color = presentDayColor;
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
            foreach (StatusEffect statusEffect in statusEffectsController.StatusEffects)
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
                panel.transform.Find("Info").transform.Find("TopPanel").transform.Find("TitleText").GetComponent<Text>().text = statusEffect.Name;
                Transform BottomPanelTransform = panel.transform.Find("Info").transform.Find("BottomPanel");
                BottomPanelTransform.transform.Find("TypePanel").GetComponentInChildren<Text>().text = statusEffect.Freqency.ToString();
                BottomPanelTransform.transform.Find("ValuePanel").GetComponentInChildren<Text>().text = (statusEffect.Value > 0 ? "+" : "") + statusEffect.Value;
            }
        }
    }
   
    

    public void ShowStorePanel(bool state)
    {
        if (state)
        {
            SetUIState(UIState.Store);
        }
        else
        {
            SetUIState(UIState.House);
        }
    }

    public void ToggleStorePanel()
    {
        ShowStorePanel(!storePanel.activeSelf);
    }

    public void SetDayProgress(float value)
    {
        if (dayProgressBarFillImage)
        {
            dayProgressBarFillImage.fillAmount = value;
        }
    }

    public void UpdateLaborExchangeView()
    {
        if (jobCategoriesPanel && jobsShowcasePanel)
        {
            UpdateJobCategoriesPanel();
            UpdateJobsShowcasePanel();
        }
    }

    void UpdateJobCategoriesPanel()
    {
        // TODO: implement empty items array behavior
        foreach (Transform child in jobCategoriesPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<JobCategory> presentedCategories = laborExchangeManager.GetPresentedCategories();

        foreach (JobCategory category in presentedCategories)
        {
            GameObject jobCategoryPanel = (GameObject)Instantiate(jobCategoryPanelPrefab);

            RectTransform mRectTransform = jobCategoryPanel.GetComponent<RectTransform>();
            jobCategoryPanel.GetComponentInChildren<Text>().text = category.ToString();
            jobCategoryPanel.transform.SetParent(jobCategoriesPanel.transform);
            mRectTransform.localScale = new Vector3(1, 1, 1);

            jobCategoryPanel.GetComponent<Button>().onClick.AddListener(delegate () { laborExchangeManager.SelectedCategory = category; print(category); });
        }

        if (!presentedCategories.Contains(laborExchangeManager.SelectedCategory))
        {
            laborExchangeManager.SelectedCategory = presentedCategories[0];
        }
    }

    void UpdateJobsShowcasePanel()
    {
        // TODO: implement empty items array behavior
        foreach (Transform child in jobsShowcasePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Job job in laborExchangeManager.GetAllJobsOfCategory(laborExchangeManager.SelectedCategory))
        {
            GameObject jobPanel = Instantiate(jobPanelPrefab);
            jobPanel.GetComponent<Button>().onClick.AddListener(delegate () { laborExchangeManager.JobPanelClick(job); });
            //itemObject.GetComponentInParent<Text>().text = item.name;
            jobPanel.transform.SetParent(jobsShowcasePanel.transform);
            jobPanel.transform.localScale = new Vector3(1, 1, 1);
            Transform iconTransform = jobPanel.transform.Find("Icon");
            iconTransform.GetComponent<Image>().sprite = job.Sprite != null ? job.Sprite : gameDataManager.placeHolderSprite;
            IncomeData incomeData = job.GetIncomeData();
            string incomeInfoText = "Разовые выплаты: \n";
            incomeInfoText += incomeData.DailyIncome > 0 ? "$" + incomeData.DailyIncome.ToString() + " в день\n" : "";
            incomeInfoText += incomeData.WeeklyIncome > 0 ? "$" + incomeData.WeeklyIncome.ToString() + " в неделю\n" : "";
            incomeInfoText += incomeData.MonthlyIncome > 0 ? "$" + incomeData.MonthlyIncome.ToString() + " в месяц\n" : "";
            incomeInfoText += incomeData.YearlyIncome > 0 ? "$" + incomeData.YearlyIncome.ToString() + " в год\n" : "";
            jobPanel.transform.Find("Income").GetComponentInChildren<Text>().text = incomeInfoText;
            jobPanel.transform.Find("TitleText").GetComponent<Text>().text = job.Title;

            if (laborExchangeManager.IsJobActive(job))
            {
                jobPanel.transform.Find("EquipHighlightPanel").gameObject.SetActive(true);
            }
            else
            {
                jobPanel.transform.Find("EquipHighlightPanel").gameObject.SetActive(false);
            }
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
        ShowLaborExchangePanel(!laborExchangePanel.activeSelf);
    }

    public void SwitchToHouseStore()
    {
        activeStoreView = new DefaultStoreView(storeManager.HouseStoreCatalog, storePanel.transform);
        
    }
}
