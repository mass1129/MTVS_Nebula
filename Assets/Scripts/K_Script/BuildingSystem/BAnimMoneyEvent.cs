using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BAnimMoneyEvent : BuildingAnimation
{
    public GameObject winEffect;
    public GameObject loseEffect;
    public Transform effectPos;

    
    public void ME()
    {
        MoneyEvent().Forget();
    }

    public async UniTaskVoid MoneyEvent(bool ishttp = false)
    {
        if (ishttp)
        {
            BuildingMoney money = new BuildingMoney
            {
                id = int.Parse(PlayerPrefs.GetString("User_Island_ID"))
            };

            string json = JsonUtility.ToJson(money, true);
            var url = "https://resource.mtvs-nebula.com/money/" + CHAN_GameManager.instance.avatarName;
            var httpReq = new HttpRequester(new JsonSerializationOption());

            H_BM_Root result = await httpReq.Post1<H_BM_Root>(url, json);
            if (result.httpStatus == 201)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Failed");
            }
        }
        else
        {
            int x = Random.Range(0, 100);
            if (x < 30)
            {
                GameObject obj = Instantiate(winEffect, effectPos.position, Quaternion.identity);
                Destroy(obj, 2);
            }
            else
            {
                GameObject obj = Instantiate(loseEffect, effectPos.position, Quaternion.identity);
                Destroy(obj, 2);
            }
        }

    }

    [System.Serializable]
    public class BuildingMoney
    {
        public int id;
    }
    public class H_BM_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
    }

}
