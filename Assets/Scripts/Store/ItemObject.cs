using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class ObjectItem : Item, IClickable, IPurchasable, IEquipable, IDrawable
{
    [SerializeField]
    private Sprite sprite;

    public UnityAction OnClick { get; set; }
    public bool CanBeEquipped { get; set; }

    public bool IsEquipped { get; private set; }
    public bool CanBePurchased { get; set; }
    public bool IsPurchased { get; private set; }

    public Sprite Sprite { get => sprite; set => sprite = value; }


    public ObjectItem()
    {
        OnClick = delegate
        {
            if (CanBeEquipped)
            {
                if (!IsEquipped)
                {
                    Equip();

                }
                else
                {
                    Uneqip();
                }
            }
            else if (CanBePurchased)
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
