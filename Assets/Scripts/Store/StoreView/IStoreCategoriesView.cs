using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoreCategoriesView
{
    StoreCatalog StoreCatalog { get; set; }
    GameObject StoreCategoriesPanel { get; set; }

    GameObject StoreCategoryPanelPrefab { get; set; }

    void Update();

}
