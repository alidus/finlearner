using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureItemClickBehavour : IStoreItemClickBehaviour
{
    StoreItem item;

    public bool IsEquipped { get; set; } 
    private HouseManager houseManager;

    public FurnitureItemClickBehavour(StoreItem item)
    {
        this.item = item;
        houseManager = HouseManager.instance;
    }

    public void Click()
    {
        if (!IsEquipped)
        {
            houseManager.EquipFurniture(item);
        }
    }
}
