using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO/Items/Job", fileName = "Job")]
public class Job : Item, IDrawable, IHaveStatusEffect, IEquipable, IDemandCertificate, ITimeConsumer
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
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    public System.Collections.Generic.List<StatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }

    [SerializeField]
    private bool canBeEquipped = false;
    [SerializeField]
    private bool isEquipped = false;

    public event Action OnEquipStateChanged;
    public event Action OnEquippableStateChanged;
    public event EquippableInstanceHandler OnInstanceEquipStateChanged;
    public event EquippableInstanceHandler OnInstanceEquippableStateChanged;

    public bool CanBeEquipped { get => canBeEquipped; set
        {
            if (value != canBeEquipped)
            {
                canBeEquipped = value;
                OnEquippableStateChanged?.Invoke();
            }
        } }
    public bool IsEquipped { get => isEquipped; set
        {
            if (value != isEquipped)
            {
                isEquipped = value;
                OnEquipStateChanged?.Invoke();
            }
        }
    }
    public List<Certificate> MandatoryCertificates { get => mandatoryCertificates; set => mandatoryCertificates = value; }
    [SerializeField]
    private float hoursOfWeekToConsume = 5 * 8;
    [SerializeField]
    private List<Certificate> mandatoryCertificates = new List<Certificate>();
    private EducationHub educationHub;

    public float HoursOfWeekToConsume { get => hoursOfWeekToConsume; set => hoursOfWeekToConsume = value; }
    public TimeConsumerCategory TimeConsumerCategory { get; set; } = TimeConsumerCategory.Job;

    public IncomeData GetIncomeData()
    {
        IncomeData result = new IncomeData();
        foreach (StatusEffect statusEffect in StatusEffects)
        {
            if (statusEffect.Type == StatusEffectType.Money)
            {
                switch (statusEffect.Frequency)
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
        if (GameDataManager.instance.CheckIfHasFreeTimeFor(this))
        {
            IsEquipped = true;
            OnEquipStateChanged?.Invoke();
        }
        else
        {
            HintsManager.instance.ShowHint(HintsManager.instance.HintPresets[HintPreset.NoFreeTime]);
        }

    }

    public void Uneqip()
    {
        IsEquipped = false;
        OnEquipStateChanged?.Invoke();
    }

    public override string ToString()
    {
        string result = "Title: " + Title + "\n";
        result += "Category: " + Category?.ToString() ?? "missing category (unacceptable!)" + "\n";
        result += "Sprite: " + Sprite?.ToString() ?? "missing sprite" + "\n";
        result += "Is Available: " + CanBeEquipped.ToString() + "\n";
        result += "Is Equipped: " + IsEquipped.ToString() + "\n";
        result += "Status effects: " + "\n";
        foreach (StatusEffect statusEffect in StatusEffects)
        {
            result += "   " + statusEffect.ToString();
        }


        return result;
    }

    public void Init()
    {
        educationHub = EducationHub.instance as EducationHub;
        educationHub.Certificates.CollectionChanged -= UpdateAvailability;
        educationHub.Certificates.CollectionChanged += UpdateAvailability;
        UpdateAvailability(null, null);
    }

    private void UpdateAvailability(object sender, NotifyCollectionChangedEventArgs e)
    {
        CanBeEquipped = Certificate.SertificateCheck(educationHub.Certificates, this);
    }

    public void NotifyOnInstanceEquipStateChanged(IEquipable equipable)
    {
        OnInstanceEquipStateChanged?.Invoke(equipable);
    }

    public void NotifyOnInstanceEquippableStateChanged(IEquipable equipable)
    {
        OnInstanceEquippableStateChanged?.Invoke(equipable);
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
            }
            else
            {
                return null;
            }
        }
    }
}
