using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/JobTemplate")]
public class JobSOTemplate : ScriptableObject
{
    public string title;
    public string description;
    public List<StatusEffect> modifiers = new List<StatusEffect>();
}
