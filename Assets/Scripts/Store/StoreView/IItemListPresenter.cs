using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemListPresenter<T> where T : Item
{
    ItemGroup<T> ItemGroup { get; }
    void UpdatePresenter();
}
