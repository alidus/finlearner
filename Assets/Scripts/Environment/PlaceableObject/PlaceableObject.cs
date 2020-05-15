using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : View
{
    protected CircleCollider2D circleCollider;
    protected RadialSelector radialSelector;
    protected RadialSelectorSpawner radialSelectorSpawner;
    protected Inventory inventory;
    protected IPlaceable placeableItem;
    protected SpriteRenderer spriteRenderer;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        radialSelectorSpawner = FindObjectOfType<RadialSelectorSpawner>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = Inventory.instance as Inventory;
    }

    public void Init(IPlaceable placeableItem)
    {
        this.placeableItem = placeableItem;
    }

    private void OnMouseDown()
    {
        if (radialSelector == null)
        {
            radialSelector = radialSelectorSpawner.SpawnRadialSelector(this);
        }
        radialSelector.UpdateView();
    }

    public override void UpdateView()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = placeableItem.Drawable.Sprite;
    }
}
