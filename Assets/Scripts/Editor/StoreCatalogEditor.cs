using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StoreCatalog))]
public class StoreCatalogEditor : Editor
{
    StoreCatalog catalog;
    SerializedProperty serializedItems;
    int listSize;

    bool canBePurchased;
    float price;

    bool canBeEquipped;

    Sprite sprite;

    private void OnEnable()
    {
        catalog = (StoreCatalog)target;
        serializedItems = serializedObject.FindProperty("items");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Items list:");
        listSize = serializedItems.arraySize;
        listSize = EditorGUILayout.IntField("List size", listSize);
        if (listSize != serializedItems.arraySize)
        {
            while (listSize > serializedItems.arraySize)
            {
                serializedItems.InsertArrayElementAtIndex(serializedItems.arraySize);
            }
            while (listSize < serializedItems.arraySize)
            {
                serializedItems.DeleteArrayElementAtIndex(serializedItems.arraySize - 1);
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Or");

        EditorGUILayout.LabelField("Add a new item with a button");
        if (GUILayout.Button("Add New"))
        {
            catalog.Items.Add(new StoreItem());
        }
        EditorGUILayout.Space();
        for (int i = 0; i < serializedItems.arraySize; i++)
        {
            Undo.RecordObject(target, "");
            StoreItem item = catalog.Items[i];
            SerializedProperty serializedItem = serializedItems.GetArrayElementAtIndex(i);

            item.Title = EditorGUILayout.TextField(item.Title);
            item.Desc = EditorGUILayout.TextField(item.Desc);
            item.Category = (ItemCategory)EditorGUILayout.EnumPopup(item.Category);
            item.Type = (ItemType)EditorGUILayout.EnumPopup(item.Type);
            // Status effects

            if (GUILayout.Button("Add New Status Effect"))
            {
                item.StatusEffects.Add(new StatusEffect("title", 50, StatusEffectType.Money, StatusEffectFrequency.OneShot));
            }
            foreach (StatusEffect se in item.StatusEffects)
            {
                EditorGUILayout.Separator();
                se.Title = EditorGUILayout.TextField(se.Title);
                se.Value = EditorGUILayout.FloatField(se.Value);
                se.Freqency = (StatusEffectFrequency)EditorGUILayout.EnumPopup(se.Freqency);
                se.Type = (StatusEffectType)EditorGUILayout.EnumPopup(se.Type);
                if (GUILayout.Button("Remove status effect (" + se.Title + ")"))
                {
                    item.StatusEffects.Remove(se);
                }
                EditorGUILayout.Separator();
            }

            // Purchase settings
            EditorGUI.BeginChangeCheck();
            var purchasableBehaviour = item.GetBehaviour<ItemPurchaseBehaviour>();
            canBePurchased = EditorGUILayout.Toggle("Can be purchased?", purchasableBehaviour != null);

            price = EditorGUILayout.FloatField("Value", purchasableBehaviour != null ? purchasableBehaviour.Price : 200);
            if (EditorGUI.EndChangeCheck())
            {
                if (canBePurchased)
                {
                    // If editor set CanBePurchased to true, then create IPurchasable behaviour if it is missing and set price
                    if (purchasableBehaviour == null)
                    {
                        switch (item.Category)
                        {
                            case ItemCategory.Furniture:
                                purchasableBehaviour = item.AddBehaviour<ItemPurchaseBehaviour>(new ItemPurchaseBehaviour());
                                purchasableBehaviour.Price = price;
                                break;
                            default:
                                break;
                        }
                    }
                    
                } else
                {
                    // Otherwise remove behaviour
                    item.RemoveBehaviour<ItemPurchaseBehaviour>();
                }
                
                
            }

            // Equip settings
            EditorGUI.BeginChangeCheck();
            var equippableBehaviour = item.GetBehaviour<FurnitureEquipBehaviour>();
            canBeEquipped = EditorGUILayout.Toggle("Can be equipped?", equippableBehaviour != null);
            if (EditorGUI.EndChangeCheck())
            {
                if (canBeEquipped)
                {
                    if (equippableBehaviour == null)
                    {
                        switch (item.Category)
                        {
                            case ItemCategory.Furniture:
                                item.AddBehaviour<FurnitureEquipBehaviour>(new FurnitureEquipBehaviour(item));
                                break;
                            default:
                                break;
                        }
                    }
                } else
                {
                    item.RemoveBehaviour<FurnitureEquipBehaviour>();
                }
                
            }
            EditorGUILayout.Space();
            sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), allowSceneObjects: false);
            EditorGUILayout.Space();
            if (GUILayout.Button("Remove this index ("+i.ToString()+")"))
            {
                catalog.Items.RemoveAt(i);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        int z = 5;
    }
}
