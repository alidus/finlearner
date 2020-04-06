using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Showcase<T> : MonoBehaviour where T : Item
{
    public ItemDatabase<T> ItemDatabase { get; set; } = new ItemDatabase<T>();
    public List<ItemGroup<T>> ItemGroups { get; set; }
    public ItemGroup<T> SelectedItemGroup { get; set; }
    public View RootView { get; set; }
    /// <summary>
    /// Splits ItemDatabase into different item groups to display in showcase, logic is specified in concrete showcase
    /// </summary>
    /// <returns></returns>
    protected abstract List<ItemGroup<T>> GetItemGroups();
    /// <summary>
    /// Update showcase view
    /// </summary>
    public abstract void UpdateShowcase();
    /// <summary>
    /// Destroy showcase view
    /// </summary>
    private void OnDisable()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
