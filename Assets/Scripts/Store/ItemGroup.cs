using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains list of Item objects and methods to manipulate with them (Similar to Database, but used in cases where you need list of somehow-related objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class ItemGroup<T> : IEnumerable<T> where T:Item
{

    public string Title { get; set; } = "GROUP_TITLE";
    public List<T> Items { get; set; } = new List<T>();

    public ItemGroup(IEnumerable<T> items)
    {
        Items = items.ToList();
    }

    public ItemGroup()
    {

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

    public void Remove(T item)
    {
        Items.Remove(item);
    }

    public override string ToString()
    {
        return "Title: " + Title + ", Items: " + Items.ToString();
    }
}