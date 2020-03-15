using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class StoreItemPresenter : MonoBehaviour, IItemPresenter<ItemObject>
{
    public Button ButtonComponent { get; set; }
    public  ButtonClickedEvent OnClick { get => ButtonComponent.onClick; set => ButtonComponent.onClick = value; }

    public ItemObject Item { get; set; }

    public void Update()
    {
        Transform iconTransform = this.transform.Find("Icon");
        iconTransform.transform.Find("PriceTag").GetComponentInChildren<Text>().text = "$" + Item.Price.ToString();
        this.transform.Find("TitleText").GetComponent<Text>().text = Item.Title;
        iconTransform.GetComponent<Image>().sprite = Item.Sprite != null ? Item.Sprite : GameDataManager.instance.placeHolderSprite;

        this.transform.Find("EquipHighlightPanel").gameObject.SetActive(Item.IsEquipped);
        iconTransform.transform.Find("OwnIndicator").gameObject.SetActive(Item.IsPurchased);
    }
}
