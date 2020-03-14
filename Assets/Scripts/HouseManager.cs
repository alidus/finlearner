﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseManager : MonoBehaviour
{
    public static HouseManager instance;

    private GameDataManager gameDataManager;

    private static GameObject house;

    public static StoreItem Bed { get; set; }
    public static StoreItem Chair { get; set; }
    public static StoreItem Armchair { get; set; }
    public static StoreItem Table { get; set; }

    public Dictionary<ItemType, StoreItem> EquippedFurniture { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        UpdateReferences();
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        gameDataManager = GameDataManager.instance;


    }


    public void UpdateReferences()
    {
        house = GameObject.Find("House");
    }

    public void EquipFurniture(StoreItem item)
    {
        if (item.Category == ItemCategory.Furniture)
        {
            if (EquippedFurniture.ContainsKey(item.Type))
            {
                EquippedFurniture[item.Type] = item;
            }
        } else
        {
            Debug.Log("Attempt to equip non-furniture category item from HouseManager.EquipFurniture");
        }
    }

    public void UpdateFlatAppearance()
    {
        foreach (Transform child in house.transform)
        {
            switch (child.name)
            {
                case "Bed":
                    child.GetComponent<Image>().sprite = HouseManager.Bed != null ? HouseManager.Bed.Sprite : gameDataManager.emptySprite;
                    break;
                case "Chair":
                    child.GetComponent<Image>().sprite = HouseManager.Chair != null ? HouseManager.Chair.Sprite : gameDataManager.emptySprite;
                    break;
                case "Armchair":
                    child.GetComponent<Image>().sprite = HouseManager.Armchair != null ? HouseManager.Armchair.Sprite : gameDataManager.emptySprite;
                    break;
                case "Table":
                    child.GetComponent<Image>().sprite = HouseManager.Table != null ? HouseManager.Table.Sprite : gameDataManager.emptySprite;
                    break;
                default:
                    break;
            }
        }
    }

}
