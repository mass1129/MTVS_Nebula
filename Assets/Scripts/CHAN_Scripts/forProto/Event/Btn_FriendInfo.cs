using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Btn_FriendInfo : MonoBehaviour
{
    public string image;
    public string keyword;
    public string NickName;
    Texture texture;
    GameObject Area_Friend_Profile;
    void Start()
    {
        transform.GetChild(0).GetComponent<Text>().text = NickName;
        GetComponent<Button>().onClick.AddListener(Btn_OnClickFriend);
        SkyView_UI_Manager.instance.Area_Friend_Profile.gameObject.SetActive(true);
        Area_Friend_Profile = GameObject.Find("Area_Friend_Profile");
        SkyView_UI_Manager.instance.Area_Friend_Profile.gameObject.SetActive(false);
        LoadImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Btn_OnClickFriend()
    {
        //친구 버튼 클릭하면 친구 정보 나오도록 한다. 
        SkyView_UI_Manager.instance.Area_Friend_info.gameObject.SetActive(true);
        
        ShowProfile();
        SkyView_UI_Manager.instance.Area_Friend_Profile.gameObject.SetActive(false);
    }
    public void ShowProfile()
    {
        Area_Friend_Profile.transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        Area_Friend_Profile.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = keyword;
    }

    void LoadImage()
    {
        StartCoroutine(GetTexture(image));
    }
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
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            texture = myTexture;
        }
    }
    void ClearInfo()
    {
        Area_Friend_Profile.transform.GetChild(0).GetComponent<RawImage>().texture = null;
        Area_Friend_Profile.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = null;
    }
}
