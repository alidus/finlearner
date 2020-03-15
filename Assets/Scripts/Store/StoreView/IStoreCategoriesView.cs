using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoreCategoriesView
{
    StoreCatalog StoreCatalog { get; }
    GameObject StoreCategoriesPanel { get; set; }


    void Update();

}
