using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeFurnitureEquipBehavour : IEquipable
{
    StoreItem item;
    private HouseManager houseManager;

    public HomeFurnitureEquipBehavour(StoreItem item)
    {
        this.item = item;
        houseManager = HouseManager.instance;
    }
    public void Equip()
    {
        houseManager.SetFurniture(this.item);
    }
}
