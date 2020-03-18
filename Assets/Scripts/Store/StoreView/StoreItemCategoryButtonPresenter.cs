using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class StoreItemCategoryButtonPresenter : MonoBehaviour, IItemGroupButtonPresenter<ObjectItem>
{
    public GameObject StoreCategoryPanel { get; private set; }
    GameDataManager gameDataManager;
    public Image ImageComponent { get; private set; }
    public Text TextComponent { get; private set; }


    public Button ButtonComponent { get; set; }
    public ButtonClickedEvent OnClick { get => ButtonComponent.onClick; set => ButtonComponent.onClick = value; }

    public Sprite Sprite { get; set; }
    public ItemGroup<ObjectItem> ItemGroup { get; set; }

    private void OnEnable()
    {
        ImageComponent = this.GetComponent<Image>();
        TextComponent = this.GetComponentInChildren<Text>();
        ButtonComponent = this.GetComponent<Button>();
    }

    public void UpdatePresenter()
    {
        if (ItemGroup != null)
        {
            TextComponent.text = ItemGroup.Title;
        }
        if (ImageComponent != null)
        {
            ImageComponent.sprite = Sprite;
        }
    }
}
