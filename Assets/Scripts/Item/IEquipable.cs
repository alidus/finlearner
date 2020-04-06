public interface IEquipable
{
    bool CanBeEquipped { get; set; }
    bool IsEquipped { get; }
    void Equip();
    void Uneqip();
}