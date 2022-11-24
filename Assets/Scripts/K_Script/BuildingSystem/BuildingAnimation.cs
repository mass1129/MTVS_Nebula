using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;
using CodeMonkey.Utils;

public class BuildingAnimation : MonoBehaviourPun {

    public AnimationCurve animationCurve = null;

    private float time;

    public Transform toRotateObj = null;

    public Vector3 toRotateV3;
    public GameObject winEffect;
    public GameObject loseEffect;
    public Transform effectPos;
    public Transform textPos;
    public bool moneyFactory = false;
    private void OnEnable()
    {

    }

    private void Update() {
        time += Time.deltaTime;

        transform.localScale = new Vector3(1, animationCurve.Evaluate(time), 1);
        if (toRotateObj != null)
            toRotateObj.localEulerAngles = toRotateV3 * time;

    }



    public async void MoneyEvent()
    {
        BuildingMoney money = new BuildingMoney
        {
            id = int.Parse(PlayerPrefs.GetString("User_Island_ID"))
        };

        string json = JsonUtility.ToJson(money, true);
        Debug.Log(json);
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/money/" + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_BM_Root result = await httpReq.Post1<H_BM_Root>(url, json);
        string json2 = JsonUtility.ToJson(result);
        if (result.httpStatus == 201)
        {
            UtilsClass.CreateWorldTextPopup(this.transform, json2, textPos.localPosition, 10, Color.white, textPos.localPosition + new Vector3(0, 2), 1f);
            GameObject obj = Instantiate(winEffect, effectPos.position, Quaternion.identity);
            Destroy(obj, 2);
        }
        else
        {
            UtilsClass.CreateWorldTextPopup(this.transform, json2, textPos.localPosition, 10, Color.red, textPos.localPosition + new Vector3(0, 2), 1f);
            GameObject obj = Instantiate(loseEffect, effectPos.position, Quaternion.identity) ;
            Destroy(obj, 2);
        }
        
    }

    [System.Serializable]
    public class BuildingMoney
    {
        public int id;
    }
    [System.Serializable]
    public class H_BM_Root
    {
        public int httpStatus;
        public string message;
    }


}
