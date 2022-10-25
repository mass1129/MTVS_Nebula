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
        #region past
        //string saveData = JsonUtility.ToJson(Container, true);
        //Debug.Log(saveData);
        //스트림으로부터 인코딩된 문자를 읽고 이를 스트림에 쓰기 위한 형식을 제공합니다.
        //기본값을 사용하여 BinaryFormatter 클래스의 새 인스턴스를 초기화합니다.
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream ? 파일을 읽고 쓰는 데 사용됩니다.
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //개체나 지정된 최상위(루트)를 가진 개체의 그래프를 해당 스트림으로 serialize합니다.
        //bf.Serialize(file, saveData);
        //현재 스트림을 닫고 현재 스트림과 관련된 소켓과 파일 핸들 등의 리소스를 모두 해제합니다.
        //이 메서드를 호출하는 대신 스트림이 올바르게 삭제되었는지 확인합니다.
        //file.Close();
        #endregion
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
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
