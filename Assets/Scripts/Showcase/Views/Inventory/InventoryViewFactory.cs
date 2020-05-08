using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryViewFactory : DefaultShowcaseViewFactory<Item, Inventory>
{
    new Inventory showcase;
    protected Object certificateListViewPrefab;
    protected Object certificateViewPrefab;
    protected Object storeItemListViewPrefab;
    protected Object storeItemViewPrefab;

    public InventoryViewFactory(Showcase<Item, Inventory> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object certificateListViewPrefab, Object itemViewPrefab, Object storeItemListViewPrefab, Object storeItemViewPrefab) 
        : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, null, null)
    {
        this.showcase = showcase as Inventory;
        this.certificateListViewPrefab = certificateListViewPrefab;
        this.storeItemListViewPrefab = storeItemListViewPrefab;
        this.storeItemViewPrefab = storeItemViewPrefab;
        certificateViewPrefab = Resources.Load("Prefabs/EducationHub/CertificateView");
    }

    public override View CreateRootView(Transform parentTransform)
    {
        Console.Print("___Start building inventory___");

        rootView = GameObject.Instantiate(rootViewPrefab as GameObject, parentTransform).GetComponent<DefaultRootView>();
        itemGroupListView = CreateItemGroupListView(rootView.transform);
        itemListView = CreateItemListView(rootView.transform);
        showcase.OnSelectedItemGroupChanged += delegate { UpdateItemListView(); };

        

        UpdateItemListView();
        rootView.UpdateView();
        return rootView;
    }

    public override void UpdateItemListView()
    {
        if (itemListView != null)
        {
            GameObject.Destroy(itemListView.gameObject);
        }
        if (showcase.SelectedItemGroup != null)
        {
            itemListView = CreateItemListView(rootView.transform);
        }
    }

    public override DefaultItemListView CreateItemListView(Transform parentTransform)
    {
        if (showcase.SelectedItemGroup.Title == "Certificates")
        {
            return CreateCertificateListView(parentTransform);
        } else if (showcase.SelectedItemGroup.Title == "Furniture")
        {
            return CreateStoreItemListView(parentTransform);
        }

        return null;
    }

    public DefaultItemListView CreateStoreItemListView(Transform parentTransform)
    {
        DefaultItemListView itemListView = GameObject.Instantiate(storeItemListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
        foreach (StoreItem item in showcase.SelectedItemGroup.Items)
        {
            CreateStoreItemView(item, itemListView.ScrollViewContentTransform);
        }
        itemListView.UpdateView();
        return itemListView;
    }

    public View CreateStoreItemView(StoreItem item, Transform parentTransform)
    {
        StoreItemView storeItemView = GameObject.Instantiate(storeItemViewPrefab as GameObject, parentTransform).GetComponent<StoreItemView>();

        storeItemView.Init(item);

        storeItemView.UpdateView();
        return storeItemView;
    }

    public DefaultItemListView CreateCertificateListView(Transform parentTransform)
    {
        DefaultItemListView certificateListView = GameObject.Instantiate(certificateListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
        if (showcase.SelectedItemGroup != null)
        {
            foreach (Item item in showcase.SelectedItemGroup.Items)
            {
                CreateCertificateView(item as Certificate, certificateListView.ScrollViewContentTransform);
            }
        }
        return certificateListView;
    }

    public View CreateCertificateView(Certificate certificate, Transform parentTransform)
    {
        CertificateView ceritifcateView = GameObject.Instantiate(certificateViewPrefab as GameObject, parentTransform).GetComponent<CertificateView>();

        ceritifcateView.Init(certificate);
        
        ceritifcateView.UpdateView();
        return ceritifcateView;
    }
    
}
