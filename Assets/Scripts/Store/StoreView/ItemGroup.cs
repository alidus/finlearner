using System.Collections;
using System.Collections.Generic;

public class ItemGroup<T> : IEnumerable<T> where T:Item
{
    public string Title { get; set; }
    public List<T> Items { get; set; }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}