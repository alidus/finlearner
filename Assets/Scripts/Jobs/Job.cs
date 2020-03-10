using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
    [SerializeField]
    private string title;
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    [SerializeField]
    private string desc;
    public string Desc
    {
        get { return desc; }
        set { desc = value; }
    }
    [SerializeField]
    private List<StatusEffect> statusEffects;
    public System.Collections.Generic.List<StatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }
    public Job(JobSOTemplate template)
    {
        Title = template.title;
        Desc = template.title;
        StatusEffects = template.modifiers;
    }
}
