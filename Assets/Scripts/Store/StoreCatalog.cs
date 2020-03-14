using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreCatalogue", order = 1)]
public class StoreCatalog : ScriptableObject
{
    public string title;
    public List<StoreItem> items = new List<StoreItem>();

    public StoreCatalog()
    {

    }

    public ItemCategory SelectedCategory { get; set; }

    public void Init()
    {
        AttachBehaviours();
    }

    public List<ItemCategory> GetPresentedCategories()
    {
        List<ItemCategory> result = new List<ItemCategory>();
        foreach (StoreItem item in items)
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
        foreach (StoreItem item in items)
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
        foreach (StoreItem item in items)
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
        foreach (StoreItem item in items)
        {
            if (item.Type == type)
            {
                result.Add(item);
            }
        }
        return result;
    }

    public void EquipItem(StoreItem storeItem)
    {
        foreach (StoreItem item in items)
        {
            if (item.Type == storeItem.Type)
            {
                if (storeItem == item)
                {
                    item.IsEquiped = true;
                } else
                {
                    item.IsEquiped = false;
                }
            }
        }
    }

    private void AttachBehaviours()
    {
        foreach (StoreItem item in items)
        {
            switch (item.Category)
            {
                case ItemCategory.Furniture:
                    item.ClicBehavior = new FurnitureItemClickBehavour(item);
                    break;
                case ItemCategory.Clothes:
                    break;
                case ItemCategory.FreeEstate:
                    break;
                case ItemCategory.Car:
                    break;
                case ItemCategory.Misc:
                    break;
                default:
                    break;
            }
        }
    }
}
