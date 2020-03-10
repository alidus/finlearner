using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
	public static GameDataManager instance;

	// Events, Delegates
	public delegate void MoneyValueChangedAction();
	public event MoneyValueChangedAction OnMoneyValueChanged;
    public delegate void MoodValueChangedAction();
    public event MoodValueChangedAction OnMoodValueChanged;

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

    // Base stats
    private float money;
	public float Money
	{
		get { return money; }
		set { money = value; OnMoneyValueChanged?.Invoke() ; }
	}

	private float mood;
	public float Mood
	{
		get { return mood; }
		set { mood = value; OnMoodValueChanged?.Invoke(); }
	}

    public int age;
    public int Age
    {
        get { return age; }
        set { age = value; }
    }
    // Income
    private int dailyIncome;
	public int DailyIncome
	{
		get { return dailyIncome; }
		set { dailyIncome = value; }
	}

	private int weeklyIncome;
	public int WeeklyIncome
	{
		get { return weeklyIncome; }
		set { weeklyIncome = value; }
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
	// Time
	private int dayOfWeekIndex;
	public int DayOfWeekIndex
	{
		get { return dayOfWeekIndex; }
		set { dayOfWeekIndex = value; }
	}

	private int dayCount;
	public int DayCount
	{
		get { return dayCount; }
		set { dayCount = value; }
	}

	public void AddToDayCounter()
	{
		DayCount++;
	}

	public void AddDayOfWeek()
	{
		if (DayOfWeekIndex > 5)
		{
			DayOfWeekIndex = 0;
		} else
		{
			DayOfWeekIndex++;
		}
	}
}
