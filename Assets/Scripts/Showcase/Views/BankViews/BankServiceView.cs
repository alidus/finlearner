using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BankServiceView : DefaultItemView
{
    public Text TitleTextComponent { get; set; }
    public HorizontalLayoutGroup AmountComponent { get; set; }
    public Slider AmountSliderComponent { get; set; }
    public Text AmountTitleTextComponent { get; set; }
    public Text ValueTextComponent { get; set; }
    public Text DurationLabelComponent { get; set; }
    public InputField AmountInputFieldComponent { get; set; }
    public Text ParametersTextComponent { get; set; }
    public Dropdown DurationDropdownComponent { get; set; }
    public Text DurationDropdownTextComponent { get; set; }
    public Button ButtonComponent { get; set; }
    public Text ButtonText { get; set; }
    private void OnEnable()
    {
        TitleTextComponent = transform.Find("Title").GetComponent<Text>();
        Transform amountComponent = transform.Find("Amount");
        if (amountComponent)
        {
            AmountTitleTextComponent = amountComponent.transform.Find("TitleText").GetComponent<Text>();
            AmountSliderComponent = amountComponent.transform.Find("Slider").GetComponent<Slider>();
            ValueTextComponent = amountComponent.transform.Find("ValueText").GetComponent<Text>();
            
        }
        AmountComponent = amountComponent.GetComponent<HorizontalLayoutGroup>();
        DurationLabelComponent = transform.Find("DurationLabel").GetComponent<Text>();
        AmountInputFieldComponent = transform.Find("AmountInputField").GetComponent<InputField>();
        ParametersTextComponent = transform.Find("ParametersText").GetComponent<Text>();
        Transform durationDropdownComponent = transform.Find("DurationDropdown");
        if (durationDropdownComponent)
        {
            DurationDropdownTextComponent = durationDropdownComponent.transform.Find("Label").GetComponentInChildren<Text>();
        }
        DurationDropdownComponent = durationDropdownComponent.GetComponentInChildren<Dropdown>();
        Transform buttonComponent = transform.Find("Button");
        if (buttonComponent)
        {
            ButtonText = buttonComponent.transform.Find("Text").GetComponentInChildren<Text>();
        }

        ButtonComponent = buttonComponent.GetComponentInChildren<Button>();
    }

    public void UpdateAmountTitle()
    {
        if (AmountTitleTextComponent)
            AmountTitleTextComponent.text = "Размер кредита";
    }
    
    public void UpdateAmountSlider()
    {
        if (AmountSliderComponent)
            AmountSliderComponent.maxValue = 1000000f;
    }

    public void UpdateButton()
    {
        if (ButtonComponent)
            ButtonText.text = "Взять кредит";
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateButton();
    }
}
