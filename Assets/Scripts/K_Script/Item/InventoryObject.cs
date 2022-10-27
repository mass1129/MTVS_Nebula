using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;

    public void AddItem(Item _item, int _amount)
    {
        if (_item.buffs.Length > 0)
        {
            SetEmptySlot(_item, _amount);
            return;
        }
        //아이템의 갯수만큼
        for (int i = 0; i < Container.Items.Length; i++)
        {
            //아이템 리스트 i 자리에 같은 아이템이 있다면
            if (Container.Items[i].ID == _item.Id)
            {
                //아이템의 갯수를 더한다.
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetEmptySlot(_item, _amount);
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for(int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <=-1)
            {
                Container.Items[i].UpdateSlot(_item.Id, _item, _amount);
                return Container.Items[i];
            }
        }
        //set up functionality for full inventory
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item _item)
    {
        for(int i =0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }
    [ContextMenu("Save")]
    public void Save()
    {

        string json = JsonUtility.ToJson(Container, true);
        PlayerPrefs.SetString("InventorySystemSave", json);
        K_SaveSystem.Save("InventorySystemSave", json, true);

    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (PlayerPrefs.HasKey("InventorySystemSave"))
        {
            string json = PlayerPrefs.GetString("InventorySystemSave");
            json = K_SaveSystem.Load("InventorySystemSave");

            JsonUtility.FromJsonOverwrite(json, Container);

        }

    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }
   
}

//전체 인벤토리
[System.Serializable]
public class Inventory
{
    //아이템 슬롯(아이템종류, 아이템 수량)으로 이루어진 리스트
    public InventorySlot[] Items = new InventorySlot[24];
}

//아이템슬룻 한개
[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;
    //기본 new InventorySlot시 만들어지는 형태
    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }
    //매개변수를 넣을때 만들어지는 형태
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    //슬룻한개를 업데이트하는 함수
    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    //슬룻한개의 갯수를 제어하는 함수
    public void AddAmount(int value)
    {
        amount += value;
    }


}
