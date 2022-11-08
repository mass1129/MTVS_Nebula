using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class K_StaticInterface_bundleList : K_UserInterface
{

    public GameObject[] subSlots;
    bool isAdded = false;
    public override void CreateSlots()
    {
      
        
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = subSlots[i];

            if (!isAdded)
            {
                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            }
            
            inventory.GetSlots[i].slotDisplay = obj;
            inventory.GetSlots[i].parent = this;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);

        }
        isAdded = true;
    }
    public override void DistorySlots()
    {
        foreach (var key in slotsOnInterface.Keys.ToList())
        {
            Destroy(key);
        }
        slotsOnInterface.Clear();
    }

    
}
