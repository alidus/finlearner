public interface IPurchasable
{
    float Price { get; set; }
    bool CanBePurchased { get; set; }
    bool IsPurchased { get; }
    void Purchase();
}