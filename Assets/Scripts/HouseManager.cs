using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public static HouseManager instance;

    private static GameObject house;

    public static StoreItem Bed { get; set; }
    public static StoreItem Chair { get; set; }
    public static StoreItem Armchair { get; set; }
    public static StoreItem Table { get; set; }

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
    }

    private void Start()
    {
        Init();
    }

    public static void Init()
    {
        house = GameObject.Find("House");
        var z = 5;
    }

    public static void SetFurniture(StoreItem item)
    {
        switch (item.Type)
        {
            case ItemType.Bed:
                Bed = item;
                break;
            case ItemType.Chair:
                Chair = item;
                break;
            case ItemType.Armchair:
                Armchair = item;
                break;
            case ItemType.Table:
                Table = item;
                break;
            case ItemType.Other:
                break;
            default:
                break;
        }
    }

    public static void UpdateFlatAppearance()
    {
        foreach (Transform child in house.transform)
        {
            switch (child.name)
            {
                case "Bed":
                    child.GetComponent<SpriteRenderer>().sprite = HouseManager.Bed != null ? HouseManager.Bed.Sprite : null;
                    break;
                case "Chair":
                    child.GetComponent<SpriteRenderer>().sprite = HouseManager.Chair != null ? HouseManager.Chair.Sprite : null;
                    break;
                case "Armchair":
                    child.GetComponent<SpriteRenderer>().sprite = HouseManager.Armchair != null ? HouseManager.Armchair.Sprite : null;
                    break;
                case "Table":
                    child.GetComponent<SpriteRenderer>().sprite = HouseManager.Table != null ? HouseManager.Table.Sprite : null;
                    break;
                default:
                    break;
            }
        }
    }

}
