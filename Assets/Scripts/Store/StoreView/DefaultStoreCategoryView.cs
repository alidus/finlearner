using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class DefaultStoreCategoryView : IStoreCategoryView
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



    public DefaultStoreCategoryView(GameObject storeCategoryPanelPrefab, Transform parent)
    {
        gameDataManager = GameDataManager.instance;
        StoreCategoryPanel = GameObject.Instantiate(storeCategoryPanelPrefab, parent);
        ImageComponent = StoreCategoryPanel.GetComponent<Image>();
        TextComponent = StoreCategoryPanel.GetComponentInChildren<Text>();
        ButtonComponent = StoreCategoryPanel.GetComponent<Button>();
        Sprite = gameDataManager.placeHolderSprite;
        Title = "DEFAULT_STORE_CATEGORY_TITLE";
    }


}
