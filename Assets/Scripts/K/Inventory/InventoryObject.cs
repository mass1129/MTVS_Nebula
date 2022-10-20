using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject database;
    //������ ����(����������, ������ ����)���� �̷���� ����Ʈ
    public List<InventorySlot> Container = new List<InventorySlot>();
    //������ �߰��Լ�
    public void AddItem(ItemObject _item, int _amount)
    {   
       
        //�������� ������ŭ
        for(int i = 0; i < Container.Count; i++)
        {   
            //������ ����Ʈ i �ڸ��� ���� �������� �ִٸ�
            if(Container[i].item == _item)
            {   
                //�������� ������ ���Ѵ�.
                Container[i].AddAmount(_amount);
                return;
            }
        }
        //���ο� ������ ������ ����Ʈ�� ���Ѵ�.
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
