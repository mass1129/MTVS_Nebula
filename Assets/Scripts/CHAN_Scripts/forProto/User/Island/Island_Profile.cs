using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Island_Profile : MonoBehaviour
{
    //이미지 파일을 가져와서  섬에 띄우도록 한다.
    float visualDistance =300;
    public string user_name;
    Image profileImage;
    Text userName_Text;
    Transform camPos;
    //Transform player;
    bool turn;
    void Start()
    {
        //player = GameObject.Find("Player").transform;
        profileImage = transform.GetChild(0).GetComponent<Image>();
        profileImage.enabled = false;
        camPos = Camera.main.transform;
        userName_Text = gameObject.transform.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!turn)
        { 
            LoadImage();
           //LoadImageByJson();
            turn = true;
        }
        if (visualDistance > GetDistanceToPlayer())
        {
            profileImage.enabled = true;
            userName_Text.enabled = true;
            transform.LookAt(camPos.position);
            //profileImage.transform.LookAt(camPos.position);
        }
        else
        {
            profileImage.enabled = false;
            userName_Text.enabled = false;
            //이미지를 끈다.
        }
    }
    float GetDistanceToPlayer()
    {
        float dis;
        dis = Vector3.Distance(transform.position, camPos.position);
        return dis;
    }

    #region 이것은 CSV타입
    void LoadImage()
    {
        JsonInfo JInfo = IslandInformation.instance.Island_Dic[user_name];
        StartCoroutine(GetTexture(JInfo.User_image));
        userName_Text.text = "UserName_" + user_name;
        userName_Text.enabled = false;
    }
    //void LoadImageByJson()
    //{
    //    JsonInfo JInfo = LoadJson.instance.Island_Dic[user_name];
    //    StartCoroutine(GetTexture(JInfo.User_image));
    //    userName_Text.text = "UserName_" + user_name;
    //    userName_Text.enabled = false;
    //}
    #endregion
    // 이 코루틴은 한번만 사용되어야 한다. 
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            profileImage.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), Vector2.zero);
        }

    }


}
