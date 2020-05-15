using UnityEngine;
using UnityEditor;

public class PlaceableCar : PlaceableObject
{

    protected override void UpdateSelectorList()
    {
        radialSelector.UpdatePlaceableList(inventory.GetItems<CarItem>().ConvertAll(item => (IPlaceable)item));
    }
}