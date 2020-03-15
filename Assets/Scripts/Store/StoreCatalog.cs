using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreCatalogue", order = 1)]
public class StoreCatalog : ScriptableObject
{
    [SerializeField]
    private List<StoreItem> items = new List<StoreItem>();
    public List<StoreItem> Items
    {
        get { return items; }
        set { items = value; }
    }
    public StoreCatalog()
    {

    }

    public ItemCategory SelectedCategory { get; set; }

    public void Init()
    {

    }

    public List<ItemCategory> GetPresentedCategories()
    {
        List<ItemCategory> result = new List<ItemCategory>();
        foreach (StoreItem item in Items)
        {
            if (!result.Contains(item.Category))
            {
                result.Add(item.Category);
            }
            
        }
        return result;
    }

    public List<ItemType> GetPresentedTypes()
    {
        List<ItemType> result = new List<ItemType>();
        foreach (StoreItem item in Items)
        {
            if (!result.Contains(item.Type))
            {
                result.Add(item.Type);
            }

        }
        return result;
    }

    public List<StoreItem> GetAllItemsOfCategory(ItemCategory category)
    {
        List<StoreItem> result = new List<StoreItem>();
        foreach (StoreItem item in Items)
        {
            if (item.Category == category)
            {
                result.Add(item);
            }
        }
        return result;
    }

    public List<StoreItem> GetAllItemsOfType(ItemType type)
    {
        List<StoreItem> result = new List<StoreItem>();
        foreach (StoreItem item in Items)
        {
            if (item.Type == type)
            {
                result.Add(item);
            }
        }
        return result;
    }
}
