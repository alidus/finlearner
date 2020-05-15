using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "SO/Items/Car", fileName = "Car")]
public class CarItem : StoreItem, IPurchasable, IEquipable, IDrawable, IPlaceable
{
    public IEquipable Equipable => this;
    public IDrawable Drawable => this;
}
