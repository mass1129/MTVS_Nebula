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

    public string avatarName { get; private set; }

    public CharacterCustomization  _CharacterCustomization;

    public async UniTask SetItemSystem(string s)
    {
        if (!photonView.IsMine) return;
        avatarName = s;
        await _CharacterCustomization.LoadCharacterFromFile(s, false);
        GetComponent<K_MoneySystem>().MoneySystemSetting(s);
        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].parent = equipmentUI;
            _equipment.GetSlots[i].onAfterUpdated += OnEquipItem;
        }
        await GetComponent<K_PlayerStats>().SetPlayerStats();
        ItemLoad();
    }


    private void OnDestroy()
    {
        if (!photonView.IsMine) return;
        
        TwoInvenSave().Forget();

        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
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
        
        var itemObject = slot.GetItemObject();
        if (itemObject == null)
        {
            switch (slot.allowedItems[0])
            {
                case ItemType.Hat:
                    _CharacterCustomization.SetElementByIndex(CharacterElementType.Hat,-1);
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

        else
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
                   
    }




    public async UniTask TwoInvenSave()
    {
        if (!photonView.IsMine) return;
        SaveTwoInven saveObject = new SaveTwoInven
        {
            equipment = _equipment.GetInventory(),
            clothesInventory = inven_Cloths.GetInventory()
        };
        await UniTask.DelayFrame(2);
        string json = JsonUtility.ToJson(saveObject, true);
        
        var url = "https://resource.mtvs-nebula.com/" + _equipment.savePath + avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());
        await httpReq.Post(url, json);

    }

    public void ItemSave()
    {
        TwoInvenSave().Forget();
    }
    public void ItemLoad()
    {
        inven_Cloths.InventoryLoad(avatarName).Forget();
        _equipment.InventoryLoad(avatarName).Forget();
        inven_Building.InventoryLoad(avatarName).Forget();
    }


    [System.Serializable]
    public class SaveTwoInven
    {
        public Inventory equipment;
        public Inventory clothesInventory;
    }

}