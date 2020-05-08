using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalaryDisplayView : View
{
    float dailySalary, weeklySalary, monthlySalary, yearlySalary;

    public float DailySalary { get => dailySalary; set => dailySalary = value; }
    public float WeeklySalary { get => weeklySalary; set => weeklySalary = value; }
    public float MonthlySalary { get => monthlySalary; set => monthlySalary = value; }
    public float YearlySalary { get => yearlySalary; set => yearlySalary = value; }

    Text dailySalaryValueTextComponent, weeklySalaryValueTextComponent, monthlySalaryValueTextComponent, yearlySalaryValueTextComponent;
    Text dailySalaryLabelTextComponent, weeklySalaryLabelTextComponent, monthlySalaryLabelTextComponent, yearlySalaryLabelTextComponent;

    Image dailySalaryBackground, weeklySalaryBackground, monthlySalaryBackground, yearlySalaryBackground;


    private void OnEnable()
    {
        var upperValues = transform.Find("UpperValues");
        dailySalaryValueTextComponent = upperValues.Find("DailySalary").Find("Value").GetComponent<Text>();
        dailySalaryLabelTextComponent = upperValues.Find("DailySalary").Find("Label").GetComponent<Text>();

        weeklySalaryValueTextComponent = upperValues.Find("WeeklySalary").Find("Value").GetComponent<Text>();
        weeklySalaryLabelTextComponent = upperValues.Find("WeeklySalary").Find("Label").GetComponent<Text>();

        dailySalaryBackground = upperValues.Find("DailySalary").GetComponent<Image>();
        weeklySalaryBackground = upperValues.Find("WeeklySalary").GetComponent<Image>();


        var lowerValues = transform.Find("LowerValues");
        monthlySalaryValueTextComponent = lowerValues.Find("MonthlySalary").Find("Value").GetComponent<Text>();
        monthlySalaryLabelTextComponent = lowerValues.Find("MonthlySalary").Find("Label").GetComponent<Text>();

        yearlySalaryValueTextComponent = lowerValues.Find("YearlySalary").Find("Value").GetComponent<Text>();
        yearlySalaryLabelTextComponent = lowerValues.Find("YearlySalary").Find("Label").GetComponent<Text>();

        monthlySalaryBackground = lowerValues.Find("MonthlySalary").GetComponent<Image>();
        yearlySalaryBackground = lowerValues.Find("YearlySalary").GetComponent<Image>();
    }

    private void Start()
    {
        UpdateColorScheme();
    }

    void UpdateColorScheme()
    {
        if (dailySalary > 0)
        {
            dailySalaryLabelTextComponent.color = GameDataManager.instance.DailyFrequencyColor;
            dailySalaryValueTextComponent.color = Color.white;
        } else
        {
            dailySalaryLabelTextComponent.color = GameDataManager.instance.DefaultSalaryDisplayLabelColor;
            dailySalaryValueTextComponent.color = GameDataManager.instance.DefaultSalaryDisplayLabelColor;
        }

        if (weeklySalary > 0)
        {
            weeklySalaryLabelTextComponent.color = GameDataManager.instance.WeeklyFrequencyColor;
            weeklySalaryValueTextComponent.color = Color.white;

        }
        else
        {
            weeklySalaryLabelTextComponent.color = GameDataManager.instance.DefaultSalaryDisplayLabelColor;
            weeklySalaryValueTextComponent.color = GameDataManager.instance.DefaultSalaryDisplayLabelColor;

        }

        if (monthlySalary > 0)
        {
            monthlySalaryLabelTextComponent.color = GameDataManager.instance.MonthlyFrequencyColor;
            monthlySalaryValueTextComponent.color = Color.white;

        }
        else
        {
            monthlySalaryLabelTextComponent.color = GameDataManager.instance.DefaultSalaryDisplayLabelColor;
            monthlySalaryValueTextComponent.color = GameDataManager.instance.DefaultSalaryDisplayLabelColor;

        }

        if (yearlySalary > 0)
        {
            yearlySalaryLabelTextComponent.color = GameDataManager.instance.YearlyFrequencyColor;
            yearlySalaryValueTextComponent.color = Color.white;

        }
        else
        {
            yearlySalaryLabelTextComponent.color = GameDataManager.instance.DefaultSalaryDisplayLabelColor;
            yearlySalaryValueTextComponent.color = GameDataManager.instance.DefaultSalaryDisplayLabelColor;

        }
    }

    public void UpdateValues(Job job)
    {
        DailySalary = job.StatusEffects.Find((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == StatusEffectFrequency.Daily)?.Value ?? 0;
        WeeklySalary = job.StatusEffects.Find((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == StatusEffectFrequency.Weekly)?.Value ?? 0;
        MonthlySalary = job.StatusEffects.Find((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == StatusEffectFrequency.Monthly)?.Value ?? 0;
        YearlySalary = job.StatusEffects.Find((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == StatusEffectFrequency.Yearly)?.Value ?? 0;
    }

    public override void UpdateView()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        dailySalaryValueTextComponent.text = DailySalary.ToString();
        weeklySalaryValueTextComponent.text = WeeklySalary.ToString();
        monthlySalaryValueTextComponent.text = MonthlySalary.ToString();
        yearlySalaryValueTextComponent.text = YearlySalary.ToString();
    }
}
