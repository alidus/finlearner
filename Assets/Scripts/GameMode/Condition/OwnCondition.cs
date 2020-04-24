using UnityEngine;
using System.Collections;
[System.Serializable]
public class OwnCondition : GameCondition
{
	[SerializeField]
	private Item targetItem;
    public Item TargetItem { get => targetItem; set => targetItem = value; }

	public OwnCondition(Item item) : base()
	{
		if (TargetItem is IPurchasable)
		{
            TargetItem = item;
        } else
		{
			Debug.Log("Creating Own Condition for an item that cannot be purchased");
		}
    }

	public override void SubscribeToTargetDataChanges()
	{
        // Fulfill condition on item purchase if the item can be purchased, otherwise fulfill on equip
		var purchasable = (TargetItem as IPurchasable);
		if (purchasable != null)
		{
            // In case if target item is concrete instance of purchasable item
            purchasable.OnPurchaseStateChanged -= () => State = purchasable.IsPurchased;
            purchasable.OnPurchaseStateChanged += () => State = purchasable.IsPurchased;

            // In case if item is general SO
            // TODO: make advanced logic of check for another purchased item of target type
            purchasable.OnInstancePurchaseStateChanged -= (IPurchasable purchasableInstance) => State = purchasableInstance.IsPurchased;
            purchasable.OnInstancePurchaseStateChanged += (IPurchasable purchasableInstance) => State = purchasableInstance.IsPurchased;
        } else
		{
			var equippable = (TargetItem as IEquipable);
			if (equippable != null)
			{
                // In case if target item is concrete instance of equippable item
                equippable.OnEquipStateChanged -= () => State = equippable.IsEquipped;
                equippable.OnEquipStateChanged += () => State = equippable.IsEquipped;

                // In case if item is general SO
                // TODO: make advanced logic of check for another purchased item of target type
                equippable.OnInstanceEquipStateChanged -= (IEquipable equipableInstance) => State = equipableInstance.IsEquipped;
                equippable.OnInstanceEquipStateChanged += (IEquipable equipableInstance) => State = equipableInstance.IsEquipped;
            }
		}
		

    }
}
