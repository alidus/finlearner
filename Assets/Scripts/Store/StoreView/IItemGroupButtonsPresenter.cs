using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemGroupButtonsPresenter<T> where T : Item
{
    List<ItemGroup<T>> ItemGroups { get; }
    void Update();
}
