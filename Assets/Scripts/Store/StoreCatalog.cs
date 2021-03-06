﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreCatalogue", order = 1)]
public class StoreCatalog : ScriptableObject
{
    public string title;
    public List<StoreItem> items = new List<StoreItem>();


    public void Init()
    {
        AttachBehaviours();
    }

    public List<ItemCategory> GetCategories()
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
                switch (item.Type)
                {
                    case ItemType.Bed:
                        item.EquipBehavour = new BedEquipBehavour(item);
                        break;
                    case ItemType.Cupboard:
                        break;
                    case ItemType.SingleBuy:
                        break;
                    case ItemType.RealEstate:
                        break;
                    case ItemType.Consumable:
                        break;
                    case ItemType.Other:
                        break;
                    default:
                        break;
                }
            }
    }
}
