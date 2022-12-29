using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cysharp.Threading.Tasks;

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
    private bool firstLoad = false;
    private void OnEnable()
    {
        if (!photonView.IsMine) return;
        ShopItemLoad().Forget();
    }


    private void TryBuyItem(Item item)
    {
        if (!photonView.IsMine) return;
        ItemObject itemObject = inventory.database.ItemObjects[item.id];
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
                BuyClothesEvent(item).Forget();
                break;
            case ItemType.Bundle_Building:
                BuyBBundleEvent(item).Forget();
                break;

        }


    }
    
    public async UniTask BuyClothesEvent(Item item)
    {
        if (!photonView.IsMine) return;
        H_Buy_Clothes buyClothes = new H_Buy_Clothes
        {
            clothesName = item.name
        };

        string json = JsonUtility.ToJson(buyClothes, true);
        Debug.Log(json);
        var url = "https://resource.mtvs-nebula.com/purchase/clothes/" + moneySystem.player.avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());


        H_CheckBuy_Root result = await httpReq.Post1<H_CheckBuy_Root>(url, json);
        //string json2 = JsonUtility.ToJson(result);
        if (result.httpStatus == 201)
        {
            await clothesInven.InventoryLoad(moneySystem.player.avatarName);
            await moneySystem.MoneyLoad();
            
        }
        else
        {
            Debug.Log("FailToBuy");
        }
    }
    
    public async UniTask BuyBBundleEvent(Item item)
    {
        if (!photonView.IsMine) return;
        H_Buy_Building buyBB = new H_Buy_Building
        {
            buildingBundleName = item.name
        };

        string json = JsonUtility.ToJson(buyBB, true);
        Debug.Log(json);
        var url = "https://resource.mtvs-nebula.com/purchase/building-bundle/" + moneySystem.player.avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());


        H_CheckBuy_Root result = await httpReq.Post1<H_CheckBuy_Root>(url, json);
        //string json2 = JsonUtility.ToJson(result);
        if (result.httpStatus == 201)
        {
            await bbinven.InventoryLoad(moneySystem.player.avatarName);
            await moneySystem.MoneyLoad();
            await ShopItemLoad();
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


    public async UniTask ShopItemLoad()
    {
        if (!photonView.IsMine) return;
        inventory.Clear();
        var url = "https://resource.mtvs-nebula.com/" + inventory.loadPath + moneySystem.player.avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Shop_Root result2 = await httpReq.Get<H_Shop_Root>(url);

        List<H_Shop_items> newList = new List<H_Shop_items>();
        newList.AddRange(result2.results.clothesList);
        if(result2.results.bundleList.Count>0)
            newList.Add(result2.results.bundleList[0]);

        

        for (int i = 0; i < newList.Count; i++)
        {
            int temp = i;
            ItemObject itemObject = inventory.database.ItemObjects[newList[temp].id];
            inventory.GetSlots[temp].UpdateSlot(new Item(itemObject), 1);
            itemCostTxt[temp].SetText("$" + newList[i].price.ToString());
            itemNameTxt[temp].SetText(itemObject.name.ToString());
            toolTip[temp].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().SetText(itemObject.name.ToString());
            toolTip[temp].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().SetText(itemObject.description);
            slopImg[temp].sprite = itemObject.uiDisplay;
            if(!firstLoad)
            buyButton[temp].onClick.AddListener(() => TryBuyItem(inventory.GetSlots[temp].item));
            //Container.slots[i].UpdateSlot(Container.slots[i].item, Container.slots[i].amount);
        }
        firstLoad = true;


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
