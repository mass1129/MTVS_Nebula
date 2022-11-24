using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer
{
    void BoughItem(ItemObject obj);

    bool TrySpendGoldAmount(int goldAmount);
}

