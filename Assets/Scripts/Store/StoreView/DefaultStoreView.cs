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
        StoreCatalog = storeCatalog;
        StoreCategoriesView = new DefaultStoreCategoriesView(this);
        StoreShowcaseView = new DefaultStoreShowcaseView(this);
        
    }    

    public void Update()
    {
        StoreCategoriesView.Update();
        StoreShowcaseView.Update();
    }
}
