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

    Text dailySalaryTextComponent, weeklySalaryTextComponent, monthlySalaryTextComponent, yearlySalaryTextComponent;

    private void OnEnable()
    {
        var upperValues = transform.Find("UpperValues");
        dailySalaryTextComponent = upperValues.Find("DailySalary").Find("Value").GetComponent<Text>();
        weeklySalaryTextComponent = upperValues.Find("WeeklySalary").Find("Value").GetComponent<Text>();
        var lowerValues = transform.Find("LowerValues");
        monthlySalaryTextComponent = lowerValues.Find("MonthlySalary").Find("Value").GetComponent<Text>();
        yearlySalaryTextComponent = lowerValues.Find("YearlySalary").Find("Value").GetComponent<Text>();
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
        dailySalaryTextComponent.text = DailySalary.ToString();
        weeklySalaryTextComponent.text = WeeklySalary.ToString();
        monthlySalaryTextComponent.text = MonthlySalary.ToString();
        yearlySalaryTextComponent.text = YearlySalary.ToString();
    }
}
