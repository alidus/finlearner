using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections.Specialized;

public enum EducationDirection { Technical, Law}
public class EducationEntity : Item, IDrawable, IPurchasable, IHaveStatusEffect, IHaveCertificate, IDemandCertificate, ITimeConsumer
{
    public Sprite Sprite { get; set; }
    [SerializeField]
    private float price = 1000f;
    public bool CanBePurchased
    {
        get => canBePurchased; set
        {
            if (value != canBePurchased)
            {
                canBePurchased = value;
                OnPurchasableStateChanged?.Invoke();
            }
        }
    }
    public bool IsPurchased { get => isPurchased; set => isPurchased = value; }
    public float Price { get => price; set => price = value; }
    [SerializeField]
    private float durationInDays = 7f;

    public bool IsComplete { get; set; }

    public List<Certificate> Certificates { get => certificates; set => certificates = value; }
    public List<StatusEffect> StatusEffects { get; set; } = new List<StatusEffect>();

    public float DurationInDays { get => durationInDays; set => durationInDays = value; }
    private float currentProgress;

    public event Action OnProgressChanged;
    public event Action OnComplete;
    public float CurrentProgress { get => currentProgress; set { currentProgress = value; OnProgressChanged?.Invoke(); } }

    public List<Certificate> MandatoryCertificates { get => mandatoryCertificates; set => mandatoryCertificates = value; }
    public TimeConsumerCategory TimeConsumerCategory { get; set; } = TimeConsumerCategory.Education;
    [SerializeField]
    private float hoursOfWeekToConsume = 5 * 2;
    public float HoursOfWeekToConsume { get => hoursOfWeekToConsume; set => hoursOfWeekToConsume = value; }

    StatusEffect purchaseStatusEffect;
    StatusEffect sellStatusEffect;


    public EducationDirection EducationDirection;

    [SerializeField]
    private bool canBePurchased;
    [SerializeField]
    private bool isPurchased;
    [SerializeField]
    private List<Certificate> certificates = new List<Certificate>();

    public event Action OnPurchaseStateChanged;
    public event Action OnPurchasableStateChanged;
    public event PurchasableInstanceHandler OnInstancePurchaseStateChanged;
    public event PurchasableInstanceHandler OnInstancePurchasableStateChanged;

    EducationHub educationHub;

    [SerializeField]
    private List<Certificate> mandatoryCertificates;


    private void Awake()
    {
        purchaseStatusEffect = new StatusEffect("Покупка " + Title, -Price, StatusEffectType.Money, StatusEffectFrequency.OneShot);
        sellStatusEffect = new StatusEffect("Продажа " + Title, Price / 2, StatusEffectType.Money, StatusEffectFrequency.OneShot);
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
        CanBePurchased = Certificate.SertificateCheck(educationHub.Certificates, this);
    }

    public void Purchase()
    {
        if (GameDataManager.instance.IsEnoughMoney(Price))
        {
            if (GameDataManager.instance.CheckIfHasFreeTimeFor(this))
            {
                IsPurchased = true;
                StatusEffectsManager.instance.ApplyStatusEffects(purchaseStatusEffect);
                StatusEffectsManager.instance.ApplyStatusEffects(StatusEffects);
                GameDataManager.instance.AddTimeConsumers(this);
                OnPurchaseStateChanged?.Invoke();
            } else
            {
                HintsManager.instance.ShowHint(HintsManager.instance.HintPresets[HintPreset.NoFreeTime]);
            }
        }
        else
        {
            HintsManager.instance.ShowHint(HintsManager.instance.HintPresets[HintPreset.NotEnoughMoney]);
        }
    }

    public void Sell()
    {
        IsPurchased = false;
        CurrentProgress = 0;
        StatusEffectsManager.instance.ApplyStatusEffects(sellStatusEffect);
        StatusEffectsManager.instance.ApplyStatusEffects(StatusEffects);
        GameDataManager.instance.RemoveTimeConsumers(this);
        OnPurchaseStateChanged?.Invoke();
    }

    public void NotifyOnInstancePurchaseStateChanged(IPurchasable purchasable)
    {
        OnInstancePurchaseStateChanged.Invoke(purchasable);
    }

    public void NotifyOnInstancePurchasableStateChanged(IPurchasable purchasable)
    {
        OnInstancePurchasableStateChanged.Invoke(purchasable);
    }

    internal void Complete()
    {
        HintsManager.instance.ShowHint("Поздравляем, вы закончили курс " + Title, "Вы закончили курс " + Title + "и теперь вам доступны новые возможности.");
        // TODO: give certificates
        IsComplete = true;
        GameDataManager.instance.RemoveTimeConsumers(this);

        foreach (Certificate certificate in Certificates)
        {
            certificate.Equip();
            educationHub.Certificates.Add(certificate);
        }

        OnComplete?.Invoke();
    }

    /// <summary>
    /// Clear attached certificates list and initiate with given certificates
    /// </summary>
    /// <param name="certificates"></param>
    public void InitCertificates(List<Certificate> certificates)
    {
        Certificates.Clear();
        foreach (Certificate certificate in certificates)
        {
            Certificates.Add(certificate);
        }
    }

    /// <summary>
    /// Clear attached certificates list and initiate with given certificate
    /// </summary>
    /// <param name="certificate"></param>
    public void InitCertificates(Certificate certificate)
    {
        Certificates.Clear();
        Certificates.Add(certificate);
    }

    /// <summary>
    /// Clear attached certificates list and initiate with new certificate generated based on entity's params
    /// </summary>
    public void InitCertificates()
    {
        Certificates.Clear();
        var certificate = ScriptableObject.CreateInstance<Certificate>();
        certificate.Title = Title;
        Certificates.Add(certificate);
    }

    public override string ToString()
    {
        var result = "";
        result += String.Format("Title: {0}, Type: {1}, Direction: {2}, Price: {3}, IsComplete: {4}, IsInProgress: {5}, IsAvailable: {6}",
            Title, this.GetType().ToString(), EducationDirection.ToString(), Price, IsComplete, IsPurchased, CanBePurchased);
        return result;
    }
}
