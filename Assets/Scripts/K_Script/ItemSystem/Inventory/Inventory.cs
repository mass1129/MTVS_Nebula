using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

[System.Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = new Item();
            slots[i].amount = 0;
        }
    }

    public bool ContainsItem(ItemObject itemObject)
    {
        return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
    }


    public bool ContainsItem(int id)
    {
        return slots.FirstOrDefault(i => i.item.id == id) != null;
    }
}
