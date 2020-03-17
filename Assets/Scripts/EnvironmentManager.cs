using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager instance;

    private GameDataManager gameDataManager;

    private GameObject environment;

    private GameObject house;

    public HomeFurnitureSettings HomeFurnitureSettings { get; set; }


    public Dictionary<FurnitureType, Item> EquippedFurniture { get; set; }
    public GameObject Environment { get => environment; set => environment = value; }
    public GameObject House { get => house; set => house = value; }

    private void Awake()
    {
        if (GameDataManager.instance.DEBUG)
            Debug.Log("HouseManager awake");
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
        SceneManager.sceneLoaded += SceneLoadedHandling;
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        UpdateReferences();
    }

    public void UpdateReferences()
    {
        Environment = GameObject.Find("Environment");
        House = Environment?.transform.Find("House")?.gameObject;
    }

    public void EquipFurniture(FurnitureItem item)
    {
        if (EquippedFurniture.ContainsKey(item.FurnitureType))
        {
            EquippedFurniture[item.FurnitureType] = item;
        }
    }

    public void UpdateFlatAppearance()
    {
        // TODO: Singleton initialization check
        Init();
        if (Environment)
        {
            Environment.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        }
        if (House)
        {
            foreach (Transform child in House.transform)
            {
                switch (child.tag)
                {
                    case "Bed":
                        child.GetComponent<SpriteRenderer>().sprite = HomeFurnitureSettings.Bed?.Sprite ?? gameDataManager.emptySprite;
                        break;
                    case "Chair":
                        child.GetComponent<SpriteRenderer>().sprite = HomeFurnitureSettings.Chair?.Sprite ?? gameDataManager.emptySprite;
                        break;
                    case "Armchair":
                        child.GetComponent<SpriteRenderer>().sprite = HomeFurnitureSettings.Armchair?.Sprite ?? gameDataManager.emptySprite;
                        break;
                    case "Table":
                        child.GetComponent<SpriteRenderer>().sprite = HomeFurnitureSettings.Table?.Sprite ?? gameDataManager.emptySprite;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    

}

public struct HomeFurnitureSettings
{
    public static FurnitureItem Bed { get; set; }
    public static FurnitureItem Chair { get; set; }
    public static FurnitureItem Armchair { get; set; }
    public static FurnitureItem Table { get; set; }
}