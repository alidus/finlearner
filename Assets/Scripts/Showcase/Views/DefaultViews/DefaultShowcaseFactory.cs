using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DefaultShowcaseViewFactory<T, TClass> where T : Item where TClass : Component
{
    protected Showcase<T, TClass> showcase;
    protected Object rootViewPrefab;
    protected Object itemGroupListViewPrefab;
    protected Object itemGroupViewPrefab;
    protected Object itemListViewPrefab;
    protected Object itemViewPrefab;

    protected View itemGroupListView;
    protected DefaultItemListView itemListView;
    protected View rootView;



    public DefaultShowcaseViewFactory(
        Showcase<T, TClass> showcase,
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
        rootView = GameObject.Instantiate(rootViewPrefab as GameObject, parentTransform).GetComponent<DefaultRootView>();
        itemGroupListView = CreateItemGroupListView(rootView.transform);
        itemListView = CreateItemListView(rootView.transform);
        showcase.OnSelectedItemGroupChanged -= UpdateItemListView;
        showcase.OnSelectedItemGroupChanged += UpdateItemListView;
        UpdateItemListView();
        rootView.UpdateView();
        return rootView;
    }

    public virtual View CreateItemGroupListView(Transform parentTransform)
    {
        Console.Print("_Building default item group list view");

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
        DefaultItemGroupView<T> storeItemGroupView = GameObject.Instantiate(itemGroupViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemGroupView<T>>();
        storeItemGroupView.OnClick.AddListener(delegate {
            showcase.SelectedItemGroup.OnCollectionModified -= UpdateItemListView;
            showcase.SelectedItemGroup = itemGroup;
            // Is case if selected item group was modified - update item list view
            showcase.SelectedItemGroup.OnCollectionModified += UpdateItemListView;
        });
        storeItemGroupView.Init(itemGroup);
        

        storeItemGroupView.UpdateView();
        return storeItemGroupView;
    }

    /// <summary>
    /// Destroy all presented item views and create item views for each item in selected group
    /// </summary>
    public virtual void UpdateItemListView()
    {
        Console.Print("_Updating default item list view");

        itemListView.DestroyItemViews();
        foreach (Item item in showcase.SelectedItemGroup.Items)
        {
            CreateItemView(item, itemListView.ScrollViewContentTransform);
        }
    }

    public virtual DefaultItemListView CreateItemListView(Transform parentTransform)
    {
        Console.Print("_Building default item list view");

        DefaultItemListView itemListView = GameObject.Instantiate(itemListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
        itemListView.UpdateView();
        return itemListView;
    }

    public virtual View CreateItemView(Item item, Transform parentTransform)
    {
        DefaultItemView itemView = GameObject.Instantiate(itemViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemView>();
        itemView.Init(item);
        itemView.UpdateView();
        return itemView;
    }
    
}
