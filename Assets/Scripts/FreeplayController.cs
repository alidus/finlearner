using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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


    // Tutorial
    private bool showTutorial = true;

    private void Awake()
    {
        if (GameDataManager.instance.DEBUG)
            Debug.Log("FreeplayController awake");


        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
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
        //gameDataManager.OnNewDayStarted += TickDay;
        //gameDataManager.OnNewWeekStarted += TickWeek;
        //gameDataManager.OnNewMonthStarted += TickMonth;
        //gameDataManager.OnNewYearStarted += TickYear;
        gameDataManager.OnBirthday += delegate {
            hintsManager.ShowHint("С днем рождения!", "Сегодня у вас день рождения, поздавляем вас и желаем счастья и денег :)"); 
        };
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
