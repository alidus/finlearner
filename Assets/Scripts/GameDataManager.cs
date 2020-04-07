using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
	public static GameDataManager instance;

	// Events, Delegates
	public event Action OnMoneyValueChanged;
	public event Action OnMoodValueChanged;

	public event Action OnDayStarted;
	public event Action OnNewWeekStarted;
	public event Action OnNewMonthStarted;
	public event Action OnNewYearStarted;
	public event Action OnBirthday;
	public event Action OnDayProgressChanged;


	// Sprites
	public Sprite placeHolderSprite;
	public Sprite emptySprite;

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
		SceneManager.sceneLoaded += SceneLoadedHandling;
	}

	private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
	{
		Debug.Log(this.GetType().ToString() + "scene loaded handled");
	}


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
			if (IsRecordingIncome)
			{
				float delta = (value - money);
				DailyIncome += delta;
				WeeklyIncome += delta;
				MonthlyIncome += delta;
				YearlyIncome += delta;
			}
			money = value;
			OnMoneyValueChanged?.Invoke();
		}
	}

	private float mood;
	public float Mood
	{
		get { return mood; }
		set { mood = value; OnMoodValueChanged?.Invoke(); }
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

	private DateTime currentDateTime = DateTime.Now;
	public System.DateTime CurrentDateTime
	{
		get { return currentDateTime; }
		private set { currentDateTime = value; }
	}

	private DateTime birthdayDate = DateTime.Now.AddDays(5);
	private float dayProgress;
	private GameManager gameManager;

	public System.DateTime BirthdayDate
	{
		get { return birthdayDate; }
		set { birthdayDate = value; }
	}

	public float DayProgress { get => dayProgress; set { dayProgress = value; OnDayProgressChanged?.Invoke(); }  }
	/// <summary>
	/// How much hours pass per second
	/// </summary>
	public float HoursPerSecond { get; internal set; } = 6;

	public bool DEBUG { get; set; } = true;
	public void SetValuesToGameModeSpecified(GameMode gameMode)
	{
		if (gameMode != null)
		{
			Money = gameMode.Money;
			Mood = gameMode.Mood;
			Age = gameMode.Age;
			StartRecordingIncomeStatistics();
			gameMode.SetupWinCondition(this);
		}
	}

	void StartRecordingIncomeStatistics()
	{
        IsRecordingIncome = true;
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
