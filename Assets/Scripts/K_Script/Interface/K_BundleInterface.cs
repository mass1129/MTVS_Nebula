using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class K_BundleInterface : K_UserInterface
{
    public GameObject inventoryPrefab;


    public GameObject[] slots;
   
    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.Select, delegate { OnSelect(obj); });
            AddEvent(obj, EventTriggerType.Deselect, delegate { OnDeselect(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);

        }

    }
    public override void DistorySlots()
    { }


}
