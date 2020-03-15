using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FurnitureEquipBehaviour : AbstractStoreItemBehaviour
{
    StoreItem item;

    public bool IsEquipped { get; set; }
    public bool CanBeEquipped { get; set; } = true;

    public ItemPurchaseBehaviour PurchaseBehaviour { get; private set; }
    private HouseManager houseManager;

    public FurnitureEquipBehaviour(StoreItem item)
    {
        this.item = item;
        PurchaseBehaviour = item.GetBehaviour<ItemPurchaseBehaviour>();
        houseManager = HouseManager.instance;
    }

    public override void Execute()
    {
        if (!IsEquipped && CanBeEquipped && !(PurchaseBehaviour != null && PurchaseBehaviour.IsPurchased == false))
        {
            IsEquipped = true;
            houseManager.EquipFurniture(item);
        }
    }
}
