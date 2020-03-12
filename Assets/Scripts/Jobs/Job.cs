using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobCategory { IT, Service, Govermant}

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
    public string Description
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
    private JobCategory category;
    public JobCategory Category
    {
        get { return category; }
        set { category = value; }
    }

    public Sprite Sprite
    {
        get; set;
    }

    public Job(JobSOTemplate template)
    {
        Title = template.title;
        Description = template.description;
        StatusEffects = template.modifiers;
    }
    private Job()
    {

    }

    public bool isValid()
    {
        
        return true;
    }

    public class JobBuilder
    {
        private Job job;
        public JobBuilder()
        {
            job = new Job();
        }
        public JobBuilder SetTitle(string title)
        {
            job.Title = title;
            return this;
        }
        public JobBuilder SetDescription(string description)
        {
            job.Description = description;
            return this;
        }
        public JobBuilder SetCategory(JobCategory category)
        {
            job.Category = category;
            return this;
        }
        public JobBuilder SetStatusEffects(List<StatusEffect> statusEffects)
        {
            job.StatusEffects = statusEffects;
            return this;
        }
        public Job Build()
        {
            if (job.isValid())
            {
                return job;
            } else
            {
                return null;
            }
        }
    }
}
