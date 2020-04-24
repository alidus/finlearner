using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StoreItem : Item, IClickable, IPurchasable, IEquipable, IDrawable, IHaveStatusEffect
{
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private float price = 200;
    [SerializeField]
    private bool canBePurchased = true;
    [SerializeField]
    private bool canBeEquipped = false;
    [SerializeField]
    private bool isEquipped = false;
    [SerializeField]
    private bool isPurchased = false;
    [SerializeField]
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    public UnityAction OnClick { get; set; }
    public bool CanBeEquipped { get => canBeEquipped; set => canBeEquipped = value; }
    public bool IsEquipped { get => isEquipped; private set => isEquipped = value; }
    public bool CanBePurchased { get => canBePurchased; set => canBePurchased = value; }
    public bool IsPurchased { get => isPurchased; private set => isPurchased = value; }
    public float Price { get => price; set => price = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public List<StatusEffect> StatusEffects { get => statusEffects; set => statusEffects = value; }
    public event Action OnEquipStateChanged;
    public event Action OnEquippableStateChanged;
    public event EquippableInstanceHandler OnInstanceEquipStateChanged;
    public event EquippableInstanceHandler OnInstanceEquippableStateChanged;
    public event Action OnPurchaseStateChanged;
    public event Action OnPurchasableStateChanged;
    public event PurchasableInstanceHandler OnInstancePurchaseStateChanged;
    public event PurchasableInstanceHandler OnInstancePurchasableStateChanged;


    public StoreItem()
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
            else if (CanBePurchased)
            {
                if (!IsPurchased)
                {
                    Buy();
                }
            }
        };
    }

    public void Equip()
    {
        IsEquipped = true;
        OnEquipStateChanged?.Invoke();
    }
    public void Uneqip()
    {
        IsEquipped = false;
        OnEquipStateChanged?.Invoke();
    }

    public void Buy()
    {
        if (CanBePurchased)
        {
            IsPurchased = true;
            CanBeEquipped = true;
            StatusEffectsManager.instance.ApplyStatusEffects(new StatusEffect("Покупка " + Title, -Price, StatusEffectType.Money, StatusEffectFrequency.OneShot));
            StatusEffectsManager.instance.ApplyStatusEffects(StatusEffects);
            OnPurchaseStateChanged?.Invoke();
        }
    }

    public void NotifyOnInstanceEquipStateChanged(IEquipable equipable)
    {
        OnInstanceEquipStateChanged.Invoke(equipable);
    }

    public void NotifyOnInstanceEquippableStateChanged(IEquipable equipable)
    {
        OnInstanceEquippableStateChanged.Invoke(equipable);
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
