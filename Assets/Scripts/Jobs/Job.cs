using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
    public string title;
    public string desc;
    public IncomeModifier incomeModifier;
    public MoodModifier moodModifier;
    public Job(JobSOTemplate template)
    {
        title = template.title;
        desc = template.title;
        incomeModifier = template.incomeModifier;
        moodModifier = template.moodModifier;
    }
}
