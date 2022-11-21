using System;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
    public ItemType[] allowedItems = new ItemType[0];

    [System.NonSerialized]
    public K_UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;

    [System.NonSerialized]
    public Action<InventorySlot> onAfterUpdated;
    [System.NonSerialized]
    public Action<InventorySlot> onBeforeUpdated;

   
   

    public ItemObject GetItemObject()
    {
        
        return item.id >= 0 ? parent.inventory.database.ItemObjects[item.id] : null;
    }
    
    public InventorySlot() => UpdateSlot(new Item(), 0);

    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);
    

    public void RemoveItem() => UpdateSlot(new Item(), 0);

    public void AddAmount(int value) => UpdateSlot(item, amount += value);


    public void UpdateSlot(Item itemValue, int amountValue)
    {
        onBeforeUpdated?.Invoke(this);
        item = itemValue;
        amount = amountValue;
        onAfterUpdated?.Invoke(this);
    }
   

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if (allowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
            return true;
        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (itemObject.type == allowedItems[i])
                return true;
        }
        return false;
    }

}
