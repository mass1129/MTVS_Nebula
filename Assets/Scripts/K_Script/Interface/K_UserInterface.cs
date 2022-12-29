using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Linq;
using Photon.Pun;

[RequireComponent(typeof(EventTrigger))]
public abstract class K_UserInterface : MonoBehaviourPun
{
    //public K_PlayerItemSystem player;
    public InventoryObject inventory;
    private InventoryObject _previousInventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

  
    bool isAddedEvent = false;
    public bool onQuickSlot=false;
    bool needFirstUpdate = false;
  
  
    public void OnEnable()
    {
        if (!photonView.IsMine) return;
         if (!isAddedEvent && !needFirstUpdate)
         {
            UISetting();
            isAddedEvent = true;
        }
        
    }

    public void UISetting()
    {
        if (!photonView.IsMine) return;
        CreateSlots();

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            if(inventory.type != InterfaceType.Equipment)
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].onAfterUpdated += OnSlotUpdate;
        }

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });

        inventory.UpdateInventory();
    }
    private void OnDestroy()
    {
        if (!photonView.IsMine) return;
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {


            inventory.GetSlots[i].onAfterUpdated -= OnSlotUpdate;
        }
    }
   
    public abstract void CreateSlots();

    public abstract void DistorySlots();
    public void UpdateInventoryLinks()
    {
        if (!photonView.IsMine) return;
        int i = 0;
        foreach (var key in slotsOnInterface.Keys.ToList())
        {
            slotsOnInterface[key] = inventory.GetSlots[i];
            i++;
        }
    }
    private void OnSlotUpdate(InventorySlot slot)
    {
        
        if (slot.item.id <= -1)
        {
            slot.slotDisplay.transform.GetChild(0).GetComponent<Image>().sprite = null;
            slot.slotDisplay.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            if (slot.parent.inventory.type == InterfaceType.Equipment)
            {
                slot.slotDisplay.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {

            slot.slotDisplay.transform.GetChild(0).GetComponent<Image>().sprite = slot.GetItemObject().uiDisplay;
            slot.slotDisplay.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            if (slot.parent.inventory.type == InterfaceType.Equipment)
            {
                slot.slotDisplay.transform.GetChild(1).gameObject.SetActive(false);
            }

        }
    }

    public void Update()
    {
        if (!photonView.IsMine) return;
        if (_previousInventory != inventory)
        {
            UpdateInventoryLinks();
        }
        _previousInventory = inventory;

    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (!trigger) { Debug.LogWarning("No EventTrigger component found!"); return; }
        var eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);

    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<K_UserInterface>();
    }
    public void OnExit(GameObject obj)
    {
        //player.mouseItem.hoverObj = null;
        //player.mouseItem.hoverItem = null;
        MouseData.slotHoveredOver = null;

    }
    
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnDragStart(GameObject obj)
    {
        Debug.Log(slotsOnInterface[obj].item.name);
        //slotsOnInterface[obj].UpdateSlot(slotsOnInterface[obj].item, slotsOnInterface[obj].amount);
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    
   

    private GameObject CreateTempItem(GameObject obj)
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
    public virtual void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);
        
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];

                inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
     
        }

    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    public void OnSelect(GameObject obj)
    {
 

    }
    public void OnDeselect(GameObject obj)
    {
       

    }


}

public static class MouseData
{
    public static K_UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
    public static GameObject slotSelected;
}
