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

    [SerializeField]
    private StoreCatalog storeCatalog;
    public StoreCatalog HomeStoreCatalog
    {
        get { return storeCatalog; }
        set { storeCatalog = value; }
    }
    private GameObject storeShowcasePanel;
    private GameObject storeCategoriesPanel;
    public GameObject storeItemPrefab;
    public GameObject categoryButtonPrefab;
    private GameObject storePanel;

    public delegate void StoreItemClickAction(StoreItem item);
    public event StoreItemClickAction OnInventoryItemClicked;

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
        storeShowcasePanel = GameObject.Find("StoreShowcasePanel");
        storeCategoriesPanel = GameObject.Find("StoreCategoriesPanel");
        storePanel = GameObject.Find("StorePanel");
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
    }

    public void UpdateStoreView()
    {
        foreach (Transform child in storeCategoriesPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<ItemCategory> presentedCategories = HomeStoreCatalog.GetCategories();

        foreach (ItemCategory category in presentedCategories)
        {
            GameObject categoryButtonObject = (GameObject)Instantiate(categoryButtonPrefab);

            RectTransform mRectTransform = categoryButtonObject.GetComponent<RectTransform>();
            categoryButtonObject.GetComponentInChildren<Text>().text = category.ToString();
            categoryButtonObject.transform.SetParent(storeCategoriesPanel.transform);
            mRectTransform.localScale = new Vector3(1, 1, 1);

            categoryButtonObject.GetComponent<Button>().onClick.AddListener(delegate () { SelectStoreCategory(category); });
        }

        if (presentedCategories.Count > 0)
        {
            SelectStoreCategory(presentedCategories[0]);
        }
    }

    void UpdateShowcase(ItemCategory category)
    {
        // Clear store item panels array
        foreach (Transform child in storeShowcasePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (StoreItem item in HomeStoreCatalog.GetAllItemsOfCategory(category))
        {
            GameObject itemObject = Instantiate(storeItemPrefab);
            itemObject.GetComponent<Button>().onClick.AddListener(delegate () { StoreItemClick(item); UpdateShowcase(category); });
            //itemObject.GetComponentInParent<Text>().text = item.name;
            itemObject.transform.SetParent(storeShowcasePanel.transform);
            itemObject.transform.localScale = new Vector3(1, 1, 1);
            Transform iconTransform = itemObject.transform.Find("Icon");
            iconTransform.transform.Find("PriceTag").GetComponentInChildren<Text>().text = "$" + item.Price.ToString();
            itemObject.transform.Find("TitleText").GetComponent<Text>().text = item.Name;
            iconTransform.GetComponent<Image>().sprite = item.Sprite != null ? item.Sprite : gameManager.placeHolder;

            if (item.IsEquiped)
            {
                itemObject.transform.Find("EquipHighlightPanel").gameObject.SetActive(true);
            } else
            {
                itemObject.transform.Find("EquipHighlightPanel").gameObject.SetActive(false);
            }

            if (item.IsOwned)
            {
                iconTransform.transform.Find("OwnIndicator").gameObject.SetActive(true);
            }
        }
    }

    void SelectStoreCategory(ItemCategory category)
    {
        UpdateShowcase(category);
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

            }
        }
        else
        {
            // Equip
            if (item.EquipBehavour != null)
            {
                item.EquipBehavour.Equip();
                storeCatalog.EquipItem(item);
                houseManager.UpdateFlatAppearance();
            }

        }

    }
}
