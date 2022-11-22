using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;
using Photon.Pun;
public class PlayerEquipment :MonoBehaviourPun, IPunObservable


{
    private InventoryObject _equipment;

    //public static GameObject LocalPlayerInstance;
    
    private  CharacterCustomization  _CharacterCustomization;

    private void Awake()
    {

       // if (!photonView.IsMine) this.enabled = false;

    }
    private void OnEnable()
    {
        Debug.Log("111");
        if (photonView.IsMine)
        {
            _equipment = GetComponent<K_PlayerItemSystem>().inven_Equipment;
            _CharacterCustomization = this.gameObject.GetComponent<CharacterCustomization>();
            //_equipment.TestLoad(PlayerPrefs.GetString("AvatarName"));

            for (int i = 0; i < _equipment.GetSlots.Length; i++)
            {
                _equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
                _equipment.GetSlots[i].onAfterUpdated += OnEquipItem;
            }
            Debug.Log("222");
        }
    }
    void Start()
    {
       
        

    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {


            for (int i = 0; i < _equipment.GetSlots.Length; i++)
            {
                _equipment.GetSlots[i].onBeforeUpdated -= OnRemoveItem;
                _equipment.GetSlots[i].onAfterUpdated -= OnEquipItem;
            }
            Debug.Log("222");
        }
    }
    private void OnApplicationQuit()
    {
        if (photonView.IsMine)
        {
           

            for (int i = 0; i < _equipment.GetSlots.Length; i++)
            {
                _equipment.GetSlots[i].onBeforeUpdated -= OnRemoveItem;
                _equipment.GetSlots[i].onAfterUpdated -= OnEquipItem;
            }
            Debug.Log("222");
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
    public void cc()
    {
        _CharacterCustomization = GetComponent<CharacterCustomization>();
        if (_CharacterCustomization != null)
            Debug.Log("loadCC");
    }
    public void OnEquipItem(InventorySlot slot)
    {
        if (!photonView.IsMine) return;

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
                             _CharacterCustomization.SetElementByIndex(CharacterElementType.Hat, itemObject.charCustomIndex);
                            break;

                        case ItemType.Accessory:
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Accessory, itemObject.charCustomIndex);
                           
                            break;        

                        case ItemType.Shirt:
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Shirt, itemObject.charCustomIndex);
                            break;
                        case ItemType.Pants:
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Pants, itemObject.charCustomIndex);
                            break;
                        case ItemType.Shoes:
                           _CharacterCustomization.SetElementByIndex(CharacterElementType.Shoes, itemObject.charCustomIndex);
                            break;
                        case ItemType.Bag:
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Item1, itemObject.charCustomIndex);
                            break;

                    }

                    
                }
                break;
        }
    }
    public ItemDatabaseObject dataBase;
    public void OnRemoveItem(InventorySlot slot)
    {
        if (!photonView.IsMine) return;
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
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Hat, -1);
                            break;

                        case ItemType.Accessory:
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Accessory, -1);
                            //var go = PhotonNetwork.Instantiate("item", transform.position, Quaternion.identity);
                            //go.GetComponent<GroundItem>().SetItem(slot.item.id, slot.item.uniqueId);
                            break;

                        case ItemType.Shirt:
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Shirt, -1);
                            break;
                        case ItemType.Pants:
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Pants, -1);
                            break;
                        case ItemType.Shoes:
                            _CharacterCustomization.SetElementByIndex(CharacterElementType.Shoes, -1);
                            break;
                        case ItemType.Bag:
                             _CharacterCustomization.SetElementByIndex(CharacterElementType.Item1, -1);
                            break;
                    }

                }
                break;
            case InterfaceType.Inventory_Cloths:
                if (slot.GetItemObject().uiDisplay != null)
                {
                   var go = PhotonNetwork.Instantiate("item", transform.position, Quaternion.identity);
                    
                        
                        



                }
                    break;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }

   



    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{

    //}
}