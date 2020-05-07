using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BankServiceView : DefaultItemView
{
    // Components
    public HorizontalLayoutGroup AmountComponent { get; set; }
    public Slider AmountSliderComponent { get; set; }
    public Text AmountTitleTextComponent { get; set; }
    public Text AmountValueTextComponent { get; set; }
    public Text DurationLabelTextComponent { get; set; }
    public InputField AmountInputFieldComponent { get; set; }
    public Text AmountInputFieldPlaceholderTextComponent { get; set; }
    public Text ParametersTextComponent { get; set; }
    public Dropdown DurationDropdownComponent { get; set; }
    public Text DurationDropdownTextComponent { get; set; }
    public Button ButtonComponent { get; set; }
    public Text ButtonTextComponent { get; set; }
    
    // Fields
    public float Rate { get; set; }
    public float InitialValue { get; set; }
    public string AmountTitle { get; set; }
    public float MaxValue { get; set; }
    public string ButtonText { get; set; }
    public string AmountValueText { get; set; }
    public string AmountInputFieldPlaceholderText { get; set; }
    public string DurationLabelText { get; set; }
    public List<Dropdown.OptionData> DurationDropdownOptions { get; set; }

    private float CalculateMonthlyPayment(float debtValue, float rate, int period)
    {
        float monthlyRate = rate / 12;
        int numberOfPeriods = period * 12;
        return (float) (monthlyRate * debtValue / (1 - Math.Pow(1 + monthlyRate, -numberOfPeriods)));
    }
    
    private void OnEnable()
    {
        TitleTextComponent = transform.Find("Title").GetComponent<Text>();
        
        Transform amountComponent = transform.Find("Amount");
        if (amountComponent)
        {
            AmountTitleTextComponent = amountComponent.transform.Find("TitleText").GetComponent<Text>();
            AmountSliderComponent = amountComponent.transform.Find("Slider").GetComponent<Slider>();
            AmountValueTextComponent = amountComponent.transform.Find("ValueText").GetComponent<Text>();
            
        }
        AmountComponent = amountComponent.GetComponent<HorizontalLayoutGroup>();
        DurationLabelTextComponent = transform.Find("DurationLabel").GetComponent<Text>();
        
        Transform amountInputFieldComponent = transform.Find("AmountInputField");
        if (amountInputFieldComponent)
        {
            AmountInputFieldPlaceholderTextComponent = amountInputFieldComponent.transform.Find("Placeholder").GetComponent<Text>();
        }
        AmountInputFieldComponent = amountInputFieldComponent.GetComponent<InputField>();
        
        ParametersTextComponent = transform.Find("Parameters").GetComponent<Text>();
        
        Transform durationDropdownComponent = transform.Find("DurationDropdown");
        if (durationDropdownComponent)
        {
            DurationDropdownTextComponent = durationDropdownComponent.transform.Find("Label").GetComponentInChildren<Text>();
        }
        DurationDropdownComponent = durationDropdownComponent.GetComponentInChildren<Dropdown>();
        
        Transform buttonComponent = transform.Find("Button");
        if (buttonComponent)
        {
            ButtonTextComponent = buttonComponent.transform.Find("Text").GetComponentInChildren<Text>();
        }

        ButtonComponent = buttonComponent.GetComponentInChildren<Button>();
    }

    public void UpdateAmount()
    {
        if (AmountTitleTextComponent)
            AmountTitleTextComponent.text = AmountTitle;
        
        if (AmountSliderComponent)
        {
            AmountSliderComponent.maxValue = MaxValue;
            AmountSliderComponent.value = InitialValue;
        }
        
        if (AmountValueTextComponent)
            AmountValueTextComponent.text = AmountValueText;
    }

    public void UpdateAmountInputField()
    {
        if (AmountInputFieldPlaceholderTextComponent)
            AmountInputFieldPlaceholderTextComponent.text = AmountInputFieldPlaceholderText;
    }
    
    public void UpdateDurationLabel()
    {
        if (DurationLabelTextComponent)
            DurationLabelTextComponent.text = DurationLabelText;
    }
    
    public void UpdateParameters()
    {
        string messageTemplate = "Your monthly payment will be {0}";
        float monthlyPayment =
            CalculateMonthlyPayment(AmountSliderComponent.value, Rate, DurationDropdownComponent.value);
        string message = string.Format(messageTemplate, monthlyPayment);
        if (ParametersTextComponent)
            ParametersTextComponent.text = message;
    }
    
    public void UpdateDurationDropdown()
    {
        if (DurationDropdownComponent)
            DurationDropdownComponent.options = DurationDropdownOptions;
    }
    
    public void UpdateButton()
    {
        if (ButtonComponent)
            ButtonTextComponent.text = ButtonText;
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateAmount();
        UpdateAmountInputField();
        UpdateDurationLabel();
        UpdateParameters();
        UpdateDurationDropdown();
        UpdateButton();
    }
}
