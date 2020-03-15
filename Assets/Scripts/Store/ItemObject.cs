using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class ItemObject : Item, IClickable, IPurchasable, IEquipable, IDrawable
{
    public enum ItemObjectType { Furniture, Clothing}
    

    public UnityAction OnClick { get; set; }
    public bool CanBeEquipped { get; set; }

    public bool IsEquipped { get; private set; }
    public bool CanBePurchased { get; set; }
    public bool IsPurchased { get; private set; }
    public ItemType Type { get => type; set => type = value; }
    public ItemObjectType ObjectType { get => objectType; set => objectType = value; }
    public Sprite Sprite { get; set; }

    [SerializeField]
    private ItemType type;
    [SerializeField]
    private ItemObjectType objectType;

    public ItemObject()
    {
        OnClick = delegate { 
            if (CanBeEquipped)
            {
                if (!IsEquipped)
                {
                    Equip();

                } else
                {
                    Uneqip();
                }
            } else if (CanBePurchased)
            {
                if (!IsPurchased)
                {
                    Purchase();
                }
            }
        };
    }

    public void Equip()
    {
        IsEquipped = true;
    }
    public void Uneqip()
    {
        IsEquipped = false;
    }

    public void Purchase()
    {
        if (CanBePurchased)
        {
            CanBeEquipped = true;
        }
    }
}
