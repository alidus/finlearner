using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemsPresenter<T> where T : Item
{

    void UpdatePresenter();

}
