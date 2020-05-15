using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Showcase that displays StoreObject items
/// </summary>
public class Store : Showcase<StoreItem, Store>
{
    StoreViewFactory factory;
    Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        // Load store database array containing different StoreItem databases of various item types (like clothing, furniture, etc)
        ItemDatabase.Clear();
        ItemDatabase = LoadAssets();

        // Setup groups
        if (ItemDatabase.Count > 0)
        {
            ItemGroups = FormItemGroups();
            if (ItemGroups.Count > 0)
                SelectedItemGroup = ItemGroups[0];
        }

        if (factory == null)
        {
            factory = new StoreViewFactory(
                this,
                Resources.Load("Prefabs/Store/Views/StoreView"),
                Resources.Load("Prefabs/Store/Views/StoreItemGroupListView"),
                Resources.Load("Prefabs/Store/Views/StoreItemGroupView"),
                Resources.Load("Prefabs/Store/Views/StoreItemListView"),
                Resources.Load("Prefabs/Store/Views/StoreItemView"));
        }

        DestroyViews();

        RootView = factory.CreateRootView(this.transform);
        
    }

    protected override ItemDatabase<StoreItem> LoadAssets()
    {
        Console.Print("Loading store catalog...");
        ItemDatabase<StoreItem> result = new ItemDatabase<StoreItem>();
        foreach (StoreItem storeItem in Resources.LoadAll("ScriptableObjects/Store/Catalog").ToList().ConvertAll(x => (StoreItem)x))
        {
            var storeItemInstance = ScriptableObject.Instantiate(storeItem) as StoreItem;
            storeItemInstance.OnPurchaseStateChanged += delegate { storeItem.NotifyOnInstancePurchaseStateChanged(storeItemInstance); };
            storeItemInstance.OnPurchasableStateChanged += delegate { storeItem.NotifyOnInstancePurchasableStateChanged(storeItemInstance); };

            storeItemInstance.OnEquipStateChanged += delegate { storeItem.NotifyOnInstanceEquipStateChanged(storeItemInstance); };
            storeItemInstance.OnEquippableStateChanged += delegate { storeItem.NotifyOnInstanceEquippableStateChanged(storeItemInstance); };

            // TODO: trigger onBuy of scriptable object when buying instance of it (for inspector-added conditions)
            result.Add(storeItemInstance);
            Console.Print("Added store item: " + storeItemInstance.Title);
        }
        Console.Print("Store catalog loaded");
        return result;
    }


    public void SelectItemGroup(ItemGroup<StoreItem> itemGroup)
    {
        SelectedItemGroup = ItemGroups.Find(group => itemGroup == group) ?? SelectedItemGroup;
    }

    public override void UpdateShowcase()
    {
        if (RootView != null)
        {
            RootView.UpdateView();
        }
    }

    protected List<ItemGroup<StoreItem>> FormItemGroups()
    {
        List<ItemGroup<StoreItem>> result = new List<ItemGroup<StoreItem>>();
        foreach (StoreItem item in ItemDatabase)
        {
            var itemGroup = FindOrCreateItemGroup(item.GetType().ToString());
            itemGroup.Add(item);
            if (!result.Contains(itemGroup))
                result.Add(itemGroup);
        }
        string log = "Store item groups: ";
        result.ForEach(item => log += (item.ToString() + ", "));
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
}

