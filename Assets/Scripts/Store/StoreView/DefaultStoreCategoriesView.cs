using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStoreCategoriesView : IStoreCategoriesView
{
    StoreManager storeManager;

    public StoreCatalog StoreCatalog { get; set; }
    public GameObject StoreCategoriesPanel { get; set; }
    public GameObject StoreCategoryPanelPrefab { get; set; }

    public DefaultStoreCategoriesView(GameObject storeCategoriesPanelPrefab, GameObject storeCategoryPanelPrefab, Transform parent)
    {
        storeManager = StoreManager.instance;
        StoreCategoriesPanel = GameObject.Instantiate(storeCategoriesPanelPrefab, parent);
        StoreCategoryPanelPrefab = storeCategoryPanelPrefab;
    }

    public void Update() {
        ClearCategoriesPanel();

        List<ItemCategory> presentedCategories = StoreCatalog.GetPresentedCategories();

        foreach (ItemCategory category in presentedCategories)
        {
            DefaultStoreCategoryView storeCategoryView = new DefaultStoreCategoryView(StoreCategoryPanelPrefab, StoreCategoriesPanel.transform);
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
