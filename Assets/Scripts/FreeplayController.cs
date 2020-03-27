using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static GameEvent;

/// <summary>
/// Controls base gameplay logic
/// </summary>
public class FreeplayController : AbstractController
{
    // Managers, Controllers
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private UIManager uiManager;
    private InventoryManager storeManager;
    private EnvironmentManager houseManager;
    private StatusEffectsManager statusEffectsManager;
    private HintsManager hintsManager;
    private LaborExchangeManager laborExchangeManager;
    private GameEventsHandler gameEventsHandler;


    // Tutorial
    private bool showTutorial = true;

    private void Awake()
    {
        UpdateReferences();
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateReferences()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GenerateStartEvents();
    }

  

    public override void Init()
    {
        base.Init();
        storeManager = InventoryManager.instance;
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        uiManager = UIManager.instance;
        houseManager = EnvironmentManager.instance;
        statusEffectsManager = StatusEffectsManager.instance;
        hintsManager = HintsManager.instance;
        laborExchangeManager = LaborExchangeManager.instance;
        gameEventsHandler = GameEventsHandler.instance;
        //gameDataManager.OnNewDayStarted += TickDay;
        //gameDataManager.OnNewWeekStarted += TickWeek;
        //gameDataManager.OnNewMonthStarted += TickMonth;
        //gameDataManager.OnNewYearStarted += TickYear;
    }

    private void GenerateStartEvents()
    {
        if (gameEventsHandler)
        {
            // Birthday event
            GameEventBuilder gameEventBuilder = new GameEventBuilder();
            GameEvent birthdayGameEvent = gameEventBuilder.
                SetTitle("С днем рождения!").
                SetDescription("Сегодня у вас день рождения, поздавляем вас и желаем счастья и денег :)").
                Build();

            gameEventsHandler.ExecuteEventAtDate(birthdayGameEvent, gameDataManager.BirthdayDate);

            // Random money from friends (Test event system)
            for (int i = 0; i < 20; i++)
            {
                GameEvent friendGiveMoneyGameEvent = gameEventBuilder.New().
                    SetTitle("Ваш друг дал вам немного денег").
                    SetDescription("Хорошие у вас друзья...").
                    AddStatusEffect(new StatusEffect("Деньги от друга", UnityEngine.Random.Range(50, 200), StatusEffectType.Money, StatusEffectFrequency.OneShot)).
                    Build();

                gameEventsHandler.ExecuteEventBetweenDates(friendGiveMoneyGameEvent, gameDataManager.CurrentDateTime, gameDataManager.CurrentDateTime.AddDays(365));
            }
        }
    }

    void FixedUpdate()
    {
        if (gameManager.ActiveController == this)
        {
            if (gameManager.GameState == GameState.InGame)
            {
                gameDataManager.DayProgress += Time.deltaTime * (gameDataManager.HoursPerSecond / 24);

                if (gameDataManager.DayProgress >= 1)
                {
                    gameDataManager.AddDay();
                }
            }
            gameManager.CheckIfPauseInput();
        }
    }

    //private void TickDay()
    //{

    //}

    //private void TickWeek()
    //{

    //}

    //private void TickMonth()
    //{

    //}
    //private void TickYear()
    //{

    //}

   

    
}
