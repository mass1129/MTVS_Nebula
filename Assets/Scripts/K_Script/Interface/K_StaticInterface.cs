using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
using Cysharp.Threading.Tasks;

public class K_StaticInterface : K_UserInterface
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
    public override void DistorySlots()
    { }
    public override void OnDragEnd(GameObject obj)
    {   
        base.OnDragEnd(obj);
        if (MouseData.interfaceMouseIsOver == null&&!UtilsClass.IsPointerOverUI())
        {
            ForGiveItem(obj).Forget();
            
        }
    }

    public async UniTask ForGiveItem(GameObject obj)
    {
        DropItem dropItem = new DropItem
        {
            uniqueId = slotsOnInterface[obj].item.uniqueId
        };
        if (dropItem.uniqueId < 0) return;
        string json = JsonUtility.ToJson(dropItem, true);
        Debug.Log(json);
        var url = "https://resource.mtvs-nebula.com/achieve/drop/" + CHAN_GameManager.instance.avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);

        await inventory.InventoryLoad(CHAN_GameManager.instance.avatarName);
    }
    [System.Serializable]
    public class DropItem
    {
        public int uniqueId;


    }


}
