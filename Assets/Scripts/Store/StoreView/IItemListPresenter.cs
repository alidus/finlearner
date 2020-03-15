using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemListPresenter<T> where T : Item
{
    ItemDatabase<T> ItemDatabase { get; }
    void Update();
}
