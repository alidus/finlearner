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
    private StoreController storeController;
    private StatusEffectsController statusEffectsController;

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
    private GameObject storeShowcasePanel;
    private GameObject storeCategoriesPanel;


    // Buttons
    private Button storeButton;
    private Button infoPanelButton;
    private Button getCreditButtonTEST;
    // Prefabs
    public GameObject StatusEffectPanelPrefab;
    public GameObject categoryButtonPrefab;
    public GameObject storeItemPrefab;

    private Image dayProgressBarFillImage;


    // Colors
    Color passedDayColor = new Color(1, 1, 1, 1);
    Color presentDayColor = new Color(1, 1, 1, 0.3f);

    // Other
    Dictionary<StatusEffect, GameObject> modifiersPanelDictionary = new Dictionary<StatusEffect, GameObject>();

    public enum UIState
    {
        MainMenu,
        CardSelection,
        Loading,
        House,
        Store,
        ModifiersInfo
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
            storeShowcasePanel = storePanel.transform.GetChild(0).transform.Find("StoreShowcasePanel").gameObject;
            storeCategoriesPanel = storePanel.transform.GetChild(0).transform.Find("StoreCategoriesPanel").gameObject;
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
        storeController = StoreController.instance;
        statusEffectsController = StatusEffectsController.instance;

        gameController.OnDailyTick += UpdateDayProgressBar;
        gameDataManager.OnMoneyValueChanged += UpdateMoneyPanel;
        gameDataManager.OnMoodValueChanged += UpdateMoodPanel;
        storeController.OnStoreStateChanged += UpdateStoreView;
        statusEffectsController.OnStatusEffectsChanged += UpdateStatusEffectsView;
    }



    private void MapButtonsToActions()
    {
        storeButton = GameObject.Find("StoreButton").GetComponent<Button>();
        infoPanelButton = GameObject.Find("InfoPanel").GetComponent<Button>();
        getCreditButtonTEST = GameObject.Find("GetCreditButton").GetComponent<Button>();

        storeButton.onClick.AddListener(gameManager.ToggleStoreMenu);
        infoPanelButton.onClick.AddListener(gameManager.ToggleModifiersInformation);
        getCreditButtonTEST.onClick.AddListener(gameController.TakeTestCredit);
    }

    /// <summary>
    /// Set UI state and manipulate related UI elements accordingly (like close mod info upon store opening, etc)
    /// </summary>
    /// <param name="state"></param>
    public void SetUIState(UIState state)
    {
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
                storePanel.SetActive(false);
                break;
            case UIState.Store:
                overlaysContainerPanel.SetActive(true);
                statusEffectsPanel.SetActive(false);
                storePanel.SetActive(true);
                break;
            case UIState.ModifiersInfo:
                overlaysContainerPanel.SetActive(true);
                statusEffectsPanel.SetActive(true);
                storePanel.SetActive(false);
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
    }

    public void UpdateInfoPanel()
    {
        if (infoPanel)
        {
            infoPanel.transform.Find("Age").GetComponent<Text>().text = gameDataManager.Age.ToString() + " лет";
            infoPanel.transform.Find("Day").GetComponent<Text>().text = "День: " + gameDataManager.DayCount.ToString();
            infoPanel.transform.Find("Income").GetComponent<Text>().text = "Доход:\n$" + gameDataManager.DailyIncome.ToString();
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



    public void UpdateDayProgressBar()
    {
        if (dayProgressBar)
        {
            if (gameDataManager.DayOfWeekIndex == 0)
            {
                foreach (Image imgComp in weekProgressBar.GetComponentsInChildren<Image>())
                {
                    if (imgComp.transform.parent.gameObject.name == "DayRB (1)")
                    {
                        imgComp.color = presentDayColor;
                        continue;
                    }

                    if (imgComp.gameObject.name == "DayRBFill")
                    {
                        imgComp.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                Image prevImgComp = weekProgressBar.transform.Find("DayRB (" + (gameDataManager.DayOfWeekIndex).ToString() + ")").Find("DayRBFill").GetComponent<Image>();
                prevImgComp.color = passedDayColor;
                Image imgComp = weekProgressBar.transform.Find("DayRB (" + (gameDataManager.DayOfWeekIndex + 1).ToString() + ")").Find("DayRBFill").GetComponent<Image>();
                imgComp.color = presentDayColor;
                imgComp.gameObject.SetActive(true);
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
            foreach (StatusEffect statusEffect in statusEffectsController.StatusEffects)
            {
                GameObject panel = Instantiate(StatusEffectPanelPrefab);
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
        UpdateStoreCategoriesPanel();
        UpdateStoreShowcasePanel();
    }

    void UpdateStoreCategoriesPanel()
    {
        foreach (Transform child in storeCategoriesPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<ItemCategory> presentedCategories = storeController.ActiveCatalog.GetCategories();

        foreach (ItemCategory category in presentedCategories)
        {
            GameObject categoryButtonObject = (GameObject)Instantiate(categoryButtonPrefab);

            RectTransform mRectTransform = categoryButtonObject.GetComponent<RectTransform>();
            categoryButtonObject.GetComponentInChildren<Text>().text = category.ToString();
            categoryButtonObject.transform.SetParent(storeCategoriesPanel.transform);
            mRectTransform.localScale = new Vector3(1, 1, 1);

            categoryButtonObject.GetComponent<Button>().onClick.AddListener(delegate () { storeController.SelectedCategory = category; });
        }

        if (presentedCategories.Count > 0)
        {
            storeController.SelectedCategory = 0;
        }
    }

    void UpdateStoreShowcasePanel()
    {
        // Clear store item panels array
        foreach (Transform child in storeShowcasePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (StoreItem item in storeController.ActiveCatalog.GetAllItemsOfCategory(storeController.SelectedCategory))
        {
            GameObject itemObject = Instantiate(storeItemPrefab);
            itemObject.GetComponent<Button>().onClick.AddListener(delegate () { storeController.StoreItemClick(item); });
            //itemObject.GetComponentInParent<Text>().text = item.name;
            itemObject.transform.SetParent(storeShowcasePanel.transform);
            itemObject.transform.localScale = new Vector3(1, 1, 1);
            Transform iconTransform = itemObject.transform.Find("Icon");
            iconTransform.transform.Find("PriceTag").GetComponentInChildren<Text>().text = "$" + item.Price.ToString();
            itemObject.transform.Find("TitleText").GetComponent<Text>().text = item.Name;
            iconTransform.GetComponent<Image>().sprite = item.Sprite != null ? item.Sprite : gameManager.placeHolder;

            if (item.IsEquiped)
            {
                itemObject.transform.Find("EquipHighlightPanel").gameObject.SetActive(true);
            }
            else
            {
                itemObject.transform.Find("EquipHighlightPanel").gameObject.SetActive(false);
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
        storePanel.SetActive(state);
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
}
