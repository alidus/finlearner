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
		var purchasable = (TargetItem as IPurchasable);
		// In case if target item is concrete instance of purchasable item
        purchasable.OnBuy -= () => State = true;
        purchasable.OnBuy += () => State = true;
		purchasable.OnSell -= () => State = false;
        purchasable.OnSell += () => State = false;
		// In case if item is general SO
		// TODO: make advanced logic of check for another purchased item of target type
        purchasable.OnInstanceBuy -= () => State = true;
        purchasable.OnInstanceBuy += () => State = true;
        purchasable.OnInstanceSell -= () => State = false;
        purchasable.OnInstanceSell += () => State = false;

    }
}
