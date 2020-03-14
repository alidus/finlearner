using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoreCategoryView
{
    GameObject StoreCategoryPanel { get; }

    string Title { get; set; }
    Sprite Sprite { get; set; }
}
