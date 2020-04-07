using System;

public interface IEquipable
{
    bool CanBeEquipped { get; set; }
    bool IsEquipped { get; }
    void Equip();
    void Uneqip();

    event Action OnEquip;
    event Action OnUnEquip;

}