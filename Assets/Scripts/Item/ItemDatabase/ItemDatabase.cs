using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemDatabase<T> : IEnumerable where T : Item
{
	[SerializeField]
	private List<T> items = new List<T>();
	public List<T> Items { get => items; set => items = value; }

	public int Count { get => Items.Count; }

	public ItemDatabase()
	{

	}
	public ItemDatabase(IEnumerable<T> items)
	{
		Items = items.ToList();
	}
	public void Add(T item)
	{
		Items.Add(item);
	}
	public void Add(List<T> items)
	{
		Items.AddRange(items);
	}

	public void Clear()
	{
		Items.Clear();
	}

	public IEnumerator GetEnumerator()
	{
		return items.GetEnumerator();
	}
}
