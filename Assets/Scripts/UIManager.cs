using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private StoreManager storeController;
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
    private GameObject gameplayHUDPanel;
    private GameObject overlaysContainerPanel;
    private GameObject moneyStatusEffectsContentPanel;
    private GameObject moodStatusEffectsContentPanel;
    // Store
    private GameObject storePanel;
    private GameObject storeItemsShowcasePanel;
    private GameObject storeItemCategoriesPanel;
    // Labor exchange
    private GameObject laborExchangePanel;
    private GameObject jobsShowcasePanel;
    private GameObject jobCategoriesPanel;


    // Buttons
    private Button storeButton;
    private Button infoPanelButton;
    private Button getCreditButtonTEST;
    private Button laborExchangeButton;
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
        } else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        UpdateReferences();
    }

    public void UpdateReferences()
    {
        uiCanvas = GameObject.Find("UICanvas");
        // Update references to UI elements based on game state
        if (gameManager == null || gameManager.GameStateP == GameManager.GameState.MainMenu)
        {
            mainMenuPanel = uiCanvas.transform.Find("MainMenuPanel").gameObject;
            cardSelectionPanel = uiCanvas.transform.Find("CardSelectionPanel").gameObject;
            loadingPanel = uiCanvas.transform.Find("LoadingPanel").gameObject;
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

            MapButtonsToActions();
        }
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
        storeController = StoreManager.instance;
        statusEffectsController = StatusEffectsController.instance;
        laborExchangeManager = LaborExchangeManager.instance;

        gameDataManager.OnNewDayStarted += UpdateDayOfWeekProgressBar;
        gameDataManager.OnMoneyValueChanged += UpdateMoneyPanel;
        gameDataManager.OnMoodValueChanged += UpdateMoodPanel;
        storeController.OnStoreStateChanged += UpdateStoreView;
        statusEffectsController.OnStatusEffectsChanged += UpdateStatusEffectsView;
        laborExchangeManager.OnLaborExchangeStateChanged += UpdateLaborExchangeView;
    }



    private void MapButtonsToActions()
    {
        storeButton = GameObject.Find("StoreButton").GetComponent<Button>();
        laborExchangeButton = GameObject.Find("LaborExchangeButton").GetComponent<Button>();
        infoPanelButton = GameObject.Find("InfoPanel").GetComponent<Button>();
        getCreditButtonTEST = GameObject.Find("GetCreditButton").GetComponent<Button>();

        laborExchangeButton.onClick.AddListener(gameManager.ToggleLaborExchange);
        storeButton.onClick.AddListener(gameManager.ToggleStoreMenu);
        infoPanelButton.onClick.AddListener(gameManager.ToggleModifiersInformation);
        getCreditButtonTEST.onClick.AddListener(gameController.GetLoan);
    }

    /// <summary>
    /// Set UI state and manipulate related UI elements accordingly (like close mod info upon store opening, etc)
    /// </summary>
    /// <param name="state"></param>
    public void SetUIState(UIState state)
    {
        // TODO: improve algorythm
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
                mainMenuPanel.SetActive(false);
                cardSelectionPanel.SetActive(false);
                loadingPanel.SetActive(true);
                break;
            case UIState.House:
                overlaysContainerPanel.SetActive(false);
                statusEffectsPanel.SetActive(false);
                laborExchangePanel.SetActive(false);
                storePanel.SetActive(false);
                break;
            case UIState.Store:
                UpdateStoreView();
                overlaysContainerPanel.SetActive(true);
                statusEffectsPanel.SetActive(false);
                storePanel.SetActive(true);
                laborExchangePanel.SetActive(false);
                break;
            case UIState.ModifiersInfo:
                overlaysContainerPanel.SetActive(true);
                statusEffectsPanel.SetActive(true);
                storePanel.SetActive(false);
                laborExchangePanel.SetActive(false);
                break;
            case UIState.LaborExchange:
                overlaysContainerPanel.SetActive(true);
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


    public void UpdateStoreView()
    {
        if (storeItemCategoriesPanel && storeItemsShowcasePanel)
        {
            UpdateStoreCategoriesPanel();
            UpdateStoreShowcasePanel();
        }
    }

    void UpdateStoreCategoriesPanel()
    {
        // TODO: implement empty items array behavior
        foreach (Transform child in storeItemCategoriesPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<ItemCategory> presentedCategories = storeController.ActiveCatalog.GetPresentedCategories();

        foreach (ItemCategory category in presentedCategories)
        {
            GameObject storeItemCategoryPanel = (GameObject)Instantiate(storeItemCategoryPanelPrefab);

            RectTransform mRectTransform = storeItemCategoryPanel.GetComponent<RectTransform>();
            storeItemCategoryPanel.GetComponentInChildren<Text>().text = category.ToString();
            storeItemCategoryPanel.transform.SetParent(storeItemCategoriesPanel.transform);
            mRectTransform.localScale = new Vector3(1, 1, 1);

            storeItemCategoryPanel.GetComponent<Button>().onClick.AddListener(delegate () { storeController.SelectedCategory = category; });
        }

        if (presentedCategories.Count > 0)
        {
            storeController.SelectedCategory = 0;
        }
    }

    void UpdateStoreShowcasePanel()
    {
        // TODO: implement empty items array behavior
        // Clear store item panels array
        foreach (Transform child in storeItemsShowcasePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (StoreItem item in storeController.ActiveCatalog.GetAllItemsOfCategory(storeController.SelectedCategory))
        {
            GameObject storeItemPanel = Instantiate(storeItemPanelPrefab);
            storeItemPanel.GetComponent<Button>().onClick.AddListener(delegate () { storeController.StoreItemClick(item); });
            //itemObject.GetComponentInParent<Text>().text = item.name;
            storeItemPanel.transform.SetParent(storeItemsShowcasePanel.transform);
            storeItemPanel.transform.localScale = new Vector3(1, 1, 1);
            Transform iconTransform = storeItemPanel.transform.Find("Icon");
            iconTransform.transform.Find("PriceTag").GetComponentInChildren<Text>().text = "$" + item.Price.ToString();
            storeItemPanel.transform.Find("TitleText").GetComponent<Text>().text = item.Name;
            iconTransform.GetComponent<Image>().sprite = item.Sprite != null ? item.Sprite : gameDataManager.placeHolderSprite;

            if (item.IsEquiped)
            {
                storeItemPanel.transform.Find("EquipHighlightPanel").gameObject.SetActive(true);
            }
            else
            {
                storeItemPanel.transform.Find("EquipHighlightPanel").gameObject.SetActive(false);
            }

            if (item.IsOwned)
            {
                iconTransform.transform.Find("OwnIndicator").gameObject.SetActive(true);
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

            jobCategoryPanel.GetComponent<Button>().onClick.AddListener(delegate () { laborExchangeManager.SelectedCategory = category; });
        }

        if (presentedCategories.Count > 0)
        {
            laborExchangeManager.SelectedCategory = 0;
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
            jobPanel.transform.Find("TitleText").GetComponent<Text>().text = job.Title;
            iconTransform.GetComponent<Image>().sprite = job.Sprite != null ? job.Sprite : gameDataManager.placeHolderSprite;
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
}
