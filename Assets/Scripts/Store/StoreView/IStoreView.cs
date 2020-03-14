using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoreView
{
    IStoreCategoriesView StoreCategoriesView { get; set; }
    IStoreShowcaseView StoreShowcaseView { get; set; }

    GameObject StorePanel { get; set; }
    StoreCatalog StoreCatalog {get; set;}

    void Update();
    void Show();
    void Hide();

}
