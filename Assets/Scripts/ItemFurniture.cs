using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FurnitureType { Bed, Table, Chair, Armchair }


public class ItemFurniture : ItemObject
{
    public FurnitureType FurnitureType { get; set; }
}
