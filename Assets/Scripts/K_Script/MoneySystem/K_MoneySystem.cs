using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class K_MoneySystem : MonoBehaviour, IShopCustomer
{
    public event EventHandler OnGoldAmountChanged;
    [SerializeField]
    private int goldAmount;

    void Start()
    {
        
    }

    public void AddGoldAmount(int addGoldAmount)
    {
        goldAmount += addGoldAmount;
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("currentGold :" + goldAmount);
    }
    public int GetGoldAmount()
    {   Debug.Log("currentGold :" + goldAmount);
        return goldAmount;
    }
    public void UpdateMoney()
    {
        MoneyLoad();
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("currentGold :" + goldAmount);
    }

    public bool TrySpendGoldAmount(int spendGoldAmount)
    {
        if (GetGoldAmount() >= spendGoldAmount)
        {
            goldAmount -= spendGoldAmount;
            OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BoughItem(ItemObject obj)
    {
        throw new NotImplementedException();
    }



    public async void MoneyLoad()
    {
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/inventory/money/" + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Money_Root result2 = await httpReq.Get<H_Money_Root>(url);

        goldAmount = result2.results.money; 
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
      

    }

   


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
