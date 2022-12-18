using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerEquipment : MonoBehaviourPun
{

    
    public InventoryObject inven_Cloths;
    public InventoryObject inven_Building;
    public InventoryObject inven_Default;
    public InventoryObject inven_Vehicle;
    public InventoryObject inven_Equipment;
    public K_Player player;




   











    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
            //부딪힌 객체의 groundItem 컴포넌트를 가져온다.
            var item = other.GetComponent<GroundItem>();
        InventoryObject _inventory = null;
        //컴포넌트가 있을때
        if (item)
        {
            //_item을 grounditem컴포넌트의 item변수의 아이템으로 만든다.
            Item _item = new Item(item.item);
            Debug.Log(_item.name);
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
            //InventoryObject에 (bool)Additem를 실행한다. 그리고 리턴값이 true이면 
            if (_inventory.AddItem(_item, 1))
            {
                //그라운드 아이템 객체를 파괴한다.
               //Destroy(other.gameObject);
            }

        }
    }


















  
   
}


