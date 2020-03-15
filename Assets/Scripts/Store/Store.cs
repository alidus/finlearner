using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store component managing its view and logic 
/// </summary>
public class Store : MonoBehaviour
{
    [SerializeField]
    private ItemDatabase<ItemObject> itemDatabase;

    public List<ItemGroup<ItemObject>> ItemGroups { get; set; }
    public ItemDatabase<ItemObject> ItemDatabase { get => itemDatabase; set => itemDatabase = value; }
    public StorePresenter StorePresenter { get; private set; }
    public ItemGroup<ItemObject> SelectedItemGroup { get; internal set; }

    private void OnEnable()
    {
        StorePresenter = StoreFactory.CreateStorePresenter(this.transform);
    }

    private void OnDisable()
    {
        GameObject.Destroy(StorePresenter);
    }

    public void SelectItemGroup(ItemGroup<ItemObject> itemGroup)
    {
        SelectedItemGroup = ItemGroups.Find(group => itemGroup == group) ?? SelectedItemGroup; 
    }
}

