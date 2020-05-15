using Showcase.Bank;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoanView : DefaultItemView
{
    Loan loan;

    // Components
    Slider AmountSliderComponent { get; set; }
    Slider DurationSliderComponent { get; set; }

    Text AmountTextComponent { get; set; }
    Text DurationLabelTextComponent { get; set; }
    Button AcceptButtonComponent { get; set; }
    Text AcceptButtonTextComponent { get; set; }
    Image AcceptButtonBackgroudImage { get; set; }
    Text rateText { get; set; }
    Text resultAmountText { get; set; }


    // Fields
    public float Rate { get; set; }
    public float InitialValue { get; set; }
    public float Amount { get; set; }
    public float MaxValue { get; set; }

    public bool IsPurchased { get; set; }

    public List<Dropdown.OptionData> DurationDropdownOptions { get; set; } = new List<Dropdown.OptionData>();

    private float CalculateMonthlyPayment(float debtValue, float rate, int period)
    {
        float monthlyRate = rate / 12;
        int numberOfPeriods = period * 12;
        return (float) (monthlyRate * debtValue / (1 - Math.Pow(1 + monthlyRate, -numberOfPeriods)));
    }
    
    private void OnEnable()
    {
        TitleTextComponent = transform.Find("TitlePanel").Find("TitleText").GetComponent<Text>();
        
        var amountAndDurationPanelTransform = transform.Find("AmountAndDurationPanel");

        Transform amountPanelTransform = amountAndDurationPanelTransform.Find("AmountPanel");
        AmountTextComponent = amountPanelTransform.Find("AmountText").GetComponent<Text>();
        AmountSliderComponent = amountPanelTransform.Find("AmountSliderPanel").Find("Slider").GetComponent<Slider>();
        Transform durationPanelTransform = amountAndDurationPanelTransform.Find("DurationPanel");

        DurationLabelTextComponent = durationPanelTransform.Find("DurationLabel").GetComponent<Text>();
        DurationSliderComponent = durationPanelTransform.Find("DurationSliderPanel").Find("DurationSlider").GetComponent<Slider>();

        var infoPanelTransform = transform.Find("InfoPanel");
        rateText = infoPanelTransform.Find("RatePanel").Find("RateText").GetComponent<Text>();
        resultAmountText = infoPanelTransform.Find("ResultAmountPanel").Find("ResultAmountValuePanel").Find("ResultAmountValueText").GetComponent<Text>();

        Transform buttonTransform = transform.Find("ButtonAccept");
        AcceptButtonBackgroudImage = buttonTransform.GetComponent<Image>();
        AcceptButtonTextComponent = buttonTransform.transform.Find("Text").GetComponentInChildren<Text>();
        AcceptButtonComponent = buttonTransform.GetComponentInChildren<Button>();
        AmountSliderComponent.onValueChanged.AddListener(AmountSliderValueChangedHandler);
        DurationSliderComponent.onValueChanged.AddListener(DurationSliderValueChangedHandler);



        AcceptButtonComponent.onClick.RemoveAllListeners();
        AcceptButtonComponent.onClick.AddListener(delegate
        {
            if (loan.CanBePurchased)
            {
                if (!loan.IsPurchased)
                {
                    var confirmationHint = HintsManager.instance.ShowHint("Подтвердите согласие на кредит",
                        String.Format("Вы собираетесь взять кредит на ${0} под {1}% годовых на {2} лет", loan.Amount, loan.Rate * 100, (int)(loan.TotalPeriodInMonths / 12)), HintType.Confirmation);
                    (confirmationHint.View as IHintAccept).OnAccept += delegate { (Bank.instance as Bank).TakeLoan(loan); };
                }
                else
                {
                    // TODO: handle case if credit is purchased and user press the button
                }
            }
        });
    }
    private void OnDestroy()
    {
        if (loan)
        {
            loan.OnPurchaseStateChanged -= LoanPurchaseStateChangedHandler;
        }
    }

    void LoanPurchaseStateChangedHandler()
    {
        IsPurchased = loan.IsPurchased;
        UpdateButton();
    }

    void DurationSliderValueChangedHandler(float value)
    {
        loan.TotalPeriodInMonths = Mathf.RoundToInt(1 + (value * 4)) * 12;
        loan.Rate = 0.2f + (((float)loan.TotalPeriodInMonths / 12f) - 1f) / 80f;
        Rate = loan.Rate;
        UpdateView();
    }

    void AmountSliderValueChangedHandler(float value)
    {
        loan.Amount = loan.MixAmount + value * (loan.MaxAmount - loan.MixAmount);
        Amount = loan.Amount;
        UpdateView();
    }

    public void Init(Loan loan)
    {
        this.loan = loan;
        Title = loan.Title;
        Amount = loan.Amount;
        IsPurchased = loan.IsPurchased;

        loan.OnPurchaseStateChanged -= LoanPurchaseStateChangedHandler;
        loan.OnPurchaseStateChanged += LoanPurchaseStateChangedHandler;
    }

    public void UpdateAmount()
    {
        if (AmountTextComponent)
            AmountTextComponent.text = "Размер: " + Amount.ToString();
    }


    
    public void UpdateDuration()
    {
        if (DurationLabelTextComponent)
            DurationLabelTextComponent.text = rateText.text = String.Format("Срок: {0} лет", (int)(loan.TotalPeriodInMonths / 12));
    }

    public void UpdateButton()
    {
        if (AcceptButtonComponent)
        {
            AcceptButtonTextComponent.text = IsPurchased ? "Кредит активен" : "Взять";
            AcceptButtonBackgroudImage.color = IsPurchased ? GameDataManager.instance.InteractiveButtonInactiveColor : GameDataManager.instance.InteractiveButtonActiveColor;
        }
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateAmount();
        UpdateDuration();
        UpdateRate();
        UpdateResultAmount();
        UpdateButton();
    }

    private void UpdateRate()
    {
        rateText.text = String.Format("Ставка {0:0.##}%", loan.Rate * 100);
    }

    private void UpdateResultAmount()
    {
        resultAmountText.text = String.Format("{0:0.##}", loan.Amount * loan.Rate + loan.Amount);
    }
}
