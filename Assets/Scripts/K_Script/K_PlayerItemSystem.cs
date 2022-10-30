using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_PlayerItemSystem : MonoBehaviour
{
    
    bool isVisible = false;
    public InventoryObject inventory;
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
        var item = other.GetComponent<GroundItem>();
        if(item)
        {
            Item _item = new Item(item.item);
            if(inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
            
            
        }
    }
   
    
    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }

   
}

