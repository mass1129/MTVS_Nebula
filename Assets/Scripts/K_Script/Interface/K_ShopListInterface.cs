using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cysharp.Threading.Tasks;

public class K_ShopListInterface : MonoBehaviourPun
{


    public InventoryObject shopList;
    public K_MoneySystem moneySystem;
    
    public ShopSlotModule[] shopSlotModule;

    public Dictionary<InventorySlot, ShopSlotModule> slotsOnInterface;

    private bool firstLoad = false;
    
    private void OnEnable()
    {
        if (!photonView.IsMine) return;
        if(!firstLoad)
        {
            slotsOnInterface = new Dictionary<InventorySlot, ShopSlotModule>();
            for (int i = 0; i < shopList.GetSlots.Length; i++)
            {
                var obj = shopSlotModule[i];
                slotsOnInterface.Add(shopList.GetSlots[i], obj);
            }

            for (int i = 0; i < shopList.GetSlots.Length; i++)
            {
                shopList.GetSlots[i].onAfterUpdated += OnSlotUpdate;
            }
            firstLoad = true;
        }
        ShopItemLoad().Forget();
    }


    private void TryBuyItem(InventorySlot slot)
    {
        if (!photonView.IsMine) return;
        ItemObject itemObject = slot.GetShopItemObject(shopList);
        switch (itemObject.type)
        {
            case ItemType.Hair:
            case ItemType.Beard:
            case ItemType.Accessory:
            case ItemType.Hat:
            case ItemType.Shirt:
            case ItemType.Pants:
            case ItemType.Shoes:
            case ItemType.Bag:
            case ItemType.Weapons:
            case ItemType.Title:
                moneySystem.BuyClothesEvent(itemObject).Forget();
                break;
            case ItemType.Bundle_Building:
                moneySystem.BuyBBundleEvent(itemObject).Forget();
                break;

        }


    }
    
    public void ShopListUpdate()
    {
        shopList.UpdateInventory();

    }    

    private void OnDestroy()
    {
        if (!photonView.IsMine) return;
        for (int i = 0; i < shopList.GetSlots.Length; i++)
        {
            shopList.GetSlots[i].onAfterUpdated -= OnSlotUpdate;
        }
        shopList.Clear();
    }
    private void OnSlotUpdate(InventorySlot slot)
    {
        slotsOnInterface[slot].SetShopModule(slot, shopList);
        slotsOnInterface[slot].buyButton.onClick.AddListener(() => TryBuyItem(slot));
    }

    public async UniTask ShopItemLoad()
    {
        if (!photonView.IsMine) return;
        shopList.Clear();
        var url = "https://resource.mtvs-nebula.com/" + shopList.loadPath + moneySystem.avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Shop_Root result2 = await httpReq.Get<H_Shop_Root>(url);

        List<H_Shop_items> newList = new List<H_Shop_items>();
        newList.AddRange(result2.results.clothesList);
        if(result2.results.bundleList.Count>0)
            newList.Add(result2.results.bundleList[0]);

        

        for (int i = 0; i < newList.Count; i++)
        {
            int temp = i;
            ItemObject itemObject = shopList.database.ItemObjects[newList[temp].id];
            shopList.GetSlots[temp].UpdateSlot(new Item(itemObject), 1);
            slotsOnInterface[shopList.GetSlots[temp]].itemCostTxt.SetText("$" + newList[i].price.ToString());
        }


    }
   



    #region 아이템 리스트Class
    public class H_Shop_items
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
    }


    public class H_Shop_results
    {

        public List<H_Shop_items> clothesList { get; set; }
        public List<H_Shop_items> bundleList { get; set; }

    }

    public class H_Shop_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public H_Shop_results results { get; set; }
    }
    #endregion

}
