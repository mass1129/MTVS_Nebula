using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
using Cysharp.Threading.Tasks;

public class K_EquipAndClothesInterface : K_UserInterface
{
    public GameObject[] slots;
    
    public override void CreateSlots()
    {
        if (!photonView.IsMine) return;
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            
            inventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
            
        }
    }

    public override void OnDragEnd(GameObject obj)
    {   
        base.OnDragEnd(obj);
        if (MouseData.interfaceMouseIsOver == null&&!UtilsClass.IsPointerOverUI())
        {
            inventory.ForGiveItem(slotsOnInterface[obj],CHAN_GameManager.instance.avatarName).Forget();
            
        }
    }




}
