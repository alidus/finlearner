using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store component managing its view and logic 
/// </summary>

    [ExecuteInEditMode]
public class Store : MonoBehaviour
{
    public StoreItemDatabases storeItemDatabases;

    [SerializeField]
    private ItemDatabase<ObjectItem> itemDatabase = new ItemDatabase<ObjectItem>();

    public List<ItemGroup<ObjectItem>> ItemGroups { get; set; }
    public ItemDatabase<ObjectItem> ItemDatabase { get => itemDatabase; set => itemDatabase = value; }
    public StorePresenter StorePresenter { get; private set; }
    public ItemGroup<ObjectItem> SelectedItemGroup { get; internal set; }

    private void OnEnable()
    {
        if (storeItemDatabases)
        {
            ItemDatabase = storeItemDatabases.GetAllObjectItemsDatabase();
            ItemGroups = GetObjectItemGroups();
            if (ItemGroups.Count > 0)
                SelectedItemGroup = ItemGroups[0];
        } else
        {
            storeItemDatabases = InventoryManager.instance.StoreItemDatabases;
        }
        if (StorePresenter)
        {
            DestroyImmediate(StorePresenter.gameObject);
        }
        StorePresenter = StoreFactory.CreateStorePresenter(this.transform);
    }

    private void OnDisable()
    {
        if (StorePresenter)
        {
            DestroyImmediate(StorePresenter.gameObject);
        }
    }



    public void SelectItemGroup(ItemGroup<ObjectItem> itemGroup)
    {
        SelectedItemGroup = ItemGroups.Find(group => itemGroup == group) ?? SelectedItemGroup; 
    }

    public void UpdateAll()
    {
        if (StorePresenter != null)
        {
            StorePresenter.UpdatePresenter();
        }
        
    }

    public List<ItemGroup<ObjectItem>> GetObjectItemGroups()
    {
        List<ItemGroup<ObjectItem>> result = new List<ItemGroup<ObjectItem>>();
        foreach (ObjectItem item in ItemDatabase)
        {
            if (item is FurnitureItem)
            {
                if (result.Count > 0)
                {
                   var k = result[0].GetType().GetGenericTypeDefinition();
                }
                var groupOfType = result.Find(group => group.Items[0] is FurnitureItem);
                if (groupOfType != null)
                {
                    groupOfType.Add(item);
                } else
                {
                    var newGroup = new ItemGroup<ObjectItem>();
                    newGroup.Title = "Мебель";
                    newGroup.Add(item);
                    result.Add(newGroup);
                }
            }
        }
        string log = "Store item groups: ";
        result.ForEach(item => log += (item.ToString() + ", "));
        Debug.Log(log);
        return result;
    }
}

