using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DefaultShowcaseViewFactory<T> where T : Item
{
    protected Showcase<T> showcase;
    protected Object rootViewPrefab;
    protected Object itemGroupListViewPrefab;
    protected Object itemGroupViewPrefab;
    protected Object itemListViewPrefab;
    protected Object itemViewPrefab;

    public DefaultShowcaseViewFactory(
        Showcase<T> showcase,
        Object rootViewPrefab,
        Object itemGroupListViewPrefab,
        Object itemGroupViewPrefab,
        Object itemListViewPrefab,
        Object itemViewPrefab)
    {
        this.showcase = showcase;
        this.rootViewPrefab = rootViewPrefab;
        this.itemGroupListViewPrefab = itemGroupListViewPrefab;
        this.itemGroupViewPrefab = itemGroupViewPrefab;
        this.itemListViewPrefab = itemListViewPrefab;
        this.itemViewPrefab = itemViewPrefab;
    }

    public virtual View CreateRootView(Transform parentTransform)
    {
        DefaultRootView storeView = GameObject.Instantiate(rootViewPrefab as GameObject, parentTransform).GetComponent<DefaultRootView>();
        storeView.Init();
        storeView.ItemGroupListView = CreateItemGroupListView(storeView.transform);
        storeView.ItemListView = CreateItemListView(storeView.transform);
        storeView.UpdateView();
        return storeView;
    }

    public virtual View CreateItemGroupListView(Transform parentTransform)
    {
        DefaultItemGroupListView storeItemGroupListView = GameObject.Instantiate(itemGroupListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemGroupListView>();
        storeItemGroupListView.Init();
        foreach (ItemGroup<T> itemGroup in showcase.ItemGroups)
        {
            CreateItemGroupView(itemGroup, storeItemGroupListView.transform);
        }
        storeItemGroupListView.UpdateView();
        return storeItemGroupListView;
    }

    public virtual View CreateItemGroupView(ItemGroup<T> itemGroup, Transform parentTransform)
    {
        DefaultItemGroupView storeItemGroupView = GameObject.Instantiate(itemGroupViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemGroupView>();
        storeItemGroupView.OnClick.AddListener(delegate { showcase.SelectedItemGroup = itemGroup; });
        storeItemGroupView.Init(itemGroup.Title);
        storeItemGroupView.UpdateView();
        return storeItemGroupView;
    }
    public virtual View CreateItemListView(Transform parentTransform)
    {
        DefaultItemListView itemListView = GameObject.Instantiate(itemListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
        itemListView.Init();
        foreach(Item item in showcase.SelectedItemGroup.Items)
        {
            CreateItemView(item, itemListView.ScrollViewContentTransform);
        }
        return itemListView;
    }

    public virtual View CreateItemView(Item item, Transform parentTransform)
    {
        DefaultItemView storeItemView = GameObject.Instantiate(itemViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemView>();
        
        storeItemView.Title = item.Title;
        storeItemView.UpdateView();
        return storeItemView;
    }
    
}
