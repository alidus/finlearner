using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier
{
    public Modifier(string name, int value, ModifierType type)
    {
        this.name = name;
        this.value = value;
        this.type = type;
    }
    public string name = "MODIFIER_NAME";
    public int value = 20;
    public ModifierType type = ModifierType.OneShot;
}