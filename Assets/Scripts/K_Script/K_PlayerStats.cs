using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class K_PlayerStats : MonoBehaviour
{

    public Attribute[] attributes;
    public GameObject[] displayValue; 
    public Attribute Agility => attributes[0];

    private InventoryObject _equipment;

    private void Start()
    {
        _equipment = GetComponent<K_PlayerItemSystem>().inven_Equipment;

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
            attributes[i].textUI = displayValue[i];
        }

        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
            _equipment.GetSlots[i].onAfterUpdated += OnEquipItem;
        }
    }

    public void AttributeModified(Attribute attribute)
    {
        attribute.textUI.GetComponentInChildren<TextMeshProUGUI>().text = string.Concat(": ", attribute.value.ModifiedValue);
    }

    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null)
            return;
        switch (slot.parent.inventory.type)
        {
            //case InterfaceType.Inventory:
            //    print("Removed " + slot.GetItemObject() + " on: " + slot.parent.inventory.type + ", Allowed items: " +
            //          string.Join(", ", slot.AllowedItems));
            //    break;

            case InterfaceType.Equipment:
                    
                for (int i = 0; i < slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == slot.item.buffs[i].stat)
                            attributes[j].value.RemoveModifier(slot.item.buffs[i]);
                    }
                }
                break;

           
        }
    }

    public void OnEquipItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null)
            return;
        switch (slot.parent.inventory.type)
        {
            //case InterfaceType.Inventory:
            //    print("Placed " + slot.GetItemObject() + " on: " + slot.parent.inventory.type + ", Allowed items: " +
            //          string.Join(", ", slot.AllowedItems));
            //    break;

            case InterfaceType.Equipment:
                 print("Placed " + slot.GetItemObject() + " on: " + slot.parent.inventory.type + ", Allowed items: " +
                      string.Join(", ", slot.allowedItems));
                for (int i = 0; i < slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == slot.item.buffs[i].stat)
                            attributes[j].value.AddModifier(slot.item.buffs[i]);
                    }
                }
                break;

           
        }
    }
}
