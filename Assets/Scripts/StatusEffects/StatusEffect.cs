using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType { Money, Mood}
public enum StatusEffectFrequency { OneShot, Daily, Weekly, Monthly, Yearly }

[System.Serializable]
public class StatusEffect
{
    [SerializeField]
    private string name = "MODIFIER_NAME";
    public string Title
    {
        get { return name; }
        set { name = value; }
    }
    [SerializeField]
    private float value = 20;
    public float Value
    {
        get { return value; }
        set { this.value = value; }
    }
    [SerializeField]
    private StatusEffectFrequency freqency = StatusEffectFrequency.OneShot;
    public StatusEffectFrequency Freqency
    {
        get { return freqency; }
        set { freqency = value; }
    }
    [SerializeField]
    private StatusEffectType type;
    public StatusEffectType Type
    {
        get { return type; }
        set { type = value; }
    }
    
    public StatusEffect(string name, float value, StatusEffectType type, StatusEffectFrequency freqency)
    {
        this.Title = name;
        this.Value = value;
        this.Type = type;
        this.Freqency = freqency;
    }
    
}