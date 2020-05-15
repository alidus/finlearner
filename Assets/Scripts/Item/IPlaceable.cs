using UnityEngine;
using System.Collections;

public interface IPlaceable
{
    IEquipable Equipable { get; }
    IDrawable Drawable { get; }
}
