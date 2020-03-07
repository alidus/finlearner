using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
	private static GameDataManager instance;
	public static GameDataManager Instance
	{
		get { return instance; }
		set { instance = value; }
	}

	private void OnEnable()
	{
		if (Instance == null)
		{
			Instance = this;
		} else if (Instance == this)
		{
			Destroy(gameObject);
		}
	}

	// Base stats
	private int money;
	public int Money
	{
		get { return money; }
		set { money = value; }
	}

	private int mood;
	public int Mood
	{
		get { return mood; }
		set { mood = value; }
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
