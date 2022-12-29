using UnityEngine;
using AdvancedPeopleSystem;
using Photon.Pun;
using Cysharp.Threading.Tasks;

public class K_PlayerItemSystem : MonoBehaviourPun
     

{
    public InventoryObject inven_Cloths;
    public InventoryObject inven_Building;
    public InventoryObject _equipment;

    public K_UserInterface equipmentUI;
    public K_Player player;

    public CharacterCustomization  _CharacterCustomization;


    public async UniTask SetEquipmentSystem()
    {
        if (!photonView.IsMine) return;
        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].parent = equipmentUI;
            _equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
            _equipment.GetSlots[i].onAfterUpdated += OnEquipItem;
        }
        await UniTask.Yield();
    }


    private void OnDestroy()
    {
        if (!photonView.IsMine) return;
        
        TwoInvenSave().Forget();

        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].onBeforeUpdated -= OnRemoveItem;
            _equipment.GetSlots[i].onAfterUpdated -= OnEquipItem;
        }
        inven_Building.Clear();

        
    }

    public void UpdateEquipment()
    {
        _equipment.UpdateInventory();
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


    public async UniTaskVoid TwoInvenSave()
    {
        if (!photonView.IsMine) return;
        SaveTwoInven saveObject = new SaveTwoInven
        {
            equipment = _equipment.GetInventory(),
            clothesInventory = inven_Cloths.GetInventory()
        };
        await UniTask.DelayFrame(2);
        string json = JsonUtility.ToJson(saveObject, true);
        
        var url = "https://resource.mtvs-nebula.com/" + _equipment.savePath + player.avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());
        await httpReq.Post(url, json);

    }

    public void ItemSave()
    {
        TwoInvenSave().Forget();
    }
    public void ItemLoad()
    {
        inven_Cloths.InventoryLoad(player.avatarName).Forget();
        _equipment.InventoryLoad(player.avatarName).Forget();
        inven_Building.InventoryLoad(player.avatarName).Forget();


    }
    private void OnApplicationQuit()
    {
        inven_Cloths.Clear();
        _equipment.Clear();
        inven_Building.Clear();
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