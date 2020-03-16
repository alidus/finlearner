using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemGroupButtonPresenter<T> where T : Item
{
    ItemGroup<T> ItemGroup { get; set; }
    void UpdatePresenter();
}
