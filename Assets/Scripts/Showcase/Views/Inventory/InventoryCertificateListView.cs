using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryCertificateListView : DefaultItemListView
{
    List<CertificateRowView> certificateRowViews = new List<CertificateRowView>();
    EducationHub educationHub;
    int certificateRowsCount = 4;
    event Action OnDestroyEvent;
    Transform certificateRowsParentTransform;
    VerticalLayoutGroup scrollViewContentLayout;


    private void OnEnable()
    {
        Console.Print("Enabling " + this.ToString());
        certificateRowsParentTransform = transform.Find("ScrollView").Find("Viewport").Find("Content");
        scrollViewContentLayout = certificateRowsParentTransform.GetComponent<VerticalLayoutGroup>();
        SetCertificateRowsCount(certificateRowsCount);
        certificateRowViews = GetComponentsInChildren<CertificateRowView>().ToList();
        Console.Print("Certificate rows count: " + certificateRowViews.Count);
    }

    public void SetCertificateRowsCount(int count)
    {
        Console.Print("Setting new certificate rows count in Inventory to: " + count.ToString());
        certificateRowsCount = count;
        certificateRowViews.Clear();
        DestroyCertificateRows();
        for (int i = 0; i < count; i++)
        {
            certificateRowViews.Add(Instantiate<GameObject>(Resources.Load("Prefabs/Inventory/Views/CertificateRowView") as GameObject, certificateRowsParentTransform).GetComponent<CertificateRowView>());
        }
        UpdateView();
    }

    private void Start()
    {
        educationHub = (EducationHub.instance as EducationHub);
        educationHub.Certificates.CollectionChanged -= CertificatesCollectionChangedHandler;
        educationHub.Certificates.CollectionChanged += CertificatesCollectionChangedHandler;
        OnDestroyEvent += delegate { educationHub.Certificates.CollectionChanged -= CertificatesCollectionChangedHandler; };
        UpdateView();
    }

    private void CertificatesCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateView();
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }


    public void AddCertificate(Certificate certificate)
    {
        Console.Print("Adding certificate to Certificate list view in Inventory");
        var properRow = GetProperRow();
        if (properRow == null)
        {
            Console.Print("Could not find proper row to add certificate");
        } else
        {
            Console.Print("Adding certificate to row: " + properRow.ToString());
        }
        CertificateView certificateView = Instantiate<GameObject>(Resources.Load("Prefabs/Education/CertificateView") as GameObject, properRow.transform).GetComponent<CertificateView>();

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollViewContentLayout.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();

        certificateView.Init(certificate);
    }   

    public void ClearCertificateRows()
    {
        foreach (CertificateRowView certificateRowView in certificateRowViews)
        {
            certificateRowView.DestroyChildren();
            // Destroy will be executed next frame so detach children to calculate right child count this frame
            certificateRowView.transform.DetachChildren();
        }
    }

    CertificateRowView GetProperRow()
    {
        if (certificateRowViews.Count > 0)
        {
            if (certificateRowViews.Count == 1 || certificateRowViews.Last().Count == certificateRowViews[0].Count)
            {
                return certificateRowViews[0];
            }
            for (int i = 0; i < certificateRowViews.Count - 1; i++)
            {
                if (certificateRowViews[i].Count > certificateRowViews[i + 1].Count)
                {
                    return certificateRowViews[i + 1];
                }
            }

            return null;
        } else
        {
            return null;
        }
        
    }

    public override void UpdateView()
    {
        ClearCertificateRows();
        if (educationHub != null)
        {
            foreach (Certificate certificate in educationHub.Certificates)
            {
                AddCertificate(certificate);
            }
        }
    }

    void DestroyCertificateRows()
    {
        
        for (int i = certificateRowsParentTransform.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(certificateRowsParentTransform.GetChild(i).gameObject);
        }
    }
}
