using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class DefaultStoreItemView : IStoreItemView
{
    public GameObject StoreItemPanel { get; set; }

    public Button ButtonComponent { get; set; }
    public  ButtonClickedEvent OnClick { get => ButtonComponent.onClick; set => ButtonComponent.onClick = value; }

    public DefaultStoreItemView(GameObject storeItemPanelPrefab, Transform parent)
    {
        StoreItemPanel = GameObject.Instantiate(storeItemPanelPrefab, parent);
        ButtonComponent = StoreItemPanel.GetComponent<Button>();
    }

    

}
