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

    public virtual void Purchase()
    {
        // TODO: implement conditional expression to check if credit is acceptable for client
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

    public void NotifyOnInstancePurchaseStateChanged(IPurchasable purchasable)
    {
        OnInstancePurchaseStateChanged.Invoke(purchasable);
    }

    public void NotifyOnInstancePurchasableStateChanged(IPurchasable purchasable)
    {
        OnInstancePurchasableStateChanged.Invoke(purchasable);
    }


}
