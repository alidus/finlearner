using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    // Managers, Controllers
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private UIManager uiManager;
    private StoreController storeManager;
    private HouseManager houseManager;
    private StatusEffectsController statusEffectsManager;
    

    float timeSinceDayStart;
    // Modifiers
    private StatusEffectContainer activeStatusEffects = new StatusEffectContainer();


    // Jobs
    private List<Job> jobsPool = new List<Job>();
    private List<Job> activeJobs = new List<Job>();

    // Fin ops
    private List<Credit> activeCredits = new List<Credit>();

    // Store catalogs
    public StoreCatalog homeStoreCatalog;

    // Store managers

    // Prefabs
    public GameObject homeStoreCategoryButtonPrefab;

    // Cashing
    public Dictionary<ItemType, StoreItem> selectedObjectPerItemType;

    // Events
    public delegate void TimeIntervalTickAction();
    public event TimeIntervalTickAction OnDailyTick;
    public event TimeIntervalTickAction OnWeeklyTick;
    public event TimeIntervalTickAction OnMonthlyTick;
    public event TimeIntervalTickAction OnYearlyTick;


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

        UpdateReferences();
        GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnEnable()
    {
        
    }

    public void UpdateReferences()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        homeStoreCatalog = Instantiate(homeStoreCatalog) as StoreCatalog;
        homeStoreCatalog.Init();
        storeManager.HomeStoreCatalog = homeStoreCatalog;

        gameDataManager.Money = gameManager.GameMode.money;
        gameDataManager.Mood = gameManager.GameMode.mood;
        gameDataManager.Age = gameManager.GameMode.age;

        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            //TODO: Add selectable items to list
        }
    }

    private void Init()
    {
        storeManager = StoreController.instance;
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        uiManager = UIManager.instance;
        houseManager = HouseManager.instance;
        statusEffectsManager = StatusEffectsController.instance;

        uiManager.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameState() == GameManager.GameState.InGame)
        {
            uiManager.SetDayProgress(timeSinceDayStart / gameManager.GameMode.dayDuration);
            timeSinceDayStart += Time.deltaTime;
            if (timeSinceDayStart > gameManager.GameMode.dayDuration)
            {
                TickDay();
                timeSinceDayStart = 0;

                if (gameDataManager.DayCount - gameDataManager.Age * 365 > 365)
                {
                    gameDataManager.Age += 1;
                }
                uiManager.UpdateInfoPanel();
            }
        }
    }

   

    private void InitJobs()
    {
        foreach (UnityEngine.Object job in Resources.LoadAll("ScriptableObjects/Jobs"))
        {
            jobsPool.Add(new Job((JobSOTemplate)job));
            print(job.name);
        }

        // DEBUG
        ActivateJob(jobsPool[0]);
    }



    private void ActivateJob(Job job)
    {
        activeJobs.Add(job);
        statusEffectsManager.AddStatusEffects(job.StatusEffects);
    }

    private void DeactivateJob(Job job)
    {
        activeJobs.Remove(job);
    }

    private void TickDay()
    {
        gameDataManager.AddDayOfWeek();
        if (gameDataManager.DayOfWeekIndex == 0)
        {
            TickWeek();
        }
        OnDailyTick();
        
        gameDataManager.AddToDayCounter();
        
    }

    private void TickWeek()
    {
        OnWeeklyTick();
    }


    private void OnGameStarted(GameMode gameMode)
    {
        InitJobs();
    }

    public void TakeTestCredit()
    {
        Credit credit = Credit.Create(5000, 100, 0.13f);
        credit.StatusEffects.Add(new StatusEffect("У вас кредит :((", -credit.GetMonthlyPaymentAmount(), StatusEffectType.Money, StatusEffectFrequency.Monthly));

        ActivateCredit(credit);
    }

    private void ActivateCredit(Credit credit)
    {
        gameDataManager.Money += credit.LoanTotalAmount;
        activeCredits.Add(credit);
        statusEffectsManager.AddStatusEffects(credit.StatusEffects);
    }
}
