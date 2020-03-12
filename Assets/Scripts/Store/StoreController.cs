using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    public static StoreController instance;

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
    private StoreCatalog activeCatalog;
    private ItemCategory selectedCategory;
    public ItemCategory SelectedCategory
    {
        get { return selectedCategory; }
        set { if (selectedCategory != value) { selectedCategory = value; OnStoreStateChanged(); } ;}
    }
    public StoreCatalog ActiveCatalog
    {
        get { return activeCatalog; }
        set { activeCatalog = value; }
    }
    

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

    public void StoreItemClick(StoreItem item)
    {
        if (!item.IsOwned)
        {
            if (item.Price <= gameDataManager.Money)
            {
                // Buy item
                item.IsOwned = true;
                gameDataManager.Money -= item.Price;
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
            if (item.EquipBehavour != null)
            {
                item.EquipBehavour.Equip();
                activeCatalog.EquipItem(item);
                houseManager.UpdateFlatAppearance();
                OnStoreStateChanged();
            }

        }

    }
}
