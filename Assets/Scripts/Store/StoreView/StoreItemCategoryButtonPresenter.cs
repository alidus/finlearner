using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class StoreItemCategoryButtonPresenter : MonoBehaviour, IItemGroupButtonPresenter<ItemObject>
{
    public GameObject StoreCategoryPanel { get; private set; }
    GameDataManager gameDataManager;
    public Image ImageComponent { get; private set; }
    public Text TextComponent { get; private set; }


    public Button ButtonComponent { get; set; }
    public ButtonClickedEvent OnClick { get => ButtonComponent.onClick; set => ButtonComponent.onClick = value; }

    public string Title { get => StoreCategoryPanel.GetComponentInChildren<Text>().text;
        set { StoreCategoryPanel.GetComponentInChildren<Text>().text = value; } }
    public Sprite Sprite { get => StoreCategoryPanel.GetComponent<Image>().sprite;
        set => StoreCategoryPanel.GetComponent<Image>().sprite = value; }
    public ItemGroup<ItemObject> ItemGroup { get; set; }

    private void OnEnable()
    {
        ItemGroup = this.GetComponentInParent<Store>().SelectedItemGroup;
        gameDataManager = GameDataManager.instance;
        ImageComponent = StoreCategoryPanel.GetComponent<Image>();
        TextComponent = StoreCategoryPanel.GetComponentInChildren<Text>();
        ButtonComponent = StoreCategoryPanel.GetComponent<Button>();
        Sprite = gameDataManager.placeHolderSprite;
        Title = "DEFAULT_STORE_CATEGORY_TITLE";
    }
    public void Update()
    {
        ItemGroup = this.GetComponentInParent<Store>().SelectedItemGroup;

        Title = ItemGroup.Title;
    }
}
