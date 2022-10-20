using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject database;
    //아이템 슬롯(아이템종류, 아이템 수량)으로 이루어진 리스트
    public List<InventorySlot> Container = new List<InventorySlot>();
    //아이템 추가함수
    public void AddItem(ItemObject _item, int _amount)
    {   
       
        //아이템의 갯수만큼
        for(int i = 0; i < Container.Count; i++)
        {   
            //아이템 리스트 i 자리에 같은 아이템이 있다면
            if(Container[i].item == _item)
            {   
                //아이템의 갯수를 더한다.
                Container[i].AddAmount(_amount);
                return;
            }
        }
        //새로운 아이템 슬롯을 리스트에 더한다.
        Container.Add(new InventorySlot(database.GetId[_item],_item, _amount));
        
    }

}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObject item;
    public int amount;
    public InventorySlot(int _id, ItemObject _item, int _amount)
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
