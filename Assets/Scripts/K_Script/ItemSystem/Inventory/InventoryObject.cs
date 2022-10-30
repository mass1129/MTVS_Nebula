using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;

    [SerializeField]
    private Inventory Container = new Inventory();
    public InventorySlot[] GetSlots => Container.Slots;

    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(item);
        if (!database.ItemObjects[item.Id].stackable || slot == null)
        {
            GetEmptySlot().UpdateSlot(item, amount);
            return true;
        }
        slot.AddAmount(amount);
        return true;
    }



    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for(int i =0; i<GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <=-1)
                    counter++;
            }
            return counter;
        }
    }
    public InventorySlot FindItemOnInventory(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id)
            {
                return GetSlots[i];
            }
            
        }
        return null;

    }
    public bool IsItemInInventory(ItemObject item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.data.Id)
            {
                return true;
            }
        }
        return false;
    }


    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1 == item2)
            return;
        if (item2.CanPlaceInSlot(item1.GetItemObject()) && item1.CanPlaceInSlot(item2.GetItemObject()))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }

    }


  
    [ContextMenu("Save")]
    public void Save()
    {

        string json = JsonUtility.ToJson(Container, true);
        PlayerPrefs.SetString(savePath, json);
        K_SaveSystem.Save(savePath, json, true);

    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (PlayerPrefs.HasKey(savePath))
        {
            string json = PlayerPrefs.GetString(savePath);
            json = K_SaveSystem.Load(savePath);

            JsonUtility.FromJsonOverwrite(json, Container);
            for(int i = 0; i < Container.Slots.Length; i++)
            {
                Container.Slots[i].UpdateSlot(Container.Slots[i].item, Container.Slots[i].amount);
            }
        }

    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
   
}

