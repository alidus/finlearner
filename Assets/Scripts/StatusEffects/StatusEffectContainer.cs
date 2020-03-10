using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumerable class containing modifiers array and methods to manipulate its data
/// </summary>
public class StatusEffectContainer : IEnumerable
{
    private List<StatusEffect> modifiers;

	public StatusEffectContainer()
	{
		modifiers = new List<StatusEffect>();
	}
	public System.Collections.Generic.List<StatusEffect> Modifiers
	{
		get { return modifiers; }
		set { modifiers = value; }
	}

	public System.Collections.IEnumerator GetEnumerator()
	{
		return modifiers.GetEnumerator();
	}

	public void AddRange(List<StatusEffect> modifiers)
	{
		this.modifiers.AddRange(modifiers);
	}

    public void Add(StatusEffect modifier)
    {
        this.modifiers.Add(modifier);
    }
}
