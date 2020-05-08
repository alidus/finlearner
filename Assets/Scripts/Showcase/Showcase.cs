using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public abstract class Showcase<T, TClass> : MonoBehaviour where T : Item where TClass : Component
{
    public static Showcase<T, TClass> instance;
    private ItemGroup<T> selectedItemGroup;

    public ItemDatabase<T> ItemDatabase { get; protected set; } = new ItemDatabase<T>();
    public List<ItemGroup<T>> ItemGroups { get; set; } = new List<ItemGroup<T>>();
    public ItemGroup<T> SelectedItemGroup { get => selectedItemGroup; set { 
            if (selectedItemGroup != value) 
            { 
                if (selectedItemGroup != null)
                {
                    selectedItemGroup.IsSelected = false;
                }
                if (value != null)
                {
                    value.IsSelected = true;
                }
                selectedItemGroup = value;
                OnSelectedItemGroupChanged?.Invoke();
            }  } }

    public Action OnSelectedItemGroupChanged;


    public View RootView { get; set; }
    /// <summary>
    /// Update showcase view
    /// </summary>
    public abstract void UpdateShowcase();
    /// <summary>
    /// Destroy showcase view
    /// </summary>
    /// 
    protected ItemGroup<T> FindItemGroup(string title)
    {
        return ItemGroups.FirstOrDefault(group => group.Title == title);
    }
    private void OnDisable()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    protected void DestroyViews()
    {
        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    protected virtual void AddItemsToDatabase(T item)
    {
        ItemDatabase.Add(item);
    }

    protected virtual void AddItemsToDatabase(List<T> items)
    {
        foreach (T item in items)
        {
            AddItemsToDatabase(item);
        }
    }

    protected virtual void RemoveItemsFromDatabase(T item)
    {
        ItemDatabase.Remove(item);
    }

    protected virtual void RemoveItemsFromDatabase(List<T> items)
    {
        foreach (T item in items)
        {
            RemoveItemsFromDatabase(item);
        }
    }

    private void OnDestroy()
    {
        Console.Print("Destroying showcase: " + this.ToString());   
    }

    protected abstract ItemDatabase<T> LoadAssets();

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
