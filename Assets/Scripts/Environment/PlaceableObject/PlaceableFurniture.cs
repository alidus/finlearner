using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableFurniture : PlaceableObject
{
    public FurnitureType FurnitureType;

    protected override void UpdateSelectorList()
    {
        radialSelector.UpdatePlaceableList(inventory.GetItems<FurnitureItem>().FindAll(item => item.FurnitureType == FurnitureType).ConvertAll(item => (IPlaceable)item));
    }
}
