using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine.EventSystems;
using TMPro;

public class Island_Profile : MonoBehaviourPun
{
    //�̹��� ������ �����ͼ�  ���� ��쵵�� �Ѵ�.
    public string user_name;
    public string user_IslandID;
    public string user_Url;
    public string user_keyword1;
    public string user_keyword2;
    public Image areaImage;
    TMP_Text userName_Text;
    Transform playerPos;
    public bool turn;
    void Start()
    {
        userName_Text = gameObject.transform.GetComponentInChildren<TMP_Text>();
        playerPos = CHAN_PlayerManger.LocalPlayerInstance.transform.GetChild(5).gameObject.transform;
        areaImage.enabled=false;
        //���� ũ�⿡���� �̹����� ��ġ�� �����Ѵ�. 
        playerPos.transform.GetChild(1).position = new Vector3(0, playerPos.transform.GetChild(1).position.y*playerPos.transform.GetChild(0).localScale.y, 0);
        //���� ������ ���� �����Ѵ�.
        LoadImage();
    }


    #region �̰��� CSVŸ��
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
            areaImage.GetComponentInChildren<Image>().sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), Vector2.zero);
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
                print("����");
                areaImage.enabled = true;
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
                areaImage.enabled = false;
                userName_Text.enabled = false;
            turn = false;
            }
        }
    }


}
