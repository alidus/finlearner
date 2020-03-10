using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    Furniture, Clothes, FreeEstate, Car, Misc
}

public enum ItemType
{
    Bed, Chair, Armchair, Table, Other
}

[System.Serializable]
public class StoreItem
{
    [SerializeField]
    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    [SerializeField]
    private string desc;
    public string Desc
    {
        get { return desc; }
        set { desc = value; }
    }
    [SerializeField]
    private int price;
    public int Price
    {
        get { return price; }
        set { price = value; }
    }
    [SerializeField]
    private bool isOwned;
    public bool IsOwned
    {
        get { return isOwned; }
        set { isOwned = value; }
    }
    [SerializeField]
    private bool isEquiped;
    public bool IsEquiped
    {
        get { return isEquiped; }
        set { isEquiped = value; }
    }
    [SerializeField]
    private ItemCategory category = ItemCategory.Misc;
    public ItemCategory Category
    {
        get { return category; }
        set { category = value; }
    }
    [SerializeField]
    private ItemType type = ItemType.Other;
    public ItemType Type
    {
        get { return type; }
        set { type = value; }
    }
    [SerializeField]
    private List<StatusEffect> modifiers;
    public List<StatusEffect> Modifiers
    {
        get { return modifiers; }
        set { modifiers = value; }
    }
    [SerializeField]
    private Sprite sprite;
    public UnityEngine.Sprite Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }
    public IEquipable EquipBehavour { get; set; }
}
