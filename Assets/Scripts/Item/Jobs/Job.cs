using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum JobCategory { IT, Service, Govermant, Art}

[CreateAssetMenu(menuName = "SO/Items/Job", fileName = "Job")]
public class Job : Item, IDrawable, IHaveStatusEffect, IEquipable, IClickable
{ 
    [SerializeField]
    private JobCategory category;
    public JobCategory Category
    {
        get { return category; }
        set { category = value; }
    }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite
    {
        get => sprite; set => sprite = value;
    }

    [SerializeField]
    private List<StatusEffect> statusEffects;
    public System.Collections.Generic.List<StatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }

    [SerializeField]
    private bool canBeEquipped = false;
    [SerializeField]
    private bool isEquipped = false;

    public event Action OnEquip;
    public event Action OnUnEquip;

    public bool CanBeEquipped { get => canBeEquipped; set => canBeEquipped = value; }
    public bool IsEquipped { get => isEquipped; private set => isEquipped = value; }
    public UnityAction OnClick { get; set; }

    private Job()
    {
        SetupClickAction();
    }

    private void SetupClickAction()
    {
        OnClick = delegate
        {
            if (CanBeEquipped)
            {
                if (!IsEquipped)
                {
                    Equip();

                }
                else
                {
                    Uneqip();
                }
            }
        };
    }

    public IncomeData GetIncomeData()
    {
        IncomeData result = new IncomeData();
        foreach (StatusEffect statusEffect in StatusEffects)
        {
            if (statusEffect.Type == StatusEffectType.Money)
            {
                switch (statusEffect.Freqency)
                {
                    case StatusEffectFrequency.Daily:
                        result.DailyIncome += statusEffect.Value;
                        break;
                    case StatusEffectFrequency.Weekly:
                        result.WeeklyIncome += statusEffect.Value;
                        break;
                    case StatusEffectFrequency.Monthly:
                        result.MonthlyIncome += statusEffect.Value;
                        break;
                    case StatusEffectFrequency.Yearly:
                        result.YearlyIncome += statusEffect.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        return result;
    }

    public bool isValid()
    {

        return true;
    }

    public void Equip()
    {
        IsEquipped = true;
        OnEquip?.Invoke();
    }

    public void Uneqip()
    {
        IsEquipped = false;
        OnUnEquip?.Invoke();
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
