using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class K_BuildingQuickSlotInterface : K_UserInterface
{

    public GameObject[] subSlots;
    bool isAdded = false;
   
    public override void CreateSlots()
    {
      
        
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = subSlots[i];
            inventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);

        }
        isAdded = true;
    }

    
}
