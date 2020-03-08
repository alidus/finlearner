using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumerable class containing modifiers array and methods to manipulate its data
/// </summary>
public class ModifiersContainer : IEnumerable
{
    private List<Modifier> modifiers;

	public ModifiersContainer()
	{
		modifiers = new List<Modifier>();
	}
	public System.Collections.Generic.List<Modifier> Modifiers
	{
		get { return modifiers; }
		set { modifiers = value; }
	}

	public System.Collections.IEnumerator GetEnumerator()
	{
		return modifiers.GetEnumerator();
	}

	public void AddRange(List<Modifier> modifiers)
	{
		this.modifiers.AddRange(modifiers);
	}

    public void Add(Modifier modifier)
    {
        this.modifiers.Add(modifier);
    }
}
