﻿using System.Collections;
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

    // UI elements
    private GameObject mainMenuPanel;
    private GameObject cardSelectionPanel;
    private GameObject loadingPanel;
    private GameObject storePanel;
    private GameObject infoPanel;
    private GameObject StatusEffectsPanel;
    private GameObject moneyPanel;
    private GameObject moodPanel;
    private GameObject weekProgressBar;
    private GameObject dayProgressBar;
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
        mainMenuPanel = GameObject.Find("MainMenuPanel");
        cardSelectionPanel = GameObject.Find("CardSelectionPanel");
        loadingPanel = GameObject.Find("LoadingPanel");
        storePanel = GameObject.Find("StorePanel");
        infoPanel = GameObject.Find("InfoPanel");
        StatusEffectsPanel = GameObject.Find("StatusEffectsPanel");
        moneyPanel = GameObject.Find("MoneyPanel");
        moodPanel = GameObject.Find("MoodPanel");
        weekProgressBar = GameObject.Find("WeekProgressBar");
        dayProgressBar = GameObject.Find("DayProgressBar");
        dayProgressBarFillImage = GameObject.Find("DayProgressBarFillImage") != null ? GameObject.Find("DayProgressBarFillImage").GetComponent<Image>() : null ;
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

        gameController.OnDailyTick += UpdateDayProgressBar;
        gameDataManager.OnMoneyValueChanged += UpdateMoneyPanel;
        gameDataManager.OnMoodValueChanged += UpdateMoodPanel;
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
                StatusEffectsPanel.SetActive(false);
                storePanel.SetActive(false);
                break;
            case UIState.Store:
                StatusEffectsPanel.SetActive(false);
                storePanel.SetActive(true);
                break;
            case UIState.ModifiersInfo:
                StatusEffectsPanel.SetActive(true);
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
        StatusEffectsPanel.SetActive(state);
    }

    public void ToggleModifiersInfoPanel()
    {
        if (StatusEffectsPanel.activeSelf)
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
