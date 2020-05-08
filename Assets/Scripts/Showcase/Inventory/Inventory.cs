using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InventoryItemType { Certificate, Furniture }

/// <summary>
/// Showcase that displays StoreObject items
/// </summary>
public class Inventory : Showcase<Item, Inventory>
{
    InventoryViewFactory factory;
    Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        // Load store database array containing different StoreItem databases of various item types (like clothing, furniture, etc)
        ItemDatabase.Clear();
        ItemDatabase = LoadAssets();

        // Setup groups
        ItemGroups = FormItemGroups();
        if (ItemGroups.Count > 0)
            SelectedItemGroup = ItemGroups[0];

        if (factory == null)
        {
            factory = new InventoryViewFactory(
                this,
                Resources.Load("Prefabs/Inventory/Views/InventoryView"),
                Resources.Load("Prefabs/Inventory/Views/InventoryItemGroupListView"),
                Resources.Load("Prefabs/Inventory/Views/InventoryItemGroupView"),
                Resources.Load("Prefabs/Inventory/Views/InventoryCertificateListView"),
                Resources.Load("Prefabs/Inventory/Views/InventoryItemView"));
        }

        DestroyViews();

        RootView = factory.CreateRootView(this.transform);
        
    }

    protected override ItemDatabase<Item> LoadAssets()
    {
        ItemDatabase<Item> result = new ItemDatabase<Item>();
        Console.Print("Loading inventory items...");
        
        Console.Print("Inventory catalog loaded");
        return result;
    }


    public override void UpdateShowcase()
    {
        if (RootView != null)
        {
            RootView.UpdateView();
        }
    }

    protected List<ItemGroup<Item>> FormItemGroups()
    {
        List<ItemGroup<Item>> result = new List<ItemGroup<Item>>();
        result.Add(new ItemGroup<Item>("Certificates"));
        result.Add(new ItemGroup<Item>("Furniture"));
        return result;
    }

    public override void Toggle()
    {
        if (animator)
        {
            animator.SetBool("IsOpened", !animator.GetBool("IsOpened"));
        }
    }

    public override void Show()
    {
        if (animator)
        {
            animator.SetBool("IsOpened", true);
        }
    }

    public override void Hide()
    {
        if (animator)
        {
            animator.SetBool("IsOpened", false);
        }
    }

    public void AddItem(Item item)
    {
        ItemDatabase.Add(item);

        if (item is Certificate)
        {
            var certItemGroup = FindItemGroup("Certificates");
            if (certItemGroup == null)
            {
                certItemGroup = new ItemGroup<Item>("Certificates");
            }
            certItemGroup.Add(item);
        }
    }

    public bool RemoveItem(Item item)
    {

        if (item is Certificate)
        {
            var certItemGroup = FindItemGroup("Certificates");
            if (certItemGroup == null)
            {
                return false;
            }
            if (certItemGroup.Contains(item))
            {
                certItemGroup.Remove(item);
                ItemDatabase.Remove(item);
                return true;
            } else
            {
                return false;
            }
        }

        return false;
    }
}

