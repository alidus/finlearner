using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType
{
    Daily,
    Weekly,
    OneShot
}

[System.Serializable]
public class IncomeModifier : Modifier
{
    public IncomeModifier(string name, int value, ModifierType type) : base(name, value, type)
    {
    }
}
