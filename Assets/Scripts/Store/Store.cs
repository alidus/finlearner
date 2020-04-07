using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store component managing its view and logic 
/// </summary>

public class Store : MonoBehaviour
{
    public StoreItemDatabases storeItemDatabases;

    [SerializeField]
    private ItemDatabase<ObjectItem> itemDatabase = new ItemDatabase<ObjectItem>();
    [SerializeField]
    private StoreView storePresenter;
    [SerializeField]
    private Object storeViewObject;

    public List<ItemGroup<ObjectItem>> ItemGroups { get; set; }
    public ItemDatabase<ObjectItem> ItemDatabase { get => itemDatabase; set => itemDatabase = value; }
    public StoreView StoreView { get => storePresenter; private set => storePresenter = value; }
    public ItemGroup<ObjectItem> SelectedItemGroup { get; internal set; }

    private void OnEnable()
    {
        storeItemDatabases = ScriptableObject.Instantiate(Resources.Load("ScriptableObjects/Store/StoreItemDatabases")) as StoreItemDatabases;

        if (storeItemDatabases)
        {
            ItemDatabase = storeItemDatabases.GetAllObjectItemsDatabase();
            ItemGroups = GetObjectItemGroups();
            if (ItemGroups.Count > 0)
                SelectedItemGroup = ItemGroups[0];
        }
        if (storeViewObject == null)
        {
            storeViewObject = Resources.Load("Prefabs/Store/Views/StoreView");
        }

        if (transform.childCount == 0)
        {
            StoreView = ViewFactory.CreateBaseView(storeViewObject, this.transform);
        } else
        {
            StoreView = transform.GetChild(0)?.GetComponent<StoreView>();
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }



    public void SelectItemGroup(ItemGroup<ObjectItem> itemGroup)
    {
        SelectedItemGroup = ItemGroups.Find(group => itemGroup == group) ?? SelectedItemGroup;
    }

    public void UpdateAll()
    {
        if (StoreView != null)
        {
            StoreView.UpdateView();
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
                }
                else
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
        return result;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}

