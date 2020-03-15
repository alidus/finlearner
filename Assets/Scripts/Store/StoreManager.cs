using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/// <summary>
/// Controls store logic
/// </summary>
public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    // Controllers, Managers
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private StatusEffectsController statusEffectsController;
    private HouseManager houseManager;
    private UIManager uiManager;

    public StoreCatalog ActiveStoreCatalog { get; set; }


    [SerializeField]
    private StoreCatalog houseStoreCatalog;
    
    public StoreCatalog HouseStoreCatalog
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


    }

    public void InitHomeStore()
    {
        HouseStoreCatalog = Instantiate(Resources.Load("ScriptableObjects/Store/HomeStoreCatalog")) as StoreCatalog;
        HouseStoreCatalog.Init();
        ActiveStoreCatalog = HouseStoreCatalog;
    }
}
