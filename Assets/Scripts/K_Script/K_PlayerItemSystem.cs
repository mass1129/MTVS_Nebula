using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_PlayerItemSystem : MonoBehaviour
{
    
    public bool isVisible = false;
    public InventoryObject inven_Cloths;
    public InventoryObject inven_Building;
    public InventoryObject inven_System;
    public InventoryObject inven_Default;
    public InventoryObject inven_Vehicle;
    public InventoryObject equipment;


    private void Update()
    {
        UpdateDisplay();
        
    }
    private void UpdateDisplay()
    {


        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isVisible)
            {
               
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                isVisible = true;
            }
            else
            {
                
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                isVisible = false;
            }

        }
    }
    public void OnTriggerEnter(Collider other)
    {   
        //부딪힌 객체의 groundItem 컴포넌트를 가져온다.
        var item = other.GetComponent<GroundItem>();
        InventoryObject _inventory = null;
        //컴포넌트가 있을때
        if (item)
        {   
            //_item을 grounditem컴포넌트의 item변수의 아이템으로 만든다.
            Item _item = new Item(item.item);
            switch(item.item.type)
            {
                case ItemType.Hair         :
                case ItemType.Beard        :
                case ItemType.Accessory    :
                case ItemType.Hat          :
                case ItemType.Shirt        :
                case ItemType.Pants        :
                case ItemType.Shoes        :
                case ItemType.Bag          :
                case ItemType.Weapons:
                case ItemType.Title:
                _inventory = inven_Cloths;
                    break;
                case ItemType.BuildObject:
                    _inventory = inven_Building;
                    break;
                case ItemType.Default:
                    _inventory = inven_Default;
                    break;
                case ItemType.System:
                    _inventory = inven_System;
                    break;
                case ItemType.Vehicle:
                    _inventory = inven_Vehicle;
                    break;



            }
            //InventoryObject에 (bool)Additem를 실행한다. 그리고 리턴값이 true이면 
            if (_inventory.AddItem(_item, 1))
            {
                //그라운드 아이템 객체를 파괴한다.
                Destroy(other.gameObject);
            }
            
            
        }
    }
   
    
    private void OnApplicationQuit()
    {
        inven_Cloths.Clear();
        inven_Building.Clear();
        inven_System.Clear();
        inven_Default.Clear();
        inven_Vehicle.Clear();
        equipment.Clear();
        
    }

   
}

