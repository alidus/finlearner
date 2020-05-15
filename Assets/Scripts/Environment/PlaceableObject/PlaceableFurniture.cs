using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableFurniture : PlaceableObject
{
    public FurnitureType FurnitureType;

    private void OnMouseDown()
    {
        if (radialSelector == null)
        {
            radialSelector = radialSelectorSpawner.SpawnRadialSelector(this);
        }
        radialSelector.UpdatePlaceableList(inventory.GetItems<FurnitureItem>().FindAll(item => item.FurnitureType == FurnitureType).ConvertAll(item => (IPlaceable)item));
        radialSelector.UpdateView();
    }
}
