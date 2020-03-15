using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class DefaultStoreItemView : IStoreItemView
{
    private GameObject storeItemPanel;

    private Button buttonComponent;

    public StoreItem StoreItem { get; private set; }

    private GameObject iconPanel;


    public string Title
    {
        get => StoreItem.Title;
    }
    public Sprite Sprite
    {
        get => StoreItem.Sprite;
    }

    public float Price
    {
        get => StoreItem.GetBehaviour<ItemPurchaseBehaviour>()?.Price ?? -1;
    }

    private Text priceTextComponent;
    private Text titleTextComponent;
    private Image iconImageComponent;
    private GameObject ownagePanel;
    private GameObject equipIndicator;
  
    public  ButtonClickedEvent OnClick { get => buttonComponent.onClick; set => buttonComponent.onClick = value; }

    public DefaultStoreItemView(StoreItem storeItem, Transform parent)
    {
        StoreItem = storeItem;
        storeItemPanel = GameObject.Instantiate(Resources.Load("Prefabs/Store/Views/DefaultStoreItemView") as GameObject, parent);
        buttonComponent = storeItemPanel.GetComponent<Button>();
        iconPanel = storeItemPanel.transform.Find("Icon").gameObject;
        priceTextComponent = iconPanel.transform.Find("Price").GetComponentInChildren<Text>();
        titleTextComponent = storeItemPanel.transform.Find("TitleText").GetComponent<Text>();
        iconImageComponent = iconPanel.GetComponent<Image>();
        ownagePanel = iconPanel.transform.Find("OwnageIndicator").gameObject;
        equipIndicator = storeItemPanel.transform.Find("EquipIndicator").gameObject;
        Update();
    }

    public void Update()
    {
        UpdatePriceView();
        UpdateTitleView();
        UpdateIconView();
        UpdateOwnageIndicatiorView();
        UpdateHightlightView();
    }

    private void UpdateHightlightView()
    {
        equipIndicator.SetActive(StoreItem.GetBehaviour<FurnitureEquipBehaviour>()?.IsEquipped ?? false);
    }

    private void UpdateOwnageIndicatiorView()
    {
        ownagePanel.SetActive(StoreItem.GetBehaviour<ItemPurchaseBehaviour>()?.IsPurchased ?? false);
    }

    void UpdatePriceView()
    {
        priceTextComponent.text = "$" + Price.ToString();
    }

    void UpdateTitleView()
    {
        titleTextComponent.text = Title;
    }

    void UpdateIconView()
    {
        iconImageComponent.sprite = StoreItem.Sprite != null ? StoreItem.Sprite : GameDataManager.instance.placeHolderSprite;
    }
}
