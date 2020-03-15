public interface IPurchasable
{
    bool CanBePurchased { get; set; }
    bool IsPurchased { get; }
    void Purchase();
}