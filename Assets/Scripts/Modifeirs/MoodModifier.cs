using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoodModifier : Modifier
{
    public MoodModifier(string name, int value, ModifierType type) : base (name, value, type) {
        }
}
