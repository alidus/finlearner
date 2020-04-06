using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Store component managing its view and logic 
/// </summary>

public class Store : Showcase<StoreItem>
{
    StoreViewFactory<StoreItem> factory;

    private void OnEnable()
    {
        // Load store database array containing different StoreItem databases of various item types (like clothing, furniture, etc)

        foreach (StoreItem item in Resources.LoadAll("ScriptableObjects/Store/Catalog").ToList().ConvertAll(x => (StoreItem)x))
        {
            ItemDatabase.Add(item);
        }

        ItemGroups = GetItemGroups();
        if (ItemGroups.Count > 0)
            SelectedItemGroup = ItemGroups[0];

        if (factory == null)
        {
            factory = new StoreViewFactory<StoreItem>(
                this,
                Resources.Load("Prefabs/Store/Views/StoreView"),
                Resources.Load("Prefabs/Store/Views/StoreItemGroupListView"),
                Resources.Load("Prefabs/Store/Views/StoreItemGroupView"),
                Resources.Load("Prefabs/Store/Views/StoreItemListView"),
                Resources.Load("Prefabs/Store/Views/StoreItemView"));
        }

        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        RootView = factory.CreateRootView(this.transform);
        
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

    protected override List<ItemGroup<StoreItem>> GetItemGroups()
    {
        List<ItemGroup<StoreItem>> result = new List<ItemGroup<StoreItem>>();
        foreach (StoreItem item in ItemDatabase)
        {
            if (item is Furniture)
            {
                if (result.Count > 0)
                {
                    var k = result[0].GetType().GetGenericTypeDefinition();
                }
                var groupOfType = result.Find(group => group.Items[0] is Furniture);
                if (groupOfType != null)
                {
                    groupOfType.Add(item);
                }
                else
                {
                    var newGroup = new ItemGroup<StoreItem>();
                    newGroup.Title = "Мебель";
                    newGroup.Add(item);
                    result.Add(newGroup);
                }
            }
        }
        string log = "Store item groups: ";
        result.ForEach(item => log += (item.ToString() + ", "));
        return result;
    }

    
}

