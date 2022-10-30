using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_PlayerItemSystem : MonoBehaviour
{
    
    bool isVisible = false;
    public InventoryObject inventory;
    public InventoryObject equipment;

    public Attribute[] attributes;

    private void Start()
    {
        for(int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for(int i=0; i<equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {   
        if(_slot.ItemObject==null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterFaceType.Inventory:

                break;
            case InterFaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ",_slot.parent.inventory.type, "Allowed Items: ", 
                    string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }
                break;
            case InterFaceType.Chest:
                break;
            default:
                break;
        }

       
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterFaceType.Inventory:

                break;
            case InterFaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, "Allowed Items: ",
                    string.Join(", ", _slot.AllowedItems)));

                for(int i=0; i<_slot.item.buffs.Length; i++)
                {
                    for(int j =0; j<attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }
                break;
            case InterFaceType.Chest:
                break;
            default:
                break;
        }
        
    }
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
   
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, "was updated! Value is now", attribute.value.ModifiedValue));
    }
    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }

   
}
[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public K_PlayerItemSystem parent;
    public Attributes type;
    public ModifiableInt value;
    public void SetParent(K_PlayerItemSystem _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}
