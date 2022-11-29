using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UltimateClean;
using UnityEngine.UI;

public class K_StaticInterface_ShopList : K_UserInterface
{
    public InventoryObject clothesInven;
    public InventoryObject bbinven;
    public K_MoneySystem moneySystem;
    public GameObject[] subSlots;

    public TextMeshProUGUI[] itemCostTxt;
    public TextMeshProUGUI[] itemNameTxt;
    public Button[] buyButton;
    bool isAdded = false;
    public GameObject[] toolTip;
    
    public override void CreateSlots()
    {

        if (!photonView.IsMine) return;
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = subSlots[i];

            if (!isAdded&&!onQuickSlot)
            {
                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
                
                //AddEvent(obj, EventTriggerType.Select, delegate { OnSelect(obj); });
                //AddEvent(obj, EventTriggerType.Deselect, delegate { OnDeselect(obj); });
            }
            
            inventory.GetSlots[i].slotDisplay = obj;
            //inventory.GetSlots[i].parent = this;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);

        }
        isAdded = true;

        ShopItemLoad();
    }
    public override void ShopUpdate()
    {
        base.ShopUpdate();
        ShopItemLoad();
    }
    private void TryBuyItem(ItemObject item)
    {
        if (!photonView.IsMine) return;
        Item _item = new Item(item);
        Debug.Log(_item.name);
        switch (item.type)
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
                BuyClothesEvent(item);
                break;
            case ItemType.Bundle_Building:
                BuyBBundleEvent(item);
                break;

        }

                
        //if (shopCustomer.TrySpendGoldAmount(i))
        //{
        //    // Can afford cost
        //    shopCustomer.BoughtItem(item);
        //}
        //else
        //{
        //    //Tooltip_Warning.ShowTooltip_Static("Cannot afford " + Item.GetCost(itemType) + "!");
        //    Debug.Log("Cannot afford" + item.name);
        //}
    }

    public async void BuyClothesEvent(ItemObject item)
    {
        if (!photonView.IsMine) return;
        H_Buy_Clothes buyClothes = new H_Buy_Clothes
        {
            clothesName = item.name,
        };

        string json = JsonUtility.ToJson(buyClothes, true);
        Debug.Log(json);
        var url = "https://resource.mtvs-nebula.com/purchase/clothes/" + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());


        H_CheckBuy_Root result = await httpReq.Post1<H_CheckBuy_Root>(url, json);
        //string json2 = JsonUtility.ToJson(result);
        if (result.httpStatus == 201)
        {
            clothesInven.TestLoad();
            moneySystem.MoneyLoad();
            
        }
        else
        {
            Debug.Log("FailToBuy");
        }
    }
    
    public async void BuyBBundleEvent(ItemObject item)
    {
        if (!photonView.IsMine) return;
        H_Buy_Building buyBB = new H_Buy_Building
        {
            buildingBundleName = item.name,
        };

        string json = JsonUtility.ToJson(buyBB, true);
        Debug.Log(json);
        var url = "https://resource.mtvs-nebula.com/purchase/building-bundle/" + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());


        H_CheckBuy_Root result = await httpReq.Post1<H_CheckBuy_Root>(url, json);
        //string json2 = JsonUtility.ToJson(result);
        if (result.httpStatus == 201)
        {
            bbinven.TestLoad();
            moneySystem.MoneyLoad();
            ShopItemLoad();
        }
        else
        {
            Debug.Log("FailToBuy");
        }

    }
   


    public override void DistorySlots()
    {
        if (!photonView.IsMine) return;
        foreach (var key in slotsOnInterface.Keys.ToList())
        {
            Destroy(key);
        }
        slotsOnInterface.Clear();
    }

    public async void ShopItemLoad()
    {
        if (!photonView.IsMine) return;
        var url = "https://resource.mtvs-nebula.com/" + inventory.loadPath + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Shop_Root result2 = await httpReq.Get<H_Shop_Root>(url);

        List<H_Shop_items> newList = result2.results.clothesList;
        newList.AddRange(result2.results.bundleList);

        

        for (int i = 0; i < newList.Count; i++)
        {
            Item item = new Item(inventory.database.ItemObjects[newList[i].id]);
            inventory.AddItem(item, 1);
            itemCostTxt[i].SetText("$" + newList[i].price.ToString());
            itemNameTxt[i].SetText(newList[i].name.ToString());
            toolTip[i].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().SetText(newList[i].name.ToString());
            toolTip[i].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().SetText(inventory.database.ItemObjects[newList[i].id].description);
            int temp = i;
            buyButton[temp].onClick.AddListener(() => TryBuyItem(inventory.database.ItemObjects[newList[temp].id]));
            //Container.slots[i].UpdateSlot(Container.slots[i].item, Container.slots[i].amount);
        }
        inventory.UpdateInventory();


    }


    [System.Serializable]
    public class H_CheckBuy_Root
    {
        public int httpStatus;
        public string message;
    }


    #region 아이템 구매 Class
    [System.Serializable]
    public class H_Buy_Building
    {
        public string buildingBundleName;
    }

    [System.Serializable]
    public class H_Buy_Clothes
    {
        public string clothesName;
    }
    #endregion



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
