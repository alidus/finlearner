using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedEquipBehavour : IEquipable
{
    StoreItem bed;

    public BedEquipBehavour(StoreItem bed)
    {
        this.bed = bed;
    }
    public void Equip() {
        FlatSettings.bed = this.bed;
        UnityEngine.Debug.Log("Equipped a bed");
    }
}
