using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer
{
    void BoughtItem(ItemObject obj);

    bool TrySpendGoldAmount(int goldAmount);
}

