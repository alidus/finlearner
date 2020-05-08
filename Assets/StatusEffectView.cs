using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectView : View
{
    Text titleText;
    Text valueText;
    Text frequencyText;
    Image frequencyBackgroundImage;

    public string Title { get; set; }
    public float Value { get; set; }
    public StatusEffectFrequency Frequency { get; set; }


    private void OnEnable()
    {
        titleText = transform.Find("Background").Find("TopPanel").Find("TitleText").GetComponent<Text>();
        valueText = transform.Find("Background").Find("BottomPanel").Find("ValuePanel").Find("ValueText").GetComponent<Text>();
        frequencyText = transform.Find("Background").Find("BottomPanel").Find("FreqPanel").Find("FreqText").GetComponent<Text>();
        frequencyBackgroundImage = transform.Find("Background").Find("BottomPanel").Find("FreqPanel").GetComponent<Image>();
    }

    public void Init(StatusEffect statusEffect)
    {
        Title = statusEffect.Title;
        Value = statusEffect.Value;
        Frequency = statusEffect.Frequency;
        UpdateView();
    }


    public override void UpdateView()
    {
        UpdateTitle();
        UpdateValue();
        UpdateFrequency();
    }

    private void UpdateFrequency()
    {
        frequencyText.text = Frequency.ToString().Substring(0,1);
        frequencyBackgroundImage.color = GameDataManager.instance.GetColorForSEFrequency(Frequency);
        Debug.Log("F: " + Frequency.ToString() + ", Color: " + GameDataManager.instance.GetColorForSEFrequency(Frequency).ToString());
    }

    private void UpdateValue()
    {
        valueText.text = Value.ToString();
    }

    private void UpdateTitle()
    {
        titleText.text = Title;
    }
}
