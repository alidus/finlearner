using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class GameplayHUD : MonoBehaviour
{
    GameManager gameManager;
    UIManager uiManager;
    GameDataManager gameDataManager;

    private MoneyPanelView moneyPanelView;
    private MoodPanelView moodPanelView;
    private GameObject weekProgressBar;
    private GameObject dayProgressBar;
    private GameObject infoPanel;

    private Button storeButton;
    private Button infoPanelButton;
    private Button getCreditButtonTEST;
    private Button jobExchangeButton;
    private Button educationHubButton;
    private Button inventoryButton;


    private Image dayProgressBarFillImage;

    // Colors
    [SerializeField]
    Color weekProgressDayIndicatorColor = new Color(1, 1, 1, 0.3f);
    [SerializeField]
    Color weekProgressWeekendDayIndicatorColor = new Color(1, 1, 1, 0.3f);

    private void Awake()
    {
        UpdateReferences();
    }

    private void UpdateReferences()
    {
        moneyPanelView = GameObject.Find("MoneyPanel").GetComponent<MoneyPanelView>();
        moodPanelView = GameObject.Find("MoodPanel").GetComponent<MoodPanelView>();
        infoPanel = GameObject.Find("InfoPanel");
        weekProgressBar = GameObject.Find("WeekProgressBar");
        dayProgressBar = GameObject.Find("DayProgressBar");
        storeButton = GameObject.Find("StoreButton")?.GetComponent<Button>();
        jobExchangeButton = GameObject.Find("JobExchangeButton")?.GetComponent<Button>();
        educationHubButton = GameObject.Find("EducationHubButton")?.GetComponent<Button>();
        inventoryButton = GameObject.Find("InventoryButton")?.GetComponent<Button>();
        infoPanelButton = GameObject.Find("InfoPanel")?.GetComponent<Button>();
        dayProgressBarFillImage = GameObject.Find("DayProgressBarFillImage")?.GetComponent<Image>();
        getCreditButtonTEST = GameObject.Find("GetCreditButton")?.GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        gameManager = GameManager.instance;
        uiManager = UIManager.instance;
        gameDataManager = GameDataManager.instance;

        storeButton.onClick.RemoveAllListeners();
        storeButton.onClick.AddListener(delegate
        {
            if (uiManager.State == GameplayUIState.Store)
            {
                uiManager.SetGamplayUIState(GameplayUIState.Home);
            }
            else
            {
                uiManager.SetGamplayUIState(GameplayUIState.Store);
            }
        });

        jobExchangeButton.onClick.RemoveAllListeners();
        jobExchangeButton.onClick.AddListener(delegate
        {
            if (uiManager.State == GameplayUIState.JobExchange)
            {
                uiManager.SetGamplayUIState(GameplayUIState.Home);
            } else
            {
                uiManager.SetGamplayUIState(GameplayUIState.JobExchange);
            }
        });

        educationHubButton.onClick.RemoveAllListeners();
        educationHubButton.onClick.AddListener(delegate
        {
            if (uiManager.State == GameplayUIState.EducationHub)
            {
                uiManager.SetGamplayUIState(GameplayUIState.Home);
            }
            else
            {
                uiManager.SetGamplayUIState(GameplayUIState.EducationHub);
            }
        });

        inventoryButton.onClick.RemoveAllListeners();
        inventoryButton.onClick.AddListener(delegate
        {
            if (uiManager.State == GameplayUIState.Inventory)
            {
                uiManager.SetGamplayUIState(GameplayUIState.Home);
            }
            else
            {
                uiManager.SetGamplayUIState(GameplayUIState.Inventory);
            }
        });

        infoPanelButton.onClick.RemoveAllListeners();
        infoPanelButton.onClick.AddListener(delegate
        {
            if (uiManager.State == GameplayUIState.StatisticsHub)
            {
                uiManager.SetGamplayUIState(GameplayUIState.Home);
            }
            else
            {
                uiManager.SetGamplayUIState(GameplayUIState.StatisticsHub);
            }
        });



        gameDataManager.OnMoneyValueChanged += UpdateMoneyPanel;
        gameDataManager.OnMoodValueChanged += UpdateMoodPanel;
        gameDataManager.OnDayStarted += UpdateInfoPanel;
        gameDataManager.OnDayStarted += UpdateWeekProgressBar;
        gameDataManager.OnDayProgressChanged += UpdateDayProgressBar;
    }

    public void UpdateHUD()
    {
        UpdateMoneyPanel();
        UpdateMoodPanel();
        UpdateInfoPanel();
        UpdateWeekProgressBar();
    }

    public void UpdateMoneyPanel(float deltaValue = 0)
    {
        if (moneyPanelView != null)
        {
            moneyPanelView.Value = (int)gameDataManager.Money;
            moneyPanelView.UpdateView();
            if (deltaValue != 0)
            {
                if (deltaValue > 0)
                    moneyPanelView.PlayAddAnim();
                else
                    moneyPanelView.PlaySubAnim();
            }
        }
    }

    public void UpdateMoodPanel(float deltaValue = 0)
    {
        if (moodPanelView != null)
        {
            moodPanelView.Value = (int)gameDataManager.Mood;
            moodPanelView.UpdateView();
            if (deltaValue != 0)
            {
                if (deltaValue > 0)
                    moodPanelView.PlayAddAnim();
                else
                    moodPanelView.PlaySubAnim();
            }
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
                    light.color = weekProgressWeekendDayIndicatorColor;
                    fillImage.color = weekProgressWeekendDayIndicatorColor;
                }
                else
                {
                    light.color = weekProgressDayIndicatorColor;
                    fillImage.color = weekProgressDayIndicatorColor;
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

    public void UpdateDayProgressBar()
    {
        if (dayProgressBarFillImage)
        {
            dayProgressBarFillImage.fillAmount = gameDataManager.DayProgress;
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
}
