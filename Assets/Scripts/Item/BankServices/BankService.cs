using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class BankService : Item, IClickable, IHaveStatusEffect
{
    [SerializeField]
    private float value;
    [SerializeField]
    private bool canBePurchased = true;
    [SerializeField] 
    private float rate;
    [SerializeField]
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    public UnityAction OnClick { get; set; }
    public bool CanBePurchased { get => canBePurchased; set => canBePurchased = value; }
    public float Value { get => value; set => this.value = value; }

    public float Rate { get => rate; set => rate = value; }

    public List<StatusEffect> StatusEffects { get => statusEffects; set => statusEffects = value; }
    public event Action OnBuy;


    public BankService()
    {
        SetupClickAction();
    }

    private void SetupClickAction()
    {
        OnClick = delegate
        {
            if (CanBePurchased)
            {
                Get();
            }
        };
    }

    public void Get()
    {
        if (CanBePurchased)
        {
            OnBuy?.Invoke();
        }
    }
}
