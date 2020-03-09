using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Store : MonoBehaviour
{
    private GameManager gameManager;

    public StoreCatalog storeCatalog;

    public GameObject storeGridPanel;
    public GameObject storeCategoriesPanel;
    public GameObject storeItemPrefab;
    public GameObject categoryButtonPrefab;
    public GameObject storePanel;

    public delegate void StoreItemClickAction(StoreItem item);
    public event StoreItemClickAction OnInventoryItemClicked;



    private void Awake()
    {
        gameManager = GameManager.instance;
    }


    public void Show()
    {
        UpdateStoreView();
        storePanel.SetActive(true);
    }
    public void Hide()
    {
        storePanel.SetActive(false);
    }

    public void Toggle()
    {
        if (storePanel.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void UpdateStoreView()
    {
        foreach (Transform child in storeCategoriesPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<ItemCategory> presentedCategories = storeCatalog.GetCategories();

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
        foreach (Transform child in storeGridPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (StoreItem item in storeCatalog.GetAllItemsOfCategory(category))
        {
            GameObject itemObject = Instantiate(storeItemPrefab);
            itemObject.GetComponent<Button>().onClick.AddListener(delegate () { OnInventoryItemClicked(item); UpdateShowcase(category); });
            //itemObject.GetComponentInParent<Text>().text = item.name;
            itemObject.transform.SetParent(storeGridPanel.transform);
            itemObject.transform.localScale = new Vector3(1, 1, 1);
            itemObject.transform.Find("PriceTag").GetComponentInChildren<Text>().text = "$" + item.Price.ToString();
            itemObject.transform.Find("TitleText").GetComponent<Text>().text = item.Name;
            itemObject.transform.Find("Image").GetComponent<Image>().sprite = item.Sprite != null ? item.Sprite : gameManager.placeHolder;

            if (item.IsEquiped)
            {
                itemObject.transform.Find("EquipHighlightPanel").gameObject.SetActive(true);
            } else
            {
                itemObject.transform.Find("EquipHighlightPanel").gameObject.SetActive(false);
            }

            if (item.IsOwned)
            {
                itemObject.transform.Find("OwnIndicator").gameObject.SetActive(true);
            }
        }
    }

    void SelectStoreCategory(ItemCategory category)
    {
        UpdateShowcase(category);
    }
}
