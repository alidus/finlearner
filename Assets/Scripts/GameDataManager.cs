using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
	public static GameDataManager instance;

	// Events, Delegates
	public delegate void MoneyValueChangedAction();
	public event MoneyValueChangedAction OnMoneyValueChanged;
	public delegate void MoodValueChangedAction();
	public event MoodValueChangedAction OnMoodValueChanged;

	public delegate void TimeMilestoneReachedAction();
	public event TimeMilestoneReachedAction OnNewDayStarted;
	public event TimeMilestoneReachedAction OnNewWeekStarted;
	public event TimeMilestoneReachedAction OnNewMonthStarted;
	public event TimeMilestoneReachedAction OnNewYearStarted;
	public delegate void BirthdayAction();
	public event BirthdayAction OnBirthday;
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
		LoadGamemodeData(gameManager.GameMode);
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

	private DateTime birthdayDate = DateTime.Now.AddDays(200);
	private float dayProgress;
	private GameManager gameManager;

	public System.DateTime BirthdayDate
	{
		get { return birthdayDate; }
		set { birthdayDate = value; }
	}

	public float DayProgress { get => dayProgress; set { dayProgress = value; OnDayProgressChanged(); }  }
	/// <summary>
	/// How much hours pass per second
	/// </summary>
	public float HoursPerSecond { get; internal set; } = 6;

	public bool DEBUG { get; set; } = true;
	public void LoadGamemodeData(GameMode gameMode)
	{
		Money = gameMode.money;
		Mood = gameMode.mood;
		Age = gameMode.age;
		IsRecordingIncome = true;
	}
	public void AddDay()
	{
		DailyIncome = 0;
		OnNewDayStarted();
		int currentMonth = currentDateTime.Month;
		int currentYear = currentDateTime.Year;
		CurrentDateTime = currentDateTime.AddDays(1);

		if ((int)currentDateTime.DayOfWeek == 0)
		{
			WeeklyIncome = 0;
			OnNewWeekStarted();
		}
		if ((int)currentDateTime.Month != currentMonth)
		{
			MonthlyIncome = 0;
			OnNewMonthStarted();
		}
		if ((int)currentDateTime.Year != currentYear)
		{
			YearlyIncome = 0;
			OnNewYearStarted();
		}

		if (currentDateTime.Date == birthdayDate.Date)
		{
			// Happy birthday
			Age++;
			OnBirthday();
		}
	}
}
