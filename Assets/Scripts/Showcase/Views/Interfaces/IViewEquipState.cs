using UnityEngine;
using UnityEditor;

public interface IViewEquipState
{
    bool IsActive { get; set; }

    void UpdateEquippedState();
}