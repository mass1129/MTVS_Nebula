using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UltimateClean;
using UnityEngine.UI;
using Photon.Pun;

public class K_StaticInterface_ShopList : MonoBehaviourPun
{


    public InventoryObject inventory;
    public InventoryObject clothesInven;
    public InventoryObject bbinven;
    public K_MoneySystem moneySystem;
    

    public TextMeshProUGUI[] itemCostTxt;
    public TextMeshProUGUI[] itemNameTxt;
    public Button[] buyButton;
    public GameObject[] toolTip;
    public Image[] slopImg;
    private void OnEnable()
    {
        if (!photonView.IsMine) return;
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


    private void OnDestroy()
    {
        if (!photonView.IsMine) return;
        inventory.Clear();
    }


    public async void ShopItemLoad()
    {
        if (!photonView.IsMine) return;
        inventory.Clear();
        var url = "https://resource.mtvs-nebula.com/" + inventory.loadPath + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Shop_Root result2 = await httpReq.Get<H_Shop_Root>(url);

        List<H_Shop_items> newList = result2.results.clothesList;
        newList.AddRange(result2.results.bundleList);

        

        for (int i = 0; i < newList.Count; i++)
        {
            int temp = i;
            Item item = new Item(inventory.database.ItemObjects[newList[temp].id]);
            inventory.AddItem(item, 1);
            itemCostTxt[temp].SetText("$" + newList[i].price.ToString());
            itemNameTxt[temp].SetText(newList[i].name.ToString());
            toolTip[temp].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().SetText(newList[temp].name.ToString());
            toolTip[temp].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().SetText(inventory.database.ItemObjects[newList[temp].id].description);
            slopImg[temp].sprite = inventory.database.ItemObjects[newList[temp].id].uiDisplay;
            
            buyButton[temp].onClick.AddListener(() => TryBuyItem(inventory.database.ItemObjects[newList[temp].id]));
            //Container.slots[i].UpdateSlot(Container.slots[i].item, Container.slots[i].amount);
        }
        


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
