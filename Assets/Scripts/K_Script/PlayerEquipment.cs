using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;
using Photon.Pun;
public class PlayerEquipment : CharacterCustomization


{
    public InventoryObject _equipment;


   



    void Start()
    {
        //if (!photonView.IsMine) this.enabled = false; 
        

       

        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
            _equipment.GetSlots[i].onAfterUpdated += OnEquipItem;
        }
    }
    //public override void KSetElementByIndex(CharacterElementType type, int index)
    //{
    //    photonView.RPC("RpcSetTrigger", RpcTarget.AllBuffered, );
    //}

    //[PunRPC]
    //public override void RPCSetElementByIndex(CharacterElementType type, int index)
    //{
    //    characterSelectedElements.SetSelectedIndex(type, index);
    //}

    public void OnEquipItem(InventorySlot slot)
    {
        var itemObject = slot.GetItemObject();
        if (itemObject == null)
            return;
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Equipment:

                if (itemObject.uiDisplay != null)
                {
                    switch (slot.allowedItems[0])
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

                    
                }
                break;
        }
    }

    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null)
            return;
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Equipment:
                if (slot.GetItemObject().uiDisplay != null)
                {
                    switch (slot.allowedItems[0])
                    {

                        case ItemType.Hat:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Hat, 0);
                            break;

                        case ItemType.Accessory:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Accessory, -1);
                            break;

                        case ItemType.Hair:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Hair, -1);
                            break;

                        case ItemType.Beard:
                            CharacterCustomization.SetElementByIndex(CharacterElementType.Beard, 0);
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

                }
            break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }



    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{

    //}
}