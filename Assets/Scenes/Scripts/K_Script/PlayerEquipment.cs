using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;

public class PlayerEquipment : MonoBehaviour
{
    private InventoryObject _equipment;


    private CharacterCustomization CharacterCustomization;



    void Start()
    {
        _equipment = GetComponent<K_PlayerItemSystem>().equipment;

        CharacterCustomization = GetComponent<CharacterCustomization>();

        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
            _equipment.GetSlots[i].onAfterUpdated += OnEquipItem;
        }
    }

    private void OnEquipItem(InventorySlot slot)
    {
        var itemObject = slot.GetItemObject();
        if (itemObject == null)
            return;
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Equipment:

                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Hat:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Hat, itemObject.charCustomIndex);
                            break;

                        case ItemType.Accessory:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Accessory, itemObject.charCustomIndex);
                            break;

                        case ItemType.Hair:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Hair, itemObject.charCustomIndex);
                            break;

                        case ItemType.Beard:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Beard, itemObject.charCustomIndex);
                            break;

                        case ItemType.Shirt:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Shirt, itemObject.charCustomIndex);
                            break;
                        case ItemType.Pants:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Pants, itemObject.charCustomIndex);
                            break;
                        case ItemType.Shoes:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Shoes, itemObject.charCustomIndex);
                            break;
                        case ItemType.Bag:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Item1, itemObject.charCustomIndex);
                            break;

                    }

                    break;
        }
    }

    private void OnRemoveItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null)
            return;
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Equipment:

                switch (slot.AllowedItems[0])
                {

                    case ItemType.Hat:
                        CharacterCustomization.SetElementByIndex(CharacterElementType.Hat, -1);
                        break;

                    case ItemType.Accessory:
                        CharacterCustomization.SetElementByIndex(CharacterElementType.Accessory, -1);
                        break;

                    case ItemType.Hair:
                        CharacterCustomization.SetElementByIndex(CharacterElementType.Hair, -1);
                        break;

                    case ItemType.Beard:
                        CharacterCustomization.SetElementByIndex(CharacterElementType.Beard, -1);
                        break;

                    case ItemType.Shirt:
                        CharacterCustomization.SetElementByIndex(CharacterElementType.Shirt, -1);
                        break;
                    case ItemType.Pants:
                        CharacterCustomization.SetElementByIndex(CharacterElementType.Pants, -1);
                        break;
                    case ItemType.Shoes:
                        CharacterCustomization.SetElementByIndex(CharacterElementType.Shoes, -1);
                        break;
                    case ItemType.Bag:
                        CharacterCustomization.SetElementByIndex(CharacterElementType.Item1, -1);
                        break;
                }

            break;
        }
    }
}