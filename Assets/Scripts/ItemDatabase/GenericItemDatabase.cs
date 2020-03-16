using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemDatabase<T> : IEnumerable where T : Item
{
	[SerializeField]
	private List<T> items;
	public List<T> Items { get => items; set => items = value; }

	public ItemDatabase()
	{

	}
	public ItemDatabase(IEnumerable<T> items)
	{
		Items = items.ToList();
	}
	public void AddItems(T item)
	{
		Items.Add(item);
	}
	public void AddItems(List<T> items)
	{
		Items.AddRange(items);
	}

	public IEnumerator GetEnumerator()
	{
		return items.GetEnumerator();
	}
}
