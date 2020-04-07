using UnityEngine;
using UnityEditor;

public interface IViewPrice
{
    float Price { get; set; }

    void UpdatePrice();
}