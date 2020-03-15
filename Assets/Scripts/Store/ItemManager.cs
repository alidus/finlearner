using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/// <summary>
/// Controls store logic
/// </summary>
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    // Controllers, Managers
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private StatusEffectsController statusEffectsController;
    private HouseManager houseManager;
    private UIManager uiManager;

    // Events, delegates
    public delegate void StoreStateChangedAction();
    public event StoreStateChangedAction OnStoreStateChanged;

    [SerializeField]
    private ItemDatabase<ItemObject> houseStoreCatalog;

    public ItemDatabase<ItemObject> HouseStoreCatalog
    {
        get { return houseStoreCatalog; }
        set { houseStoreCatalog = value; }
    }

    private StatusEffect dafaultStoreItemStatusEffect = new StatusEffect("Покупка в магазине", 20, StatusEffectType.Mood, StatusEffectFrequency.OneShot);
    

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

        HouseStoreCatalog = ScriptableObject.Instantiate(Resources.Load("ScriptableObjects/Store/HomeStoreCatalog")) as ItemDatabase<ItemObject>;
        StoreFactory.CreateStore(uiManager.storePanel.transform);
    }
}
