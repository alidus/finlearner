using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemPresenter<T> where T : Item
{
     T Item { get; }
    void Update();
}
