using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System;

public enum InterFaceType
{
    Inventory,
    Equipment,
    Chest
}
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterFaceType type;
    public Inventory Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    public bool AddItem(Item _item, int _amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(_item);
        if (!database.ItemObjects[_item.Id].stackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
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
    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == _item.Id)
            {
                return GetSlots[i];
            }
            
        }
        return null;

    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for(int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        //set up functionality for full inventory
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {   
        if(item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }

    }

    public void RemoveItem(Item _item)
    {
        for(int i =0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item == _item)
            {
                GetSlots[i].UpdateSlot( null, 0);
            }
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

//전체 인벤토리
[System.Serializable]
public class Inventory
{
    //아이템 슬롯(아이템종류, 아이템 수량)으로 이루어진 리스트
    public InventorySlot[] Slots = new InventorySlot[24];
    public void Clear()
    {
        for(int i = 0; i<Slots.Length; i++)
        {
            Slots[i].RemoveItem();  
        }
    }
}

public delegate void SlotUpdated(InventorySlot _slot);
//아이템슬룻 한개
[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public K_UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    public Item item;
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            if(item.Id>=0)
            {
                return parent.inventory.database.ItemObjects[item.Id];
            }
            return null;
        }
    }
    //기본 new InventorySlot시 만들어지는 형태
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    //매개변수를 넣을때 만들어지는 형태
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }
    //슬룻한개를 업데이트하는 함수
    public void UpdateSlot(Item _item, int _amount)
    {
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);
        item = _item;
        amount = _amount;
        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }
    //슬룻한개의 갯수를 제어하는 함수
    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
        
    }
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if(AllowedItems.Length <=0 || _itemObject ==null || _itemObject.data.Id<0)
        {
            return true;
        }
        for(int i=0; i<AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }

}
