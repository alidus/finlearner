using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialSelectorButton : MonoBehaviour
{
    public IPlaceable placeableItem { get; set; }
    RadialSelector radialSelector;
    private Button button;
    private Image iconImage;

    public void Init(RadialSelector radialSelector)
    {
        this.radialSelector = radialSelector;
    }

    private void OnEnable()
    {
        button = GetComponent<Button>();

        if (button)
        {
            button.onClick.AddListener(delegate 
            {
                placeableItem.Equipable.Equip();
                radialSelector.SelectItem(placeableItem);
            });
        }
    }

    public void UpdateButton()
    {
       iconImage = transform.Find("Icon")?.GetComponent<Image>();
       if (iconImage)
           iconImage.sprite = placeableItem.Drawable.Sprite;
    }

    
}
