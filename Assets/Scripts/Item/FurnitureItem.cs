using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FurnitureType { Bed, Table, Chair, Armchair }

[CreateAssetMenu(menuName = "SO/Items/Furniture", fileName = "Furniture")]
public class FurnitureItem : StoreItem, IClickable, IPurchasable, IEquipable, IDrawable, IHaveStatusEffect
{
    [SerializeField]
    private FurnitureType furnitureType;
    public FurnitureType FurnitureType { get => furnitureType; set => furnitureType = value; }
}
