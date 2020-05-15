using Showcase.Bank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentDepositView : DefaultItemView
{
    CurrentDeposit currentDeposit;

    // Components
    Slider AmountSliderComponent { get; set; }

    Text AmountTextComponent { get; set; }
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

        Transform amountPanelTransform = transform.Find("AmountPanel");
        AmountTextComponent = amountPanelTransform.Find("AmountText").GetComponent<Text>();
        AmountSliderComponent = amountPanelTransform.Find("AmountSliderPanel").Find("Slider").GetComponent<Slider>();

        var infoPanelTransform = transform.Find("InfoPanel");
        rateText = infoPanelTransform.Find("RatePanel").Find("RateText").GetComponent<Text>();
        resultAmountText = infoPanelTransform.Find("ResultAmountPanel").Find("ResultAmountValuePanel").Find("ResultAmountValueText").GetComponent<Text>();

        Transform buttonTransform = transform.Find("ButtonAccept");
        AcceptButtonBackgroudImage = buttonTransform.GetComponent<Image>();
        AcceptButtonTextComponent = buttonTransform.transform.Find("Text").GetComponentInChildren<Text>();
        AcceptButtonComponent = buttonTransform.GetComponentInChildren<Button>();
        AmountSliderComponent.onValueChanged.AddListener(AmountSliderValueChangedHandler);



        AcceptButtonComponent.onClick.RemoveAllListeners();
        AcceptButtonComponent.onClick.AddListener(delegate
        {
            if (currentDeposit.CanBePurchased)
            {
                if (!currentDeposit.IsPurchased)
                {
                    var confirmationHint = HintsManager.instance.ShowHint("Подтвердите согласие на вклад",
                        String.Format("Вы собираетесь оформить вклад на сумму ${0} под {1}% годовых", currentDeposit.Amount,
                        currentDeposit.Rate * 100, (int)(currentDeposit.TotalPeriodInMonths / 12)), HintType.Confirmation);
                    (confirmationHint.View as IHintAccept).OnAccept += delegate { (Bank.instance as Bank).MakeDeposit(currentDeposit); };
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
        if (currentDeposit)
        {
            currentDeposit.OnPurchaseStateChanged -= LoanPurchaseStateChangedHandler;
        }
    }

    void LoanPurchaseStateChangedHandler()
    {
        IsPurchased = currentDeposit.IsPurchased;
        UpdateButton();
    }


    void AmountSliderValueChangedHandler(float value)
    {
        currentDeposit.Amount = currentDeposit.MixAmount + value * (currentDeposit.MaxAmount - currentDeposit.MixAmount);
        Amount = currentDeposit.Amount;
        UpdateView();
    }

    public void Init(CurrentDeposit currentDeposit)
    {
        this.currentDeposit = currentDeposit;
        Title = currentDeposit.Title;
        Amount = currentDeposit.Amount;
        IsPurchased = currentDeposit.IsPurchased;

        currentDeposit.OnPurchaseStateChanged -= LoanPurchaseStateChangedHandler;
        currentDeposit.OnPurchaseStateChanged += LoanPurchaseStateChangedHandler;
    }

    public void UpdateAmount()
    {
        if (AmountTextComponent)
            AmountTextComponent.text = "Размер: " + Amount.ToString();
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
        UpdateRate();
        UpdateResultAmount();
        UpdateButton();
    }

    private void UpdateRate()
    {
        rateText.text = String.Format("Ставка {0:0.##}%", currentDeposit.Rate * 100);
    }

    private void UpdateResultAmount()
    {
        resultAmountText.text = String.Format("{0:0.##}", currentDeposit.Amount * currentDeposit.Rate + currentDeposit.Amount);
    }
}
