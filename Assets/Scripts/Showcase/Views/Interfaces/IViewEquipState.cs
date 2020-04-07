using UnityEngine;
using UnityEditor;

public interface IViewEquipState
{
    bool IsEquipped { get; set; }

    void UpdateEquippedState();
}