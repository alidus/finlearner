﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class ObjectItem : Item, IClickable, IPurchasable, IEquipable, IDrawable
{
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private float price = 200;
    [SerializeField]
    private bool canBePurchased = true;
    [SerializeField]
    private bool canBeEquipped = false;
    [SerializeField]
    private bool isEquipped = false;
    [SerializeField]
    private bool isPurchased = false;

    public UnityAction OnClick { get; set; }
    public bool CanBeEquipped { get => canBeEquipped; set => canBeEquipped = value; }
    public bool IsEquipped { get => isEquipped; private set => isEquipped = value; }
    public bool CanBePurchased { get => canBePurchased; set => canBePurchased = value; }
    public bool IsPurchased { get => isPurchased; private set => isPurchased = value; }
    public float Price { get => price; set => price = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }

    public event Action OnBuy;
    public event Action OnEquip;
    public event Action OnSell;
    public event Action OnUnEquip;


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
                    Buy();
                }
            }
        };
    }

    public void Equip()
    {
        IsEquipped = true;
        OnEquip?.Invoke();
    }
    public void Uneqip()
    {
        IsEquipped = false;
        OnUnEquip?.Invoke();
    }

    public void Buy()
    {
        if (CanBePurchased)
        {
            CanBeEquipped = true;
            OnBuy?.Invoke();
        }
    }
}
