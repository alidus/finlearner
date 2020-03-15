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
    private ItemManager storeManager;
    private HouseManager houseManager;
    private StatusEffectsController statusEffectsManager;
    private HintsManager hintsManager;
    private LaborExchangeManager laborExchangeManager;


    float timeSinceDayStart;


    // Fin ops
    private List<Loan> activeCredits = new List<Loan>();


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
    }

    private void Init()
    {
        storeManager = ItemManager.instance;
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        uiManager = UIManager.instance;
        houseManager = HouseManager.instance;
        statusEffectsManager = StatusEffectsController.instance;
        hintsManager = HintsManager.instance;
        laborExchangeManager = LaborExchangeManager.instance;

        gameDataManager.Money = gameManager.GameMode.money;
        gameDataManager.Mood = gameManager.GameMode.mood;
        gameDataManager.Age = gameManager.GameMode.age;
        gameDataManager.IsRecordingIncome = true;

        gameManager.OnGameStarted += OnGameStarted;
        //gameDataManager.OnNewDayStarted += TickDay;
        //gameDataManager.OnNewWeekStarted += TickWeek;
        //gameDataManager.OnNewMonthStarted += TickMonth;
        //gameDataManager.OnNewYearStarted += TickYear;
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

        if (Input.GetKey(KeyCode.Escape))
        {
            gameManager.Pause();
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

    private void OnGameStarted(GameMode gameMode)
    {
        if (showTutorial)
        {
            hintsManager.ShowHint("Свободная игра", "В этом режиме игры вам предстоит прожить жизнь по собственному желанию", new HoveringMessageHintPresenter(true, true));
        }

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
