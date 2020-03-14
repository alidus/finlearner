using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStoreShowcaseView : IStoreShowcaseView
{
    public StoreCatalog StoreCatalog { get; set; }
    public GameObject StoreShowcasePanel { get; set; }

    public GameObject StoreItemPanelPrefab { get; set; }

    public DefaultStoreShowcaseView(GameObject storeShowcasePanelPrefab, Transform parent)
    {
        StoreShowcasePanel = GameObject.Instantiate(storeShowcasePanelPrefab, parent);
    }

    public void Update() {
        // TODO: implement empty items array behavior
        // Clear store item panels array
        foreach (Transform child in StoreShowcasePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (StoreItem item in StoreCatalog.GetAllItemsOfCategory(StoreCatalog.SelectedCategory))
        {
            DefaultStoreItemView itemView = new DefaultStoreItemView(StoreItemPanelPrefab, StoreShowcasePanel.transform);
            itemView.OnClick.AddListener(delegate () { storeManager.StoreItemClick(item); });

            //itemObject.GetComponentInParent<Text>().text = item.name;
            storeItemPanel.transform.SetParent(storeItemsShowcasePanel.transform);
            storeItemPanel.transform.localScale = new Vector3(1, 1, 1);
            Transform iconTransform = storeItemPanel.transform.Find("Icon");
            iconTransform.transform.Find("PriceTag").GetComponentInChildren<Text>().text = "$" + item.Price.ToString();
            storeItemPanel.transform.Find("TitleText").GetComponent<Text>().text = item.Name;
            iconTransform.GetComponent<Image>().sprite = item.Sprite != null ? item.Sprite : gameDataManager.placeHolderSprite;

            if (item.IsEquiped)
            {
                storeItemPanel.transform.Find("EquipHighlightPanel").gameObject.SetActive(true);
            }
            else
            {
                storeItemPanel.transform.Find("EquipHighlightPanel").gameObject.SetActive(false);
            }

            if (item.IsOwned)
            {
                iconTransform.transform.Find("OwnIndicator").gameObject.SetActive(true);
            }
        }
    }
}
