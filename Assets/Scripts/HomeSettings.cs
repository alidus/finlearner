using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HomeSettings
{
    public static StoreItem Bed {get; set;}
    public static StoreItem Chair { get; set; }
    public static StoreItem Armchair { get; set; }
    public static StoreItem Table { get; set; }

    public static void EquipItem(StoreItem item)
    {
        switch (item.Type)
        {
            case ItemType.Bed:
                Bed = item;
                break;
            case ItemType.Chair:
                Chair = item;
                break;
            case ItemType.Armchair:
                Armchair = item;
                break;
            case ItemType.Table:
                Table = item;
                break;
            case ItemType.Other:
                break;
            default:
                break;
        }
    }
}
