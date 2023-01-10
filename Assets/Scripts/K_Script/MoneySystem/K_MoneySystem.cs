using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;


public class K_MoneySystem : MonoBehaviourPun
{
    public event EventHandler OnGoldAmountChanged;
    public int goldAmount { get; private set; }
    K_PlayerItemSystem itemsystem;
    public string avatarName { get; private set; }
    public K_ShopListInterface shopInterface { get; }


    public void MoneySystemSetting(string s)
    {
        avatarName = s;
        itemsystem=GetComponent<K_PlayerItemSystem>();
        StartCoroutine(MoneyCoroutine());
    }

    IEnumerator MoneyCoroutine()
    {
        while (true)
        {
            MoneyLoad().Forget();
            yield return new WaitForSeconds(5f);
        }
    }
    public async UniTaskVoid MoneyLoad()
    {
        //if (!photonView.IsMine) return;
        //var url = "https://resource.mtvs-nebula.com/inventory/money/" + avatarName;
        //var httpReq = new HttpRequester(new JsonSerializationOption());

        //H_Money_Root result2 = await httpReq.Get<H_Money_Root>(url);

        //goldAmount = result2.results.money; 
        //OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
      

    }
    public async UniTaskVoid BuyClothesEvent(ItemObject item)
    {
        //H_Buy_Clothes buyClothes = new H_Buy_Clothes
        //{
        //    clothesName = item.name
        //};

        //string json = JsonUtility.ToJson(buyClothes, true);
        //Debug.Log(json);
        //var url = "https://resource.mtvs-nebula.com/purchase/clothes/" + avatarName;
        //var httpReq = new HttpRequester(new JsonSerializationOption());


        //H_CheckBuy_Root result = await httpReq.Post1<H_CheckBuy_Root>(url, json);
        ////string json2 = JsonUtility.ToJson(result);
        //if (result.httpStatus == 201)
        //{
        //    await itemsystem.inven_Cloths.InventoryLoad(avatarName);
        //    MoneyLoad().Forget();

        //}
        //else
        //{
        //    Debug.Log("FailToBuy");
        //}
    }

    public async UniTaskVoid BuyBBundleEvent(ItemObject item)
    {
        H_Buy_Building buyBB = new H_Buy_Building
        {
            buildingBundleName = item.name
        };

        string json = JsonUtility.ToJson(buyBB, true);
        Debug.Log(json);
        var url = "https://resource.mtvs-nebula.com/purchase/building-bundle/" + avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());


        H_CheckBuy_Root result = await httpReq.Post1<H_CheckBuy_Root>(url, json);

        if (result.httpStatus == 201)
        {
            itemsystem.inven_Building.InventoryLoad(avatarName);
            MoneyLoad().Forget();
            await shopInterface.ShopItemLoad();
        }
        else
        {
            Debug.Log("FailToBuy");
        }

    }

    #region 아이템 구매 Class
    [System.Serializable]
    public class H_CheckBuy_Root
    {
        public int httpStatus;
        public string message;
    }


    
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




    public class H_Money_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }

        public Results results { get; set; }

    }
    public class Results
    {
        public int money { get; set; }
    }


}
