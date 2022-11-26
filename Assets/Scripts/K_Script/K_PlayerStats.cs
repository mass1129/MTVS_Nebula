using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UltimateClean;
public class K_PlayerStats : MonoBehaviour
{

    public Attribute[] attributes;
    public GameObject[] displayValue;
    public SlicedFilledImage[] _fillImg;
    public Attribute Agility => attributes[0];

    private InventoryObject _equipment;

    bool isAdded = false;
    private void Start()
    {   
      
            _equipment = GetComponent<K_PlayerItemSystem>()._equipment;

            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i].SetParent(this);
                attributes[i].textUI = displayValue[i];
                attributes[i].fillImg = _fillImg[i];
            }

            for (int i = 0; i < _equipment.GetSlots.Length; i++)
            {
                _equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
                _equipment.GetSlots[i].onAfterUpdated += OnEquipItem;
            }

        
       
       

    }
    private void OnDisable()
    {
        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].onBeforeUpdated -= OnRemoveItem;
            _equipment.GetSlots[i].onAfterUpdated -= OnEquipItem;
        }
    }
    private void OnApplicationQuit()
    {
        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].onBeforeUpdated -= OnRemoveItem;
            _equipment.GetSlots[i].onAfterUpdated -= OnEquipItem;
        }
    }
    public void AttributeModified(Attribute attribute)
    {
        attribute.textUI.GetComponentInChildren<TextMeshProUGUI>().text = string.Concat(attribute.value.ModifiedValue , "%");
        attribute.fillImg.fillAmount= (float)attribute.value.ModifiedValue/100f;
    }

    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null)
            return;
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Inventory_Cloths:
            //    print("Removed " + slot.GetItemObject() + " on: " + slot.parent.inventory.type + ", Allowed items: " +
            //          string.Join(", ", slot.AllowedItems));
                break;

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
            case InterfaceType.Inventory_Cloths:
                print("Placed " + slot.GetItemObject() + " on: " + slot.parent.inventory.type + ", Allowed items: " +
                      string.Join(", ", slot.allowedItems));
                break;

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
