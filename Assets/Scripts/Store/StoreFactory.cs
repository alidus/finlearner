using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class StoreFactory
{
    public static Store CreateStore(Transform containerTransform)
    {
        return containerTransform.gameObject.AddComponent<Store>();
    }

    public static StoreItemPresenter CreateItemPresenter(Transform parentTransform)
    {
        StoreItemPresenter storeItemPresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StoreItemPresenter") as GameObject, parentTransform).AddComponent<StoreItemPresenter>(); ;
        return storeItemPresenterComponent;
    }

    public static StorePresenter CreateStorePresenter(Transform parentTransform)
    {
        StorePresenter storePresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StorePresenter") as GameObject, parentTransform).AddComponent<StorePresenter>(); ;
        return storePresenterComponent;
    }
    public static StoreItemCategoryButtonsPresenter CreateStoreItemCategoryButtonsPresenter(Transform parentTransform)
    {
        StoreItemCategoryButtonsPresenter storeItemCategoryButtonsPresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StoreItemCategoryButtonsPresenter") as GameObject, parentTransform).AddComponent<StoreItemCategoryButtonsPresenter>(); ;
        return storeItemCategoryButtonsPresenterComponent;
    }

    public static StoreItemsListPresenter CreateStoreItemsListPresenter(Transform parentTransform)
    {
        StoreItemsListPresenter storeItemsListPresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StoreItemsListPresenter") as GameObject, parentTransform).AddComponent<StoreItemsListPresenter>(); ;
        return storeItemsListPresenterComponent;
    }

    public static StoreItemCategoryButtonPresenter CreateStoreItemCategoryButtonPresenter(Transform parentTransform)
    {
        StoreItemCategoryButtonPresenter storeItemCategoryButtonPresenterComponent = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/StoreItemCategoryButtonPresenter") as GameObject, parentTransform).AddComponent<StoreItemCategoryButtonPresenter>(); ;
        return storeItemCategoryButtonPresenterComponent;
    }
}
