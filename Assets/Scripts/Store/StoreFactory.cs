using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class StoreFactory
{
    public static Store storeComponent;

    //public static Store CreateStore(Transform containerTransform)
    //{
    //    storeComponent = containerTransform.gameObject.AddComponent<Store>();
    //    storeComponent.ItemGroups = storeComponent.GetObjectItemCategories();
    //    storeComponent.SelectedItemGroup = storeComponent.ItemGroups[0];
    //    return storeComponent;
    //}

    public static StorePresenter CreateStorePresenter(Transform parentTransform)
    {
        StorePresenter storePresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StorePresenter") as GameObject, parentTransform).AddComponent<StorePresenter>(); ;
        return storePresenterComponent;
    }

    public static StoreItemPresenter CreateItemPresenter(ObjectItem item, Transform parentTransform)
    {
        StoreItemPresenter storeItemPresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StoreItemPresenter") as GameObject, parentTransform).AddComponent<StoreItemPresenter>(); ;
        storeItemPresenterComponent.Item = item;
        return storeItemPresenterComponent;
    }
    
    public static StoreItemCategoryButtonsPresenter CreateStoreItemCategoryButtonsPresenter(Transform parentTransform)
    {
        StoreItemCategoryButtonsPresenter storeItemCategoryButtonsPresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StoreItemCategoryButtonsPresenter") as GameObject, parentTransform).AddComponent<StoreItemCategoryButtonsPresenter>(); ;
        return storeItemCategoryButtonsPresenterComponent;
    }

    public static StoreItemListPresenter CreateStoreItemsListPresenter(Transform parentTransform)
    {
        StoreItemListPresenter storeItemsListPresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StoreItemsListPresenter") as GameObject, parentTransform).AddComponent<StoreItemListPresenter>();
        return storeItemsListPresenterComponent;
    }

    public static StoreItemCategoryButtonPresenter CreateStoreItemCategoryButtonPresenter(ItemGroup<ObjectItem> itemGroup, Transform parentTransform)
    {
        StoreItemCategoryButtonPresenter storeItemCategoryButtonPresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StoreItemCategoryButtonPresenter") as GameObject, parentTransform).AddComponent<StoreItemCategoryButtonPresenter>();
        storeItemCategoryButtonPresenterComponent.ItemGroup = itemGroup;
        return storeItemCategoryButtonPresenterComponent;
    }
}
