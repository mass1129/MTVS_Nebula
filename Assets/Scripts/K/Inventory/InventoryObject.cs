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

//    private void OnEnable()
//    {
//#if UNITY_EDITOR
//        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/ItemDatabase.asset", typeof(ItemDatabaseObject));
//#else
//        database = Resources.Load<ItemDatabaseObject>("ItemDatabase");
//#endif
//    }
    //������ �߰��Լ�
    public void AddItem(Item _item, int _amount)
    {   
       
        //�������� ������ŭ
        for(int i = 0; i < Container.Items.Count; i++)
        {   
            //������ ����Ʈ i �ڸ��� ���� �������� �ִٸ�
            if(Container.Items[i].item.Id == _item.Id)
            {   
                //�������� ������ ���Ѵ�.
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        //���ο� ������ ������ ����Ʈ�� ���Ѵ�.
        Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
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
        //    if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        //{
        //    //BinaryFormatter bf = new BinaryFormatter();
        //    //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
        //    //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
        //    //file.Close();
        //    IFormatter formatter = new BinaryFormatter();
        //    Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
        //    Container = (Inventory)formatter.Deserialize(stream);
        //    stream.Close();
        //}
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }
   
}

[System.Serializable]
public class Inventory
{
    //������ ����(����������, ������ ����)���� �̷���� ����Ʈ
    public List<InventorySlot> Items = new List<InventorySlot>();
}
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int amount;
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }


}
