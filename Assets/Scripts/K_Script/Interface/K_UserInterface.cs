using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using CodeMonkey.Utils;



public abstract class K_UserInterface : MonoBehaviour
{
    //public K_PlayerItemSystem player;
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();


    bool isAddedEvent = false;



    private void OnEnable()
    {
        if (!isAddedEvent)
        {
            CreateSlots();
            isAddedEvent = true;
        }
        inventory.UpdateInventory();
    }


    private void OnDestroy()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].onAfterUpdated -= OnSlotUpdate;
        }
    }

    public abstract void CreateSlots();


    public abstract void OnSlotUpdate(InventorySlot slot);

    




    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (!trigger) { Debug.LogWarning("No EventTrigger component found!"); return; }
        var eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);

    }


    protected void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<K_UserInterface>();
    }
    protected void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    protected void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    protected void OnExit(GameObject obj)
    {
        //player.mouseItem.hoverObj = null;
        //player.mouseItem.hoverItem = null;
        MouseData.slotHoveredOver = null;

    }

    protected void OnDragStart(GameObject obj)
    {
        Debug.Log(slotsOnInterface[obj].item.name);
        //slotsOnInterface[obj].UpdateSlot(slotsOnInterface[obj].item, slotsOnInterface[obj].amount);
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    protected void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    protected void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];

            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);

        }
        if (MouseData.interfaceMouseIsOver == null && !UtilsClass.IsPointerOverUI())
        {
            inventory.ForGiveItem(slotsOnInterface[obj], CHAN_GameManager.instance.avatarName).Forget();

        }

    }

    protected GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].GetItemObject().uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }


    //public void Update()
    //{
    //    if (!photonView.IsMine || _previousInventory == inventory)
    //        return;

    //    UpdateInventoryLinks();

    //}

    //public void UpdateInventoryLinks()
    //{
    //    int i = 0;
    //    foreach (var key in slotsOnInterface.Keys.ToList())
    //    {
    //        slotsOnInterface[key] = inventory.GetSlots[i];
    //        i++;
    //    }
    //    _previousInventory = inventory;
    //}

}




