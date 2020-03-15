using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStoreCategoriesView : IStoreCategoriesView
{
    StoreManager storeManager;

    public StoreCatalog StoreCatalog { get; private set; }
    public GameObject StoreCategoriesPanel { get; set; }

    public DefaultStoreCategoriesView(IStoreView parentView)
    {
        StoreCatalog = parentView.StoreCatalog;
        storeManager = StoreManager.instance;
        StoreCategoriesPanel = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/DefaultStoreCategoriesView") as GameObject, parentView.StorePanel.transform);
    }

    public void Update() {
        ClearCategoriesPanel();

        List<ItemCategory> presentedCategories = StoreCatalog.GetPresentedCategories();

        foreach (ItemCategory category in presentedCategories)
        {
            DefaultStoreCategoryView storeCategoryView = new DefaultStoreCategoryView(StoreCategoriesPanel);
            storeCategoryView.Title = category.ToString();
            storeCategoryView.OnClick.AddListener(delegate () { StoreCatalog.SelectedCategory = category; });
        }
    }

    private void ClearCategoriesPanel()
    {
        foreach (Transform child in StoreCategoriesPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
