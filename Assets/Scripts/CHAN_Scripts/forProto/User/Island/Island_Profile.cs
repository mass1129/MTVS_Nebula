using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class Island_Profile : MonoBehaviour
{
    //이미지 파일을 가져와서  섬에 띄우도록 한다.
    public string user_name;
    public Image profileImage;
    Text userName_Text;
    Transform playerPos;
    //Transform player;
    bool turn;
    void Start()
    {
        //profileImage = transform.GetChild(0).GetComponent<Image>();
        profileImage.enabled = false;
        playerPos = CHAN_PlayerManger.LocalPlayerInstance.transform;
        userName_Text = gameObject.transform.GetComponentInChildren<Text>();
        LoadImage();
    }
    private void Update()
    {
        if (!turn)
            return;
        ShowImage();
    }

    #region 이것은 CSV타입
    void LoadImage()
    {
        JsonInfo JInfo = IslandInformation.instance.Island_Dic[user_name];
        GetTexture(JInfo.User_image);
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
    async void GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        var operation= www.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        try
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            profileImage.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), Vector2.zero);
        }
        catch
        {
            Debug.Log(www.error);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("감지");
            profileImage.enabled = true;
            userName_Text.enabled = true;
            turn = true;
        }

    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            profileImage.enabled = false;
            userName_Text.enabled = false;
            turn = false;
        }

    }
    void ShowImage()
    {
        profileImage.transform.LookAt(playerPos);
    }


}
