using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase<T> : ScriptableObject where T : Item
{
    public List<T> Items { get; set; }

	public ItemDatabase()
	{

	}
	public ItemDatabase(List<T> items)
	{
		Items = items;
	}
	public void AddItems(T item)
	{
		Items.Add(item);
	}
	public void AddItems(List<T> items)
	{
		Items.AddRange(items);
	}
}
