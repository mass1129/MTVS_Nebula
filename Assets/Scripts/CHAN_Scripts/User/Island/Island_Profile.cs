using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine.EventSystems;

public class Island_Profile : MonoBehaviourPun
{
    //이미지 파일을 가져와서  섬에 띄우도록 한다.
    public string user_name;
    public string user_IslandID;
    public string user_Url;
    public string user_keyword1;
    public string user_keyword2;
    public Image profileImage;
    Text userName_Text;
    Transform playerPos;
    bool turn;
    void Start()
    {
        profileImage.enabled = false;
        playerPos = CHAN_PlayerManger.LocalPlayerInstance.transform.GetChild(5).gameObject.transform;
        userName_Text = gameObject.transform.GetComponentInChildren<Text>();
        //섬의 크기에따라 이미지의 위치를 조정한다. 
        playerPos.transform.GetChild(1).position = new Vector3(0, playerPos.transform.GetChild(1).position.y*playerPos.transform.GetChild(0).localScale.y, 0);
        //섬의 아이콘 색을 조정한다.
        LoadImage();
    }
    private void Update()
    {
        //트리거 발동됐을 때 이미지 보이게 하는 부분
        if (!turn)
            return;
        ShowImage();
    }

    #region 이것은 CSV타입
    async void LoadImage()
    {
        while (Island_Information.instance.Island_Dic == null)
        {
            await Task.Yield();
        }
        GetTexture(user_Url);
        userName_Text.text = user_name;
        userName_Text.enabled = false;
    }
    #endregion
    async void GetTexture(string url)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        var operation = www.SendWebRequest();
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
            if (other.gameObject.GetComponent<PhotonView>().IsMine)
            { 
                print("감지");
                profileImage.enabled = true;
                userName_Text.enabled = true;
                turn = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PhotonView>().IsMine)
            { 
            profileImage.enabled = false;
            userName_Text.enabled = false;
            turn = false;
               
            
            }
        }
    }
    void ShowImage()
    {
        profileImage.transform.LookAt(playerPos);
    }

}
