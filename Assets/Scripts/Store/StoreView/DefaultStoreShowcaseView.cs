using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStoreShowcaseView : IStoreShowcaseView
{
    public StoreCatalog StoreCatalog { get; set; }
    public GameObject StoreShowcasePanel { get; set; }

    public GameObject StoreItemPanelPrefab { get; set; }

    public DefaultStoreShowcaseView(IStoreView parentView)
    {
        StoreCatalog = parentView.StoreCatalog;
        StoreShowcasePanel = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/DefaultStoreShowcaseView") as GameObject, parentView.StorePanel.transform);
    }

    public void Update() {
        // TODO: implement empty items array behavior
        // Clear store item panels array
        foreach (Transform child in StoreShowcasePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (StoreItem storeItem in StoreCatalog.GetAllItemsOfCategory(StoreCatalog.SelectedCategory))
        {
            DefaultStoreItemView itemView = new DefaultStoreItemView(storeItem, StoreShowcasePanel.transform);
            itemView.OnClick.AddListener(delegate () { storeItem.Click(); });
        }
    }
}
