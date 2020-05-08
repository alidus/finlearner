using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType { Money, Mood}
public enum StatusEffectFrequency { OneShot, Daily, Weekly, Monthly, Yearly }
[Flags]
public enum StatusEffectFlags { Job = 1, Education = 2, Exhaustion = 4 }



[System.Serializable]
public class StatusEffect
{
    [SerializeField]
    private string title;
    public string Title
    {
        get { return title; }
        set { title = value; }
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
    public StatusEffectFrequency Frequency
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
    [SerializeField]
    private StatusEffectFlags flags;
    public StatusEffectFlags Flags { get => flags; set => flags = value; }

    public StatusEffect(string name, float value, StatusEffectType type, StatusEffectFrequency freqency, StatusEffectFlags flags = 0)
    {
        Title = name;
        Value = value;
        Type = type;
        Frequency = freqency;
        Flags = flags;
    }

    public override string ToString()
    {
        string result = "";
        result += "Title: " + Title + ", ";
        result += "Value: " + Value.ToString() + ", ";
        result += "Type: " + Type.ToString() + ", ";
        result += "Frequency: " + Frequency.ToString() + ", ";
        result += "Flags: " + Flags.ToString() + ".";

        return result;
    }

}