using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Controls base gameplay logic
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController instance;
    // Managers, Controllers
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private UIManager uiManager;
    private StoreManager storeManager;
    private HouseManager houseManager;
    private StatusEffectsController statusEffectsManager;
    private HintsManager hintsManager;
    

    float timeSinceDayStart;

    // Jobs
    private List<Job> jobsPool = new List<Job>();
    private List<Job> activeJobs = new List<Job>();

    // Fin ops
    private List<Loan> activeCredits = new List<Loan>();

    // Store catalogs
    private StoreCatalog homeStoreCatalog;


    // Tutorial
    private bool showTutorial = true;


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
        homeStoreCatalog = Resources.Load("ScriptableObjects/Store/HomeStoreCatalog") as StoreCatalog;
        homeStoreCatalog = Instantiate(homeStoreCatalog) as StoreCatalog;
        homeStoreCatalog.Init();
        storeManager.HouseStoreCatalog = homeStoreCatalog;
    }

    private void Init()
    {
        storeManager = StoreManager.instance;
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        uiManager = UIManager.instance;
        houseManager = HouseManager.instance;
        statusEffectsManager = StatusEffectsController.instance;
        hintsManager = HintsManager.instance;

        gameDataManager.Money = gameManager.GameMode.money;
        gameDataManager.Mood = gameManager.GameMode.mood;
        gameDataManager.Age = gameManager.GameMode.age;
        gameDataManager.IsRecordingIncome = true;

        gameManager.OnGameStarted += OnGameStarted;
        gameDataManager.OnNewDayStarted += TickDay;
        gameDataManager.OnNewWeekStarted += TickWeek;
        gameDataManager.OnNewMonthStarted += TickMonth;
        gameDataManager.OnNewYearStarted += TickYear;
        gameDataManager.OnBirthday += delegate { hintsManager.ShowHint("С днем рождения!", "Сегодня у вас день рождения, поздавляем вас и желаем счастья и денег :)"); };

        uiManager.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GameStateP == GameManager.GameState.InGame)
        {
            // TODO: implement as event from dataManager
            uiManager.SetDayProgress(timeSinceDayStart / gameManager.GameMode.dayDuration);
            timeSinceDayStart += Time.deltaTime;
            if (timeSinceDayStart > gameManager.GameMode.dayDuration)
            {
                gameDataManager.AddDay();
                timeSinceDayStart = 0;

                // TODO: implement as event from dataManager
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

    }

    private void TickWeek()
    {

    }

    private void TickMonth()
    {

    }
    private void TickYear()
    {

    }

    private void OnGameStarted(GameMode gameMode)
    {
        InitJobs();
    }

    public void GetLoan()
    {
        Loan.LoanBuilder builder = new Loan.LoanBuilder();
        Loan loan = builder.SetRate(0.16f)
            .SetInitialValue(5000f)
            .SetPeriod(100)
            .Build();
        loan.StatusEffects.Add(new StatusEffect("Выплата кредита", -loan.GetMonthlyPaymentValue(), StatusEffectType.Money, StatusEffectFrequency.Monthly));
        ActivateLoan(loan);
    }

    private void ActivateLoan(Loan loan)
    {
        gameDataManager.Money += loan.InitialValue;
        activeCredits.Add(loan);
        statusEffectsManager.AddStatusEffects(loan.StatusEffects);
    }
}
