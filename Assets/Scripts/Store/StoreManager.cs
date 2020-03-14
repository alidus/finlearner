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

    // Events, delegates
    public delegate void StoreStateChangedAction();
    public event StoreStateChangedAction OnStoreStateChanged;

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

        homeStoreCatalog = Resources.Load("ScriptableObjects/Store/HomeStoreCatalog") as StoreCatalog;
        homeStoreCatalog = Instantiate(homeStoreCatalog) as StoreCatalog;
        homeStoreCatalog.Init();
        HomeStoreCatalog = homeStoreCatalog;
    }

    

    public void StoreItemClick(StoreItem item)
    {
        if (!item.IsOwned)
        {
            if (item.Price <= gameDataManager.Money)
            {
                // Buy item
                item.IsOwned = true;
                gameDataManager.Money -= item.Price;
                // TODO: do it better way
                // If store item doesnt have modifier then apply default store item modifier
                if (item.Modifiers.Count == 0)
                {
                    item.Modifiers.Add(dafaultStoreItemStatusEffect);
                }
                foreach (StatusEffect statusEffect in item.Modifiers)
                {
                    if (statusEffect.Freqency == StatusEffectFrequency.OneShot)
                    {
                        statusEffectsController.ExecuteOneShotStatusEffect(statusEffect);
                    }
                    else
                    {
                        statusEffectsController.AddStatusEffects(statusEffect);
                    }
                }
                OnStoreStateChanged();
            }
        }
        else
        {
            // Equip
            if (item.ClicBehavior != null)
            {
                item.ClicBehavior.Equip();
                houseStoreCatalog.EquipItem(item);
                houseManager.UpdateFlatAppearance();
                OnStoreStateChanged();
            }

        }

    }
}
