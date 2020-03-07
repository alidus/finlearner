using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Factory class which creates store components
/// </summary>
public class StoreComponentFactory : MonoBehaviour
{
    /// <summary>
    /// Creates new store component and attach it to a target object
    /// </summary>
    /// <param name="target">Target object to attach store component to</param>
    /// <param name="storeCatalog"></param>
    /// <param name="storePanel"></param>
    /// <param name="storeItemPrefab"></param>
    /// <param name="categoryButtonPrefab"></param>
    /// <param name="storeCategoriesPanel"></param>
    /// <param name="storeGridPanel"></param>
    /// <returns></returns>
    public static Store CreateStoreComponent(GameObject target, StoreCatalog storeCatalog, GameObject storePanel, GameObject storeItemPrefab, GameObject categoryButtonPrefab, GameObject storeCategoriesPanel, GameObject storeGridPanel)
    {
        Store store = target.AddComponent<Store>();

        store.storeCatalog = storeCatalog;
        store.storePanel = storePanel;
        store.storeItemPrefab = storeItemPrefab;
        store.categoryButtonPrefab = categoryButtonPrefab;
        store.storeCategoriesPanel = storeCategoriesPanel;
        store.storeGridPanel = storeGridPanel;

        return store;
    }

}
