using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
public class K_ShopListInterface : K_UserInterface
{

    public K_MoneySystem moneySystem;
    
    public GameObject shopSlotModuleprefeb;
    public Transform moduleParent;
    public new Dictionary<InventorySlot, ShopSlotModule> slotsOnInterface;


    public override void CreateSlots()
    {
        ShopItemLoad().Forget();
    }

    public override void OnSlotUpdate(InventorySlot slot)
    {
        slotsOnInterface[slot].SetShopModule(slot, inventory);
        slotsOnInterface[slot].buyButton.onClick.AddListener(() => TryBuyItem(slot));

    }


    public void ShopListUpdate()
    {
        ShopItemLoad().Forget();
    }


    private void TryBuyItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.GetShopItemObject(inventory);
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
    


    public async UniTask ShopItemLoad()
    {
        if (!photonView.IsMine) return;
        inventory.Clear();
        var url = "https://resource.mtvs-nebula.com/" + inventory.loadPath + moneySystem.avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Shop_Root result2 = await httpReq.Get<H_Shop_Root>(url);

        List<H_Shop_items> newList = new List<H_Shop_items>();
        newList.AddRange(result2.results.clothesList);
        if(result2.results.bundleList.Count>0)
            newList.AddRange(result2.results.bundleList);


        slotsOnInterface = new Dictionary<InventorySlot, ShopSlotModule>();

        for (int i = 0; i < newList.Count; i++)
        {
            int temp = i;
            var obj = Instantiate(shopSlotModuleprefeb, Vector3.zero, Quaternion.identity, moduleParent);
            slotsOnInterface.Add(inventory.GetSlots[temp], obj.GetComponent<ShopSlotModule>());
            inventory.GetSlots[temp].onAfterUpdated += OnSlotUpdate;
        }

        for (int i = 0; i < newList.Count; i++)
        {
            int temp = i;
            ItemObject itemObject = inventory.database.ItemObjects[newList[temp].id];
            slotsOnInterface[inventory.GetSlots[temp]].itemPrice = newList[temp].price;
            inventory.GetSlots[temp].UpdateSlot(new Item(itemObject), 1);
        }
        await UniTask.Yield();

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
