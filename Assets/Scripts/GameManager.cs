using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameController gameController;

    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    private GameDataManager gameDataManager;

    public GameObject mainMenuPage;
    public GameObject cardSelectionPage;
    public GameObject inGameHud;

    public GameObject storePanel;

    public GameObject modifierPanelPrefab;

    public GameObject infoPanel;
    private GameObject flat;
    public GameObject modifiersInfoPanel;

    // Money panel
    private GameObject moneyPanel;

    // Mood panel
    private GameObject moodPanel;

    // Week days progress bar
    private GameObject weekProgressBar;
    Color passedDayColor = new Color(1, 1, 1, 1);
    Color presentDayColor = new Color(1, 1, 1, 0.3f);

    // Action delegates and events
    public delegate void GameDelegate(GameMode gameMode);
    public static event GameDelegate OnGameStarted;
    //public static event GameDelegate OnGameOver;
    public delegate void StoreOpenAction();
    public static event StoreOpenAction OnStoreButtonPress;

    // Misc
    public Sprite placeHolder;

    private GameMode gameMode;
    public GameMode GameMode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }
    

    enum UIState
    {
        MainMenu,
        CardSelection,
        Home,
        Store,
        ModifiersInfo
    }

    private void OnEnable()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }

        
        Init();
        flat = GameObject.Find("Flat");
        weekProgressBar = GameObject.Find("WeekProgressBar");
        moneyPanel = GameObject.Find("MoneyPanel");
        moodPanel = GameObject.Find("MoodPanel");
        gameDataManager = GetComponent<GameDataManager>();
        GameMode = (GameMode)Resources.Load("ScriptableObjects/GameModes/FreePlayGM");

        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    private void Init()
    {
        gameDataManager = GameDataManager.Instance;
        gameController = GetComponent<GameController>();
        gameController.OnModifierAdded += AddModifierPanel;
    }


    void SetUIState(UIState State)
    {
        switch (State)
        {
            case UIState.MainMenu:
                mainMenuPage.SetActive(true);
                cardSelectionPage.SetActive(false);
                inGameHud.SetActive(false);
                break;
            case UIState.CardSelection:
                mainMenuPage.SetActive(false);
                cardSelectionPage.SetActive(true);
                inGameHud.SetActive(false);
                break;
            case UIState.Home:
                mainMenuPage.SetActive(false);
                cardSelectionPage.SetActive(false);
                inGameHud.SetActive(true);
                break;
            case UIState.Store:
                modifiersInfoPanel.SetActive(false);
                storePanel.SetActive(true);
                break;
            case UIState.ModifiersInfo:
                modifiersInfoPanel.SetActive(true);
                storePanel.SetActive(false);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Update each UI element, slow
    /// </summary>
    public void UpdateUI()
    {
        UpdateMoneyPanel();
        UpdateMoodPanel();
        UpdateFlatAppearance();
        UpdateInfoPanel();
    }

    public void OpenMainMenu()
    {
        SetUIState(UIState.MainMenu);
    }

    public void OpenCardSelection()
    {
        SetUIState(UIState.CardSelection);
    }

    public void StartGame(GameMode gameMode)
    {
        SetUIState(UIState.Home);

        OnGameStarted(gameMode);
    }

    public void PlayButtonOnClick(int gameModeIndex)
    {
        
    }

    public void UpdateInfoPanel()
    {
        infoPanel.transform.Find("Age").GetComponent<Text>().text = gameDataManager.Age.ToString() + " лет";
        infoPanel.transform.Find("Day").GetComponent<Text>().text = "День: " + gameDataManager.DayCount.ToString();
        infoPanel.transform.Find("Income").GetComponent<Text>().text = "Доход:\n$" + gameDataManager.DailyIncome.ToString();
    }

    public void UpdateFlatAppearance()
    {
        foreach (Transform child in flat.transform)
        {
            switch (child.name)
            {
                case "Bed":
                    child.GetComponent<SpriteRenderer>().sprite = HomeSettings.Bed != null ? HomeSettings.Bed.Sprite : null;
                    break;
                case "Chair":
                    child.GetComponent<SpriteRenderer>().sprite = HomeSettings.Chair != null ? HomeSettings.Chair.Sprite : null;
                    break;
                case "Armchair":
                    child.GetComponent<SpriteRenderer>().sprite = HomeSettings.Armchair != null ? HomeSettings.Armchair.Sprite : null;
                    break;
                case "Table":
                    child.GetComponent<SpriteRenderer>().sprite = HomeSettings.Table != null ? HomeSettings.Table.Sprite : null;
                    break;
                default:
                    break;
            }
        }
    }

    public void UpdateMoneyPanel()
    {
        moneyPanel.GetComponentInChildren<Text>().text = "$" + gameDataManager.Money.ToString();
    }

    public void UpdateMoodPanel()
    {
        moodPanel.GetComponentInChildren<Text>().text = gameDataManager.Mood.ToString();
        if (gameDataManager.Mood > 0)
        {
            float coef = (float)gameDataManager.Mood / 100;
            moodPanel.GetComponentInChildren<Text>().color = new Color(1, coef, coef, 1);
        }
       
    }

    public void ShowModifiersInfoPanel(bool state)
    {
        if (state)
        {
            SetUIState(UIState.ModifiersInfo);
        } else
        {
            SetUIState(UIState.Home);
        }
        modifiersInfoPanel.SetActive(state);
        
    }

    public void ToggleModifiersInfoPanel()
    {
        if (modifiersInfoPanel.activeSelf)
        {
            ShowModifiersInfoPanel(false);
        }
        else
        {
            ShowModifiersInfoPanel(true);
        }
    }

    public void AddModifierPanel(Modifier modifier)
    {
        Transform[] children = modifiersInfoPanel.transform.GetComponentsInChildren<Transform>();
        GameObject panel = Instantiate(modifierPanelPrefab);
        string ContentPanelName = "";
        switch (modifier.Type)
        {
            case ModifierType.Money:
                ContentPanelName = "MoneyModsContent";
                break;
            case ModifierType.Mood:
                ContentPanelName = "MoodModsContent";
                break;
            default:
                break;
        }

        // Iterate through children of modifiers panel and find content panels of each mod type containing array of modifiers
        foreach (Transform child in children)
        {
            if (child.name == ContentPanelName)
            {
                panel.transform.SetParent(child);
                panel.transform.localScale = new Vector3(1, 1, 1);
                Transform titleTextTransform = panel.transform.Find("Title");
                titleTextTransform.GetComponent<Text>().text = modifier.Name;
                panel.transform.Find("TypePanel").GetComponentInChildren<Text>().text = modifier.Type.ToString();
                panel.transform.Find("ValuePanel").GetComponentInChildren<Text>().text = (modifier.Value > 0 ? "+" : "") + modifier.Value;
            }
        }
    }

    public void AddDayToWeekProgressIndicator()
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
        } else {
            Image prevImgComp = weekProgressBar.transform.Find("DayRB (" + (gameDataManager.DayOfWeekIndex).ToString() + ")").Find("DayRBFill").GetComponent<Image>();
            prevImgComp.color = passedDayColor;
            Image imgComp = weekProgressBar.transform.Find("DayRB (" + (gameDataManager.DayOfWeekIndex + 1).ToString() + ")").Find("DayRBFill").GetComponent<Image>();
            imgComp.color = presentDayColor;
            imgComp.gameObject.SetActive(true);

        }

        
    }

    public void ShowStore()
    {
        OnStoreButtonPress();
    }
}
