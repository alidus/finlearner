using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreItemDatabases", menuName = "SO/Items/StoreItemDatabase")]
[System.Serializable]
public class StoreItemDatabases : ScriptableObject
{
    [SerializeField]
    private FurnitureItemDatabase furniture;

    public FurnitureItemDatabase Furniture { get => furniture; set => furniture = value; }

    public ItemDatabase<ObjectItem> GetAllObjectItemsDatabase() {
        return new ItemDatabase<ObjectItem>(furniture.Items);
    }

}
