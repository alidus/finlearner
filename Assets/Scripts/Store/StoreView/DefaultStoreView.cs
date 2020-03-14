using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStoreView : IStoreView
{
    public IStoreCategoriesView StoreCategoriesView { get; set; }
    public IStoreShowcaseView StoreShowcaseView {get; set;}
    public StoreCatalog StoreCatalog { get; set; }
    public GameObject StorePanel { get; set; }

    public DefaultStoreView(StoreCatalog storeCatalog, Transform parent)
    {
        StorePanel = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/DefaultStoreView") as GameObject, parent);
        StoreCategoriesView = new DefaultStoreCategoriesView(Resources.Load("Prefabs/Store/Views/DefaultStoreCategoriesView") as GameObject, Resources.Load("Prefabs/Store/Views/DefaultStoreCategoryView") as GameObject, StorePanel.transform);
        StoreShowcaseView = new DefaultStoreShowcaseView(Resources.Load("Prefabs/Store/Views/DefaultStoreShowcaseView") as GameObject, StorePanel.transform);
        StoreCatalog = storeCatalog;
    }    

    public void Update()
    {
        StoreCategoriesView.Update();
        StoreShowcaseView.Update();
    }

    public void Show()
    {
        StorePanel.SetActive(false);
    }

    public void Hide()
    {
        StorePanel.SetActive(true);
    }
 
}
