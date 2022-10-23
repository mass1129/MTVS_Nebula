using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Island_Profile : MonoBehaviour
{
    //�̹��� ������ �����ͼ�  ���� ��쵵�� �Ѵ�.
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
            //�̹����� ����.
        }
    }
    float GetDistanceToPlayer()
    {
        float dis;
        dis = Vector3.Distance(transform.position, camPos.position);
        return dis;
    }

    #region �̰��� CSVŸ��
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
    // �� �ڷ�ƾ�� �ѹ��� ���Ǿ�� �Ѵ�. 
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
