using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType { Money, Mood}
public enum ModifierEffectFreqency { OneShot, Daily, Weekly, Yearly }

[System.Serializable]
public class Modifier
{
    [SerializeField]
    private string name = "MODIFIER_NAME";
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    [SerializeField]
    private int value = 20;
    public int Value
    {
        get { return value; }
        set { this.value = value; }
    }
    [SerializeField]
    private ModifierEffectFreqency freqency = ModifierEffectFreqency.OneShot;
    public ModifierEffectFreqency Freqency
    {
        get { return freqency; }
        set { Freqency = value; }
    }
    [SerializeField]
    private ModifierType type;
    public ModifierType Type
    {
        get { return type; }
        set { type = value; }
    }
    
    public Modifier(string name, int value, ModifierType type, ModifierEffectFreqency freqency)
    {
        this.Name = name;
        this.Value = value;
        this.Freqency = freqency;
    }
    
}