using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class BankService : Item, IClickable, IHaveStatusEffect
{
    [SerializeField] private float value;
    [SerializeField] private bool canBePurchased = true;
    [SerializeField] private float rate;
    [SerializeField] private List<StatusEffect> statusEffects = new List<StatusEffect>();
    [SerializeField] public float initialValue;
    [SerializeField] public string amountTitle;
    [SerializeField] public float maxValue;
    [SerializeField] public string buttonText;
    [SerializeField] public string amountValueText;
    [SerializeField] public string amountInputFieldPlaceholderText;
    [SerializeField] public string durationLabelText;
    [SerializeField] public List<Dropdown.OptionData> durationDropdownOptions;

    public bool CanBePurchased { get => canBePurchased; set => canBePurchased = value; }
    public float Value { get => value; set => this.value = value; }

    public float Rate { get => rate; set => rate = value; }

    public List<StatusEffect> StatusEffects { get => statusEffects; set => statusEffects = value; }

    public float InitialValue { get => initialValue; set => initialValue = value; }

    public string AmountTitle { get => amountTitle; set => amountTitle = value; }

    public float MaxValue { get => maxValue; set => maxValue = value; }

    public string ButtonText { get => buttonText; set => buttonText = value; }

    public string AmountValueText { get => amountValueText; set => amountValueText = value; }

    public string AmountInputFieldPlaceholderText
    { get => amountInputFieldPlaceholderText; set => amountInputFieldPlaceholderText = value; }

    public string DurationLabelText { get => durationLabelText; set => durationLabelText = value; }

    public List<Dropdown.OptionData> DurationDropdownOptions 
    { get => durationDropdownOptions; set => durationDropdownOptions = value; }

    public event Action OnBuy;
    public UnityAction OnClick { get; set; }


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
