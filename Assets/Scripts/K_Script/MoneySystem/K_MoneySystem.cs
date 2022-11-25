using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class K_MoneySystem : MonoBehaviourPun, IShopCustomer
{
    public event EventHandler OnGoldAmountChanged;
    [SerializeField]
    private int goldAmount;
    public K_01_Character player;
    void Awake()
    {
        if(!photonView.IsMine) this.enabled = false;
    }

    public void AddGoldAmount(int addGoldAmount)
    {
        if (!photonView.IsMine) return;
            goldAmount += addGoldAmount;
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("currentGold :" + goldAmount);
    }
    public int GetGoldAmount()
    {
       
            Debug.Log("currentGold :" + goldAmount);
            return goldAmount;
        
        
    }
    public void UpdateMoney()
    {
        if (!photonView.IsMine) return;
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
        var url = "https://resource.mtvs-nebula.com/inventory/money/" + player.avatarName;
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
