using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryViewFactory : DefaultShowcaseViewFactory<Item, Inventory>
{
    protected Object certificateListViewPrefab;
    protected Object certificateViewPrefab;

    public InventoryViewFactory(Showcase<Item, Inventory> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object certificateListViewPrefab, Object itemViewPrefab) 
        : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, null, null)
    {
        this.certificateListViewPrefab = certificateListViewPrefab;
        certificateViewPrefab = Resources.Load("Prefabs/EducationHub/CertificateView");
    }

    public override View CreateRootView(Transform parentTransform)
    {
        Console.Print("___Start building inventory___");

        rootView = GameObject.Instantiate(rootViewPrefab as GameObject, parentTransform).GetComponent<DefaultRootView>();
        itemGroupListView = CreateItemGroupListView(rootView.transform);
        itemListView = CreateItemListView(rootView.transform);
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
        }

        return null;
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
