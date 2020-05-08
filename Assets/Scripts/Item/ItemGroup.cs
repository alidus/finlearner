using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains list of Item objects and methods to manipulate with them (Similar to Database, but used in cases where you need list of somehow-related objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class ItemGroup<T> : IEnumerable<T>, ISelectable
{
    private bool isSelected;

    public string Title { get; set; } = "GROUP_TITLE";
    public List<T> Items { get; set; } = new List<T>();

    public bool IsSelected { get => isSelected; set { if (isSelected != value) { isSelected = value; OnSelectedStateChanged?.Invoke(); } } }

    public event Action OnSelectedStateChanged;

    public ItemGroup<T> ParentGroup { get; set; }

    public ItemGroup(IEnumerable<T> items, string title = "ITEM_GROUP_TITLE")
    {
        Items = items.ToList();
        Title = title;
    }

    public ItemGroup(string title = "ITEM_GROUP_TITLE")
    {
        Title = title;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        Items.Add(item);
    }

    public bool Remove(T item)
    {
       return Items.Remove(item);
    }

    public override string ToString()
    {
        return "Title: " + Title + ", Items: " + Items.ToString();
    }
}