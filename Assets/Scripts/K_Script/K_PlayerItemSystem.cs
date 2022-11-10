using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_PlayerItemSystem : MonoBehaviour
{

    
    public InventoryObject inven_Cloths;
    public InventoryObject inven_Building;
    public InventoryObject inven_Default;
    public InventoryObject inven_Vehicle;
    public InventoryObject inven_Equipment;
    //

    private void Update()
    {
      

    }
   
    public void OnTriggerEnter(Collider other)
    {
        //�ε��� ��ü�� groundItem ������Ʈ�� �����´�.
        var item = other.GetComponent<GroundItem>();
        InventoryObject _inventory = null;
        //������Ʈ�� ������
        if (item)
        {
            //_item�� grounditem������Ʈ�� item������ ���������� �����.
            Item _item = new Item(item.item);
            switch (item.item.type)
            {
                case ItemType.Hair:
                case ItemType.Beard:
                case ItemType.Accessory:
                case ItemType.Hat:
                case ItemType.Shirt:
                case ItemType.Pants:
                case ItemType.Shoes:
                case ItemType.Bag:
                case ItemType.Weapons:
                case ItemType.Title:
                    _inventory = inven_Cloths;
                    break;
                case ItemType.Bundle_Building:
                    _inventory = inven_Building;
                    break;
                case ItemType.Default:
                    _inventory = inven_Default;
                    break;
                case ItemType.Vehicle:
                    _inventory = inven_Vehicle;
                    break;



            }
            //InventoryObject�� (bool)Additem�� �����Ѵ�. �׸��� ���ϰ��� true�̸� 
            if (_inventory.AddItem(_item, 1))
            {
                //�׶��� ������ ��ü�� �ı��Ѵ�.
                Destroy(other.gameObject);
            }


        }
    }


    private void OnApplicationQuit()
    {
        inven_Cloths.Clear();
        inven_Building.Clear();
        inven_Default.Clear();
        inven_Vehicle.Clear();
        inven_Equipment.Clear();

    }
    public async void TestTwoInvenSave()
    {
       
        SaveTwoInven saveObject = new SaveTwoInven
        {
            equipment = inven_Equipment.GetInventory(),
            clothesInventory = inven_Cloths.GetInventory()
        };
        string json = JsonUtility.ToJson(saveObject, true);
        Debug.Log(json);
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/" + inven_Equipment.savePath + "/testAvatar";
        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);

    }

    [System.Serializable]
    public class SaveTwoInven
    {
        public Inventory equipment;
        public Inventory clothesInventory;
    }
}


