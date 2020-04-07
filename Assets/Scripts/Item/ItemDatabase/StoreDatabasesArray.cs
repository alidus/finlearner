//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "StoreItemDatabasesArray", menuName = "SO/Items/StoreItemDatabasesArray")]
//[System.Serializable]
//public class StoreDatabasesArray : ScriptableObject
//{
//    [SerializeField]
//    private FurnitureItemDatabase furniture;

//    public FurnitureItemDatabase Furniture { get => furniture; set => furniture = value; }

//    public ItemDatabase<StoreItem> GetAllObjectItemsDatabase() {
//        return new ItemDatabase<StoreItem>(furniture.Items);
//    }

//}
