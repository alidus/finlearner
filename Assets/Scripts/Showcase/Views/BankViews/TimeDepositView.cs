using Showcase.Bank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDepositView : DefaultItemView
{
    TimeDeposit timeDeposit;

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
        return (float)(monthlyRate * debtValue / (1 - Math.Pow(1 + monthlyRate, -numberOfPeriods)));
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
            if (timeDeposit.CanBePurchased)
            {
                if (!timeDeposit.IsPurchased)
                {
                    var confirmationHint = HintsManager.instance.ShowHint("Подтвердите согласие на вклад",
                        String.Format("Вы собираетесь оформить вклад на сумму ${0} под {1}% годовых", timeDeposit.Amount,
                        timeDeposit.Rate * 100, (int)(timeDeposit.TotalPeriodInMonths / 12)), HintType.Confirmation);
                    (confirmationHint.View as IHintAccept).OnAccept += delegate { (Bank.instance as Bank).MakeDeposit(timeDeposit); };
                }
                else
                {
                    // TODO: handle case if deposit is purchased and user press the button
                }
            }
        });
    }
    private void OnDestroy()
    {
        if (timeDeposit)
        {
            timeDeposit.OnPurchaseStateChanged -= LoanPurchaseStateChangedHandler;
        }
    }

    void LoanPurchaseStateChangedHandler()
    {
        IsPurchased = timeDeposit.IsPurchased;
        UpdateButton();
    }

    void DurationSliderValueChangedHandler(float value)
    {
        timeDeposit.TotalPeriodInMonths = Mathf.RoundToInt(1 + (value * 4)) * 12;
        timeDeposit.Rate = 0.2f + (((float)timeDeposit.TotalPeriodInMonths / 12f) - 1f) / 80f;
        Rate = timeDeposit.Rate;
        UpdateView();
    }

    void AmountSliderValueChangedHandler(float value)
    {
        timeDeposit.Amount = timeDeposit.MixAmount + value * (timeDeposit.MaxAmount - timeDeposit.MixAmount);
        Amount = timeDeposit.Amount;
        UpdateView();
    }

    public void Init(TimeDeposit timeDeposit)
    {
        this.timeDeposit = timeDeposit;
        Title = timeDeposit.Title;
        Amount = timeDeposit.Amount;
        IsPurchased = timeDeposit.IsPurchased;

        timeDeposit.OnPurchaseStateChanged -= LoanPurchaseStateChangedHandler;
        timeDeposit.OnPurchaseStateChanged += LoanPurchaseStateChangedHandler;
    }

    public void UpdateAmount()
    {
        if (AmountTextComponent)
            AmountTextComponent.text = "Размер: " + Amount.ToString();
    }



    public void UpdateDuration()
    {
        if (DurationLabelTextComponent)
            DurationLabelTextComponent.text = rateText.text = String.Format("Срок: {0} лет", (int)(timeDeposit.TotalPeriodInMonths / 12));
    }

    public void UpdateButton()
    {
        if (AcceptButtonComponent)
        {
            AcceptButtonTextComponent.text = IsPurchased ? "Вклад активен" : "Взять";
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
        rateText.text = String.Format("Ставка {0:0.##}%", timeDeposit.Rate * 100);
    }

    private void UpdateResultAmount()
    {
        resultAmountText.text = String.Format("{0:0.##}", timeDeposit.Amount * timeDeposit.Rate + timeDeposit.Amount);
    }
}
