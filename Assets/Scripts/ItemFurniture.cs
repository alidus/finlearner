using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FurnitureType { Bed, Table, Chair, Armchair }

[System.Serializable]
public class FurnitureItem : ObjectItem
{
    [SerializeField]
    private FurnitureType furnitureType;
    public FurnitureType FurnitureType { get => furnitureType; set => furnitureType = value; }
}
