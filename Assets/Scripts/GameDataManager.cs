using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
	public static GameDataManager instance;
	StatusEffectsManager statusEffectsManager;

	// Events, Delegates
	public delegate void ValueChangedAction(float deltaValue);
	public event ValueChangedAction OnMoneyValueChanged;
	public event ValueChangedAction OnMoodValueChanged;

	public event Action OnDayStarted;
	public event Action OnNewWeekStarted;
	public event Action OnNewMonthStarted;
	public event Action OnNewYearStarted;
	public event Action OnBirthday;
	public event Action OnDayProgressChanged;


	// Sprites
	public Sprite placeHolderSprite;
	public Sprite emptySprite;

    [Header("Frequency colors")]
    public Color DailyFrequencyColor = new Color(0.07f, 1f, 0.07f, 0.6f);
	public Color WeeklyFrequencyColor = new Color(0.95f, 0.68f, 1f, 0.6f);
	public Color MonthlyFrequencyColor = new Color(0.68f, 0.87f, 1f, 0.6f);
	public Color YearlyFrequencyColor = new Color(1f, 0.68f, 0.72f, 0.6f);

    [Header("Buttons colors")]
    public Color ButtonDefaultColor = new Color(0.14f, 0.14f, 0.14f);
	public Color ButtonSelectedColor = new Color(0.44f, 0.08f, 0.14f);

	[Header("Education-related colors")]
	public Color CourseEducationEnityTypeColor = new Color(1f, 0, 0, 0.4f);
	public Color DegreeEducationEnityTypeColor = new Color(1f, 0, 0.7f, 0.4f);
	public Color TechnicalEducationDirectionTypeColor = new Color(0.12f, 0.13f, 0.68f, 0.7f);
	public Color LawEducationDirectionTypeColor = new Color(0.84f, 0.62f, 0, 0.7f);
	[Header("Salary display")]
	public Color DefaultSalaryDisplayLabelColor = Color.white;



	private void Awake()
	{

		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		

		Init();
	}

	private void Init()
	{
		gameManager = GameManager.instance;
		statusEffectsManager = StatusEffectsManager.instance;

        InitSleepTimeConsumer();
		SceneManager.sceneLoaded += SceneLoadedHandling;
	}

	private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
	{
		Debug.Log(this.GetType().ToString() + "scene loaded handled");
	}

	public bool IsEnoughMoney(float value)
	{
		return Money >= value;
	}

	public const int TOTAL_FREE_HOURS_IN_A_WEEK = 7 * 24;

	public float FreeHoursOfWeekLeft { get => freeHoursOfWeekLeft; set { freeHoursOfWeekLeft = value; CalculateFreeTimeAmountEffects(); } }
	private bool isRecordingIncome;
	public bool IsRecordingIncome
	{
		get { return isRecordingIncome; }
		set { isRecordingIncome = value; }
	}
	// Base stats
	private float money;
	public float Money
	{
		get { return money; }
		set
		{
			if (value != money)
			{
				float delta = (value - money);
				if (IsRecordingIncome)
                {
                    DailyIncome += delta;
                    WeeklyIncome += delta;
                    MonthlyIncome += delta;
                    YearlyIncome += delta;
                }
                money = value;
                OnMoneyValueChanged?.Invoke(delta);
            }
		}
	}

	private float mood;
	public float Mood
	{
		get { return mood; }
		set {
			if (value != mood)
			{
				float delta = (value - money);
				mood = value;
				OnMoodValueChanged?.Invoke(delta);
            }
        }
	}

	private int age = 18;
	public int Age
	{
		get { return age; }
		set { age = value; }
	}
	// Income
	private float dailyIncome;
	public float DailyIncome
	{
		get { return dailyIncome; }
		set { dailyIncome = value; }
	}

	private float weeklyIncome;
	public float WeeklyIncome
	{
		get { return weeklyIncome; }
		set { weeklyIncome = value; }
	}

	private float monthlyIncome;
	public float MonthlyIncome
	{
		get { return monthlyIncome; }
		set { monthlyIncome = value; }
	}
	private float yearlyIncome;
	public float YearlyIncome
	{
		get { return yearlyIncome; }
		set { yearlyIncome = value; }
	}
	// Mood
	private int dailyMoodChange;
	public int DailyMoodChange
	{
		get { return dailyMoodChange; }
		set { dailyMoodChange = value; }
	}
	private int weeklyMoodChange;
	public int WeeklyMoodChange
	{
		get { return weeklyMoodChange; }
		set { weeklyMoodChange = value; }
	}

	public float GetDeltaHours()
	{
		return Time.deltaTime * HoursPerSecond;
	}

	public float GetDeltaDay()
	{
		return Time.deltaTime * (HoursPerSecond / 24);
	}

	private DateTime currentDateTime = DateTime.Now;
	public System.DateTime CurrentDateTime
	{
		get { return currentDateTime; }
		private set { currentDateTime = value; }
	}

	private DateTime birthdayDate = DateTime.Now.AddDays(5);
	private float dayProgress;
	private GameManager gameManager;
	private float freeHoursOfWeekLeft = TOTAL_FREE_HOURS_IN_A_WEEK;

	public System.DateTime BirthdayDate
	{
		get { return birthdayDate; }
		set { birthdayDate = value; }
	}

	public float DayProgress { get => dayProgress; set { dayProgress = value; OnDayProgressChanged?.Invoke(); } }
	/// <summary>
	/// How much hours pass per second
	/// </summary>
	public float HoursPerSecond { get; internal set; } = 6;

	public ObservableCollection<ITimeConsumer> TimeConsumers { get; set; } = new ObservableCollection<ITimeConsumer>();

	public bool DEBUG { get; set; } = true;

	StatusEffect moderateExhaustion = new StatusEffect("Небольшая усталость", -2, StatusEffectType.Mood, StatusEffectFrequency.Weekly, StatusEffectFlags.Exhaustion);
    StatusEffect heavyExhaustion = new StatusEffect("Сильная усталость", -5, StatusEffectType.Mood, StatusEffectFrequency.Weekly, StatusEffectFlags.Exhaustion);
    StatusEffect tremendousExhaustion = new StatusEffect("Огромная усталость", -10, StatusEffectType.Mood, StatusEffectFrequency.Weekly, StatusEffectFlags.Exhaustion);


    public void UpdateValuesToNewGameMode(GameMode gameMode)
	{
		if (gameMode != null)
		{
			StopAndResetIncomeStatistics();
			Money = gameMode.Money;
			Mood = gameMode.Mood;
			Age = gameMode.Age;
			StartRecordingIncomeStatistics();
		}
	}

	public void AddTimeConsumers(ITimeConsumer timeConsumer)
	{
		TimeConsumers.Add(timeConsumer);
		UpdateFreeHoursOfWeekLeft();
	}
	

	/// <summary>
	/// Calculate free time of week amount and apply mood status effects respectively
	/// </summary>
	public void CalculateFreeTimeAmountEffects()
	{
		float freeTimeOfWeekPercentage = FreeHoursOfWeekLeft / TOTAL_FREE_HOURS_IN_A_WEEK;
		StatusEffect sutableStatusEffect = null;
		if (freeTimeOfWeekPercentage <= 0.1)
		{
			// Moderate exhaustion
			sutableStatusEffect = tremendousExhaustion;
		} else if (freeTimeOfWeekPercentage <= 0.2)
		{
            sutableStatusEffect = heavyExhaustion;
        } else if (freeTimeOfWeekPercentage <= 0.3)
		{
            sutableStatusEffect = moderateExhaustion;
        }
		
		if (sutableStatusEffect == null)
		{
			statusEffectsManager.RemoveStatusEffects(StatusEffectFlags.Exhaustion);
        } else
		{
            if (!statusEffectsManager.StatusEffects.Contains(sutableStatusEffect))
			{
                statusEffectsManager.RemoveStatusEffects(StatusEffectFlags.Exhaustion);
                statusEffectsManager.ApplyStatusEffects(sutableStatusEffect);
            }
        }
    }

	public void AddTimeConsumers(List<ITimeConsumer> timeConsumers)
	{
		foreach (ITimeConsumer timeConsumer in timeConsumers)
		{
			AddTimeConsumers(timeConsumer);
		}
		UpdateFreeHoursOfWeekLeft();
	}

	public void RemoveTimeConsumers(ITimeConsumer timeConsumer)
	{
		TimeConsumers.Remove(timeConsumer);
		UpdateFreeHoursOfWeekLeft();
	}

	public void RemoveTimeConsumers(List<ITimeConsumer> timeConsumers)
	{
		foreach (TimeConsumer timeConsumer in timeConsumers)
		{
			TimeConsumers.Remove(timeConsumer);
		}
		UpdateFreeHoursOfWeekLeft();
	}

	void InitSleepTimeConsumer()
	{
		var sleepTimeConsumer = ScriptableObject.CreateInstance<TimeConsumer>();
		sleepTimeConsumer.Title = "Sleep";
		sleepTimeConsumer.HoursOfWeekToConsume = 7 * 8;
		AddTimeConsumers(sleepTimeConsumer);
	}

	public void UpdateFreeHoursOfWeekLeft()
	{
		float result = TOTAL_FREE_HOURS_IN_A_WEEK;
		foreach (ITimeConsumer timeConsumer in TimeConsumers)
		{
			result -= timeConsumer.HoursOfWeekToConsume;
		}
		FreeHoursOfWeekLeft = result;
	}


	void StartRecordingIncomeStatistics()
	{
		IsRecordingIncome = true;
	}

    void StopAndResetIncomeStatistics()
    {
        IsRecordingIncome = false;
		DailyIncome = 0;
        WeeklyIncome = 0;
        MonthlyIncome = 0;
        YearlyIncome = 0;
    }

    public Color GetColorForSEFrequency(StatusEffectFrequency statusEffectFrequency)
	{
		switch (statusEffectFrequency)
		{
			case StatusEffectFrequency.OneShot:
				return Color.red;
			case StatusEffectFrequency.Daily:
				return DailyFrequencyColor;
			case StatusEffectFrequency.Weekly:
				return WeeklyFrequencyColor;
			case StatusEffectFrequency.Monthly:
				return MonthlyFrequencyColor;
			case StatusEffectFrequency.Yearly:
				return YearlyFrequencyColor;
			default:
				return Color.white;
		}
	}

	public void AddDay()
	{
		DailyIncome = 0;
		int currentMonth = currentDateTime.Month;
		int currentYear = currentDateTime.Year;
		CurrentDateTime = currentDateTime.AddDays(1);

		if ((int)currentDateTime.DayOfWeek == 0)
		{
			WeeklyIncome = 0;
			OnNewWeekStarted?.Invoke();
		}
		if ((int)currentDateTime.Month != currentMonth)
		{
			MonthlyIncome = 0;
			OnNewMonthStarted?.Invoke();
		}
		if ((int)currentDateTime.Year != currentYear)
		{
			YearlyIncome = 0;
			OnNewYearStarted?.Invoke();
		}

		if (currentDateTime.Date == birthdayDate.Date)
		{
			// Happy birthday
			Age++;
			OnBirthday?.Invoke();
		}
		DayProgress -= 1;
		OnDayStarted();
	}
}
