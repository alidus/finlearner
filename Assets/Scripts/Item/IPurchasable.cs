using System;

public interface IPurchasable
{
    float Price { get; set; }
    bool CanBePurchased { get; set; }
    bool IsPurchased { get; }
    void Buy();

    event Action OnBuy;
    event Action OnSell;


}