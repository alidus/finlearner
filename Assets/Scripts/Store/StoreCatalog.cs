//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreCatalogue", order = 1)]
//public class ItemDatabase : ScriptableObject
//{
//    public string title;
//    public List<Item> items = new List<Item>();

//    public ItemDatabase()
//    {

//    }

//    public ItemCategory SelectedCategory { get; set; }

//    public void Init()
//    {
//        AttachBehaviours();
//    }

//    public List<ItemCategory> GetPresentedCategories()
//    {
//        List<ItemCategory> result = new List<ItemCategory>();
//        foreach (Item item in items)
//        {
//            if (!result.Contains(item.Category))
//            {
//                result.Add(item.Category);
//            }
            
//        }
//        return result;
//    }

//    public List<ItemType> GetPresentedTypes()
//    {
//        List<ItemType> result = new List<ItemType>();
//        foreach (Item item in items)
//        {
//            if (!result.Contains(item.Type))
//            {
//                result.Add(item.Type);
//            }

//        }
//        return result;
//    }

//    public List<Item> GetAllItemsOfCategory(ItemCategory category)
//    {
//        List<Item> result = new List<Item>();
//        foreach (Item item in items)
//        {
//            if (item.Category == category)
//            {
//                result.Add(item);
//            }
//        }
//        return result;
//    }

//    public List<Item> GetAllItemsOfType(ItemType type)
//    {
//        List<Item> result = new List<Item>();
//        foreach (Item item in items)
//        {
//            if (item.Type == type)
//            {
//                result.Add(item);
//            }
//        }
//        return result;
//    }

//    public void EquipItem(Item storeItem)
//    {
//        foreach (Item item in items)
//        {
//            if (item.Type == storeItem.Type)
//            {
//                if (storeItem == item)
//                {
//                    item.IsEquiped = true;
//                } else
//                {
//                    item.IsEquiped = false;
//                }
//            }
//        }
//    }

//    private void AttachBehaviours()
//    {
//        foreach (Item item in items)
//        {
//            switch (item.Category)
//            {
//                case ItemCategory.Furniture:
//                    item.ClicBehavior = new FurnitureItemClickBehavour(item);
//                    break;
//                case ItemCategory.Clothes:
//                    break;
//                case ItemCategory.FreeEstate:
//                    break;
//                case ItemCategory.Car:
//                    break;
//                case ItemCategory.Misc:
//                    break;
//                default:
//                    break;
//            }
//        }
//    }
//}
