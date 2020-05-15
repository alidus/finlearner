using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialSelector : View
{
    List<IPlaceable> placeableItems = new List<IPlaceable>();
    private Transform buttonsWrapper;
    private Text emptyText;
    private Button button;
    public PlaceableObject placeableObject { get; set; }
    private void OnEnable()
    {
        emptyText = transform.Find("EmptyText")?.GetComponent<Text>();
        buttonsWrapper = transform.Find("ButtonsWrapper")?.transform;
        button = GetComponent<Button>();
        button.onClick.AddListener(HideAndDestroy);
    }

    internal void SelectItem(IPlaceable placeableItem)
    {
        placeableObject.Init(placeableItem);
        placeableObject.UpdateView();
    }

    public void Init(PlaceableObject placeableObject)
    {
        this.placeableObject = placeableObject;
        transform.position = placeableObject.transform.position;
    }

    void UpdateFurnitureButtons()
    {
        // Destroy all buttons
        foreach (Transform child in buttonsWrapper)
        {
            Destroy(child.gameObject);
        }
        // Calculate buttons position based on furnitureItems count

        if (emptyText)
        {
            if (placeableItems.Count == 0)
            {
                emptyText.enabled = true;
            }
            else
            {
                emptyText.enabled = false;
            }
        }
        

        float angleBetweenButtons = placeableItems.Count > 0 ? 360 / placeableItems.Count : 0;

        for (int i = 0; i < placeableItems.Count; i++)
        {
            Vector2 buttonPosition = new Vector2(0, 0);
            buttonPosition = Quaternion.AngleAxis(i * angleBetweenButtons, Vector3.forward) * Vector3.up * 120;
            RadialSelectorButton radialSelectorButton = (Instantiate(Resources.Load("Prefabs/HUD/RadialSelectorButton"), buttonsWrapper) as GameObject).GetComponent<RadialSelectorButton>();
            radialSelectorButton.Init(this);
            radialSelectorButton.transform.localPosition = buttonPosition;
            radialSelectorButton.placeableItem = placeableItems[i];
            radialSelectorButton.UpdateButton();
        }
    }

    internal void HideAndDestroy()
    { 
        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Hide");
        Destroy(gameObject, animator != null ? GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length : 0);
    }

    private void OnDestroy()
    {
        
    }

    public void UpdatePlaceableList(List<IPlaceable> placeables)
    {
        placeableItems = placeables;
    }

    public override void UpdateView()
    {
        UpdateFurnitureButtons();
    }
}
