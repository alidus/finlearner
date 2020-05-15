using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class DefaultItemGroupView<T> : View, IViewTitle where T : Item
{
    ItemGroup<T> itemGroup;

    public Image ImageComponent { get; private set; }
    public Text TextComponent { get; private set; }

    public Button ButtonComponent { get; set; }
    public ButtonClickedEvent OnClick { get => ButtonComponent.onClick; set => ButtonComponent.onClick = value; }
    public string Title { get; set; }

    public bool IsSelected { get; set; }


    private void OnEnable()
    {
        ImageComponent = this.GetComponent<Image>();
        TextComponent = this.GetComponentInChildren<Text>();
        ButtonComponent = this.GetComponent<Button>();
    }

    public void Init(ItemGroup<T> itemGroup)
    {
        this.itemGroup = itemGroup;

        if (itemGroup != null)
        {
            Title = itemGroup.Title;
            itemGroup.OnSelectedStateChanged -= TargetItemGroupSelectedStateChangedHandler;
            itemGroup.OnSelectedStateChanged += TargetItemGroupSelectedStateChangedHandler;
            TargetItemGroupSelectedStateChangedHandler();
        }
    }

    private void OnDestroy()
    {
        if (itemGroup != null)
            itemGroup.OnSelectedStateChanged -= TargetItemGroupSelectedStateChangedHandler;
    }

    public void Init(string title)
    {
        Title = title;
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateSelectionState();
    }

    private void UpdateSelectionState()
    {
        if (IsSelected)
        {
            ImageComponent.color = GameDataManager.instance.GroupButtonSelectedColor;
        } else
        {
            ImageComponent.color = GameDataManager.instance.GroupButtonDefaultColor;
        }
    }

    public void UpdateTitle()
    {
        TextComponent.text = Title;
    }

    void TargetItemGroupSelectedStateChangedHandler()
    {
        IsSelected = itemGroup.IsSelected; 
        UpdateView();
    }

}
