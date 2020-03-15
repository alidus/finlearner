using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Button;

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
    private string title;
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    [SerializeField]
    private string desc;
    public string Desc
    {
        get { return desc; }
        set { desc = value; }
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
    private List<StatusEffect> statusEffects;
    public List<StatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }
    [SerializeField]
    private Sprite sprite;
    public UnityEngine.Sprite Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }

    [SerializeField]
    private List<AbstractStoreItemBehaviour> behaviours = new List<AbstractStoreItemBehaviour>();
    public List<AbstractStoreItemBehaviour> Behaviours { private get { return behaviours; } set { behaviours = value; } }

    public T AddBehaviour<T>(AbstractStoreItemBehaviour behaviour) where T : AbstractStoreItemBehaviour
    {
        Behaviours.Add(behaviour);
        return GetBehaviour<T>();
    }

    public bool RemoveBehaviour<T>() where T : AbstractStoreItemBehaviour
    {
        return Behaviours.Remove(GetBehaviour<T>());
    }
    public T GetBehaviour<T>() where T : AbstractStoreItemBehaviour
    {
        return (T)Behaviours.Find(behaviour => behaviour is T);
    }

    public void Click()
    {
        foreach (AbstractStoreItemBehaviour storeItemClickBehaviour in Behaviours)
        {
            storeItemClickBehaviour.Execute();
        }
    }
}
