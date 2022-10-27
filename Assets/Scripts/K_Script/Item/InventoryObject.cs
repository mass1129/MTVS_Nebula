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
        //�������� ������ŭ
        for (int i = 0; i < Container.Items.Length; i++)
        {
            //������ ����Ʈ i �ڸ��� ���� �������� �ִٸ�
            if (Container.Items[i].ID == _item.Id)
            {
                //�������� ������ ���Ѵ�.
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

//��ü �κ��丮
[System.Serializable]
public class Inventory
{
    //������ ����(����������, ������ ����)���� �̷���� ����Ʈ
    public InventorySlot[] Items = new InventorySlot[24];
}

//�����۽��� �Ѱ�
[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;
    //�⺻ new InventorySlot�� ��������� ����
    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }
    //�Ű������� ������ ��������� ����
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    //�����Ѱ��� ������Ʈ�ϴ� �Լ�
    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    //�����Ѱ��� ������ �����ϴ� �Լ�
    public void AddAmount(int value)
    {
        amount += value;
    }


}
