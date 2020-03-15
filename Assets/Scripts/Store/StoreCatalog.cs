// //using System.Collections;
// //using System.Collections.Generic;
// //using UnityEngine;

// <<<<<<< HEAD
// [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreCatalogue", order = 1)]
// public class StoreCatalog : ScriptableObject
// {
//     [SerializeField]
//     private List<StoreItem> items = new List<StoreItem>();
//     public List<StoreItem> Items
//     {
//         get { return items; }
//         set { items = value; }
//     }
//     public StoreCatalog()
//     {
// =======
// //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreCatalogue", order = 1)]
// //public class ItemDatabase : ScriptableObject
// //{
// //    public string title;
// //    public List<Item> items = new List<Item>();

// //    public ItemDatabase()
// //    {
// >>>>>>> origin/alidus

// //    }

// //    public ItemCategory SelectedCategory { get; set; }

// <<<<<<< HEAD
//     public void Init()
//     {

//     }

//     public List<ItemCategory> GetPresentedCategories()
//     {
//         List<ItemCategory> result = new List<ItemCategory>();
//         foreach (StoreItem item in Items)
//         {
//             if (!result.Contains(item.Category))
//             {
//                 result.Add(item.Category);
//             }
// =======
// //    public void Init()
// //    {
// //        AttachBehaviours();
// //    }

// //    public List<ItemCategory> GetPresentedCategories()
// //    {
// //        List<ItemCategory> result = new List<ItemCategory>();
// //        foreach (Item item in items)
// //        {
// //            if (!result.Contains(item.Category))
// //            {
// //                result.Add(item.Category);
// //            }
// >>>>>>> origin/alidus
            
// //        }
// //        return result;
// //    }

// <<<<<<< HEAD
//     public List<ItemType> GetPresentedTypes()
//     {
//         List<ItemType> result = new List<ItemType>();
//         foreach (StoreItem item in Items)
//         {
//             if (!result.Contains(item.Type))
//             {
//                 result.Add(item.Type);
//             }
// =======
// //    public List<ItemType> GetPresentedTypes()
// //    {
// //        List<ItemType> result = new List<ItemType>();
// //        foreach (Item item in items)
// //        {
// //            if (!result.Contains(item.Type))
// //            {
// //                result.Add(item.Type);
// //            }
// >>>>>>> origin/alidus

// //        }
// //        return result;
// //    }

// <<<<<<< HEAD
//     public List<StoreItem> GetAllItemsOfCategory(ItemCategory category)
//     {
//         List<StoreItem> result = new List<StoreItem>();
//         foreach (StoreItem item in Items)
//         {
//             if (item.Category == category)
//             {
//                 result.Add(item);
//             }
//         }
//         return result;
//     }

//     public List<StoreItem> GetAllItemsOfType(ItemType type)
//     {
//         List<StoreItem> result = new List<StoreItem>();
//         foreach (StoreItem item in Items)
//         {
//             if (item.Type == type)
//             {
//                 result.Add(item);
//             }
//         }
//         return result;
//     }
// }
// =======
// //    public List<Item> GetAllItemsOfCategory(ItemCategory category)
// //    {
// //        List<Item> result = new List<Item>();
// //        foreach (Item item in items)
// //        {
// //            if (item.Category == category)
// //            {
// //                result.Add(item);
// //            }
// //        }
// //        return result;
// //    }

// //    public List<Item> GetAllItemsOfType(ItemType type)
// //    {
// //        List<Item> result = new List<Item>();
// //        foreach (Item item in items)
// //        {
// //            if (item.Type == type)
// //            {
// //                result.Add(item);
// //            }
// //        }
// //        return result;
// //    }

// //    public void EquipItem(Item storeItem)
// //    {
// //        foreach (Item item in items)
// //        {
// //            if (item.Type == storeItem.Type)
// //            {
// //                if (storeItem == item)
// //                {
// //                    item.IsEquiped = true;
// //                } else
// //                {
// //                    item.IsEquiped = false;
// //                }
// //            }
// //        }
// //    }

// //    private void AttachBehaviours()
// //    {
// //        foreach (Item item in items)
// //        {
// //            switch (item.Category)
// //            {
// //                case ItemCategory.Furniture:
// //                    item.ClicBehavior = new FurnitureItemClickBehavour(item);
// //                    break;
// //                case ItemCategory.Clothes:
// //                    break;
// //                case ItemCategory.FreeEstate:
// //                    break;
// //                case ItemCategory.Car:
// //                    break;
// //                case ItemCategory.Misc:
// //                    break;
// //                default:
// //                    break;
// //            }
// //        }
// //    }
// //}
// >>>>>>> origin/alidus
