using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreViewFactory : DefaultShowcaseViewFactory<StoreItem, Store>
{
    public StoreViewFactory(Showcase<StoreItem, Store> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object itemListViewPrefab, Object itemViewPrefab) 
        : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, itemListViewPrefab, itemViewPrefab)
    {
    }

    public override View CreateRootView(Transform parentTransform)
    {
        Console.Print("___Start building store___");

        return base.CreateRootView(parentTransform);
    }

    public override DefaultItemListView CreateItemListView(Transform parentTransform)
    {
        DefaultItemListView itemListView = GameObject.Instantiate(itemListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
        foreach (StoreItem item in showcase.SelectedItemGroup.Items)
        {
            CreateItemView(item, itemListView.ScrollViewContentTransform);
        }
        itemListView.UpdateView();
        return itemListView;
    }

    public View CreateItemView(StoreItem item, Transform parentTransform)
    {
        StoreItemView storeItemView = GameObject.Instantiate(itemViewPrefab as GameObject, parentTransform).GetComponent<StoreItemView>();

        storeItemView.Init(item);
        
        storeItemView.UpdateView();
        return storeItemView;
    }
    
}
