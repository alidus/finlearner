using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/// <summary>
/// Controls store logic
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    // Controllers, Managers
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private StatusEffectsController statusEffectsController;
    private HouseManager houseManager;
    private UIManager uiManager;

    // Events, delegates
    public delegate void StoreStateChangedAction();
    public event StoreStateChangedAction OnStoreStateChanged;


    private StoreItemDatabases storeItemDatabase;

    public Store Store { get; set; }
    public StoreItemDatabases StoreItemDatabases { get => storeItemDatabase; set => storeItemDatabase = value; }

    private StatusEffect dafaultStoreItemStatusEffect = new StatusEffect("Покупка в магазине", 20, StatusEffectType.Mood, StatusEffectFrequency.OneShot);
    

    private void Awake()
    {
        if (GameDataManager.instance.DEBUG)
            Debug.Log("ItemManager awake");
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

    public void UpdateReferences()
    {
        
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        statusEffectsController = StatusEffectsController.instance;
        houseManager = HouseManager.instance;
        uiManager = UIManager.instance;
    }

    public void InstantiateHomeStore(Transform storeContainerTransform)
    {
        StoreItemDatabases = ScriptableObject.Instantiate(Resources.Load("ScriptableObjects/Store/StoreItemDatabases")) as StoreItemDatabases;
        // TODO: Redundant, improve logic of singletons initialization to evade this crap
        uiManager = UIManager.instance;
        if (uiManager.storeContainer)
        {
            if (!storeContainerTransform.gameObject.GetComponent<Store>())
            {
                storeContainerTransform.gameObject.AddComponent<Store>();
            }
        }
    }
}
