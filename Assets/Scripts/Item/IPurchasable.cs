using System;

public delegate void PurchasableInstanceHandler(IPurchasable purchasable);

public interface IPurchasable
{
    float Price { get; set; }
    bool CanBePurchased { get; set; }
    bool IsPurchased { get; }
    void Buy();

    event Action OnPurchaseStateChanged;
    event Action OnPurchasableStateChanged;
    event PurchasableInstanceHandler OnInstancePurchaseStateChanged;
    event PurchasableInstanceHandler OnInstancePurchasableStateChanged;

    void NotifyOnInstancePurchaseStateChanged(IPurchasable purchasable);
    void NotifyOnInstancePurchasableStateChanged(IPurchasable purchasable);


}