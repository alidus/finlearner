using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Controls base gameplay logic
/// </summary>
public class FreeplayController : MonoBehaviour
{
    // Managers, Controllers
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private UIManager uiManager;
    private InventoryManager storeManager;
    private HouseManager houseManager;
    private StatusEffectsController statusEffectsManager;
    private HintsManager hintsManager;
    private LaborExchangeManager laborExchangeManager;

    // Fin ops
    private List<Loan> activeCredits = new List<Loan>();

    // Tutorial
    private bool showTutorial = true;
    private bool isPlayerController;

    public bool IsPlayerController { 
        get => isPlayerController; 
        set { isPlayerController = value; if (isPlayerController) { hintsManager.ShowHint("Добро пожаловать в свободный режим", "Активирован свободный режим..."); } } }

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

    public void Init()
    {
        storeManager = InventoryManager.instance;
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        uiManager = UIManager.instance;
        houseManager = HouseManager.instance;
        statusEffectsManager = StatusEffectsController.instance;
        hintsManager = HintsManager.instance;
        laborExchangeManager = LaborExchangeManager.instance;


        //gameDataManager.OnNewDayStarted += TickDay;
        //gameDataManager.OnNewWeekStarted += TickWeek;
        //gameDataManager.OnNewMonthStarted += TickMonth;
        //gameDataManager.OnNewYearStarted += TickYear;
        gameDataManager.OnBirthday += delegate { hintsManager.ShowHint("С днем рождения!", "Сегодня у вас день рождения, поздавляем вас и желаем счастья и денег :)"); };
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerController)
        {
            if (gameManager.GameState == GameState.InGame)
            {
                // TODO: implement as event from dataManager
                gameDataManager.DayProgress += Time.deltaTime * (gameDataManager.HoursPerSecond / 24);

                if (gameDataManager.DayProgress >= 1)
                {
                    gameDataManager.AddDay();
                    gameDataManager.DayProgress -= 1;

                    // TODO: implement as event from dataManager
                    uiManager.UpdateInfoPanel();
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
