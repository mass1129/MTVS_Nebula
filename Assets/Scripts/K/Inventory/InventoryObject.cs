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
    //아이템 추가함수
    public void AddItem(Item _item, int _amount)
    {   
       
        //아이템의 갯수만큼
        for(int i = 0; i < Container.Items.Count; i++)
        {   
            //아이템 리스트 i 자리에 같은 아이템이 있다면
            if(Container.Items[i].item.Id == _item.Id)
            {   
                //아이템의 갯수를 더한다.
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        //새로운 아이템 슬롯을 리스트에 더한다.
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
    //아이템 슬롯(아이템종류, 아이템 수량)으로 이루어진 리스트
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
