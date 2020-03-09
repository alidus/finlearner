using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeFurnitureEquipBehavour : IEquipable
{
    StoreItem item;

    public HomeFurnitureEquipBehavour(StoreItem item)
    {
        this.item = item;
    }
    public void Equip()
    {
        HouseManager.SetFurniture(this.item);
    }
}
