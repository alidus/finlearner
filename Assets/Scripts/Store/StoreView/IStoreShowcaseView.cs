using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoreShowcaseView
{
    StoreCatalog StoreCatalog { get; set; }
    GameObject StoreShowcasePanel { get; set; } 
    GameObject StoreItemPanelPrefab { get; set; }
    void Update();
}
