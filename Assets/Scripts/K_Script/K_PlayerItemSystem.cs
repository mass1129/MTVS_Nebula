using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;
using Photon.Pun;
using Cysharp.Threading.Tasks;
public class K_PlayerItemSystem : MonoBehaviourPun, IPunObservable
     

{
    public InventoryObject inven_Cloths;
    public InventoryObject inven_Building;
    public InventoryObject inven_Default;
    public InventoryObject inven_Vehicle;
    public InventoryObject _equipment;
   
    public K_Player player;

    //public static GameObject LocalPlayerInstance;

    CharacterCustomization  _CharacterCustomization;
    bool isdone = false;
    private void Awake()
    {
       
          

    }

    public void UpdateItemSystem()
    {
        _CharacterCustomization = GetComponent<CharacterCustomization>();
        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
            _equipment.GetSlots[i].onAfterUpdated += OnEquipItem;
        }
    }
    public void RPCUpdateItemSystem()
    {
        
    }



    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            ItemSave();

            for (int i = 0; i < _equipment.GetSlots.Length; i++)
            {
                _equipment.GetSlots[i].onBeforeUpdated -= OnRemoveItem;
                _equipment.GetSlots[i].onAfterUpdated -= OnEquipItem;
            }
            Debug.Log("222");
        }
    }
    







    public void OnEquipItem(InventorySlot slot)
    {
        if (photonView.IsMine)
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

       
    }
    public ItemDatabaseObject dataBase;
    public void OnRemoveItem(InventorySlot slot)
    {
        if (photonView.IsMine)
        {
            var itemObject = slot.GetItemObject();
            if (itemObject == null)
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
                                _CharacterCustomization.SetElementByIndex(CharacterElementType.Shirt, 1);
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
      
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }
    public async UniTaskVoid TwoInvenSave()
    {
        if (!photonView.IsMine) return;
        SaveTwoInven saveObject = new SaveTwoInven
        {
            equipment = _equipment.GetInventory(),
            clothesInventory = inven_Cloths.GetInventory()
        };
        string json = JsonUtility.ToJson(saveObject, true);
        Debug.Log(json);
        //string s = PlayerPrefs.GetString(" AvatarName");
        var url = "https://resource.mtvs-nebula.com/" + _equipment.savePath + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);
        _equipment.Clear();
        inven_Cloths.Clear();
        await UniTask.Yield();
    }

    public void ItemSave()
    {
        TwoInvenSave().Forget();
    }
    public void ItemLoad()
    {
        inven_Cloths.TestLoad();
        _equipment.TestLoad();
        inven_Building.TestLoad();
        

    }
    private void OnApplicationQuit()
    {

    }

    [System.Serializable]
    public class SaveTwoInven
    {
        public Inventory equipment;
        public Inventory clothesInventory;
    }




    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{

    //}
}