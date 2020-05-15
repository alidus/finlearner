using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class BankService : Item, IHaveStatusEffect, IPurchasable
{

    public event Action OnPurchaseStateChanged;
    public event Action OnPurchasableStateChanged;
    public event PurchasableInstanceHandler OnInstancePurchaseStateChanged;
    public event PurchasableInstanceHandler OnInstancePurchasableStateChanged;
    public List<StatusEffect> StatusEffects { get; set; } = new List<StatusEffect>();

    [SerializeField]
    float rate = 0.20f;
    public float Rate { get => rate; set => rate = value; }
    [SerializeField]
    float mixAmount = 1000f;
    public float MixAmount
    {
        get { return mixAmount; }
        set { mixAmount = value; }
    }
    [SerializeField]
    float maxAmount = 10000f;
    public float MaxAmount
    {
        get { return maxAmount; }
        set { maxAmount = value; }
    }

    [SerializeField]
    int totalPeriodInMonths = 12;
    public int TotalPeriodInMonths
    {
        get { return totalPeriodInMonths; }
        set { totalPeriodInMonths = value; }
    }

    public float Price { get => price; set => price = value; }
    private float price = 0f;

    [SerializeField]
    float amount;

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

    [SerializeField]
    private bool canBePurchased;
    [SerializeField]
    private bool isPurchased;

    public bool IsPurchased { get => isPurchased; set => isPurchased = value; }
    public float Amount { get => amount; set => amount = value; }

    protected List<StatusEffect> statusEffects = new List<StatusEffect>();

    public virtual void Purchase()
    {
        // TODO: implement conditional expression to check if credit is acceptable for client
        GenerateStatusEffects();
        if (true)
        {
            IsPurchased = true;
            StatusEffectsManager.instance.ApplyStatusEffects(StatusEffects);
            OnPurchaseStateChanged?.Invoke();
        }
        else
        {
            HintsManager.instance.ShowHint(HintsManager.instance.HintPresets[HintPreset.NotEnoughMoney]);
        }
    }

    public virtual void Sell()
    {
        IsPurchased = false;
        StatusEffectsManager.instance.ApplyStatusEffects(StatusEffects);
        OnPurchaseStateChanged?.Invoke();
    }

    protected virtual void GenerateStatusEffects()
    {
        StatusEffects.Add(new StatusEffect("Использована банковская услуга", 1, StatusEffectType.Money, StatusEffectFrequency.OneShot, StatusEffectFlags.Loan));
    }
    public void NotifyOnInstancePurchaseStateChanged(IPurchasable purchasable)
    {
        OnInstancePurchaseStateChanged.Invoke(purchasable);
    }

    public void NotifyOnInstancePurchasableStateChanged(IPurchasable purchasable)
    {
        OnInstancePurchasableStateChanged.Invoke(purchasable);
    }


}
