using System;

public delegate void EquippableInstanceHandler(IEquipable equipable);

public interface IEquipable
{
    bool CanBeEquipped { get; set; }
    bool IsEquipped { get; }
    void Equip();
    void Uneqip();

    event Action OnEquipStateChanged;
    event Action OnEquippableStateChanged;
    event EquippableInstanceHandler OnInstanceEquipStateChanged;
    event EquippableInstanceHandler OnInstanceEquippableStateChanged;

    void NotifyOnInstanceEquipStateChanged(IEquipable equipable);
    void NotifyOnInstanceEquippableStateChanged(IEquipable equipable);

}