using UnityEngine;
using UnityEditor;

public interface IViewPurchaseState
{
    bool IsPurchased { get; set; }

    void UpdatePurchasedState();
}