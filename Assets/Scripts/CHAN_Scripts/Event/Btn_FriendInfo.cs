using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using User_Info;
using CUI = User_Info.CHAN_Users_Info;

public class Btn_FriendInfo : MonoBehaviour
{
    public string islandId;
    public string image;
    public string keyword1;
    public string keyword2;
    public string keyword3;
    public string keyword4;
    public int followers;
    public string NickName;
    Texture texture;
    GameObject Area_Friend_Profile;
    void Start()
    {
        //이름을 입력한다. 
        transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = NickName;
        //해당 버튼 기능을 활성화 한다.
        GetComponent<Button>().onClick.AddListener(Btn_OnClickFriend);
        SkyView_UI_Manager.instance.Area_Friend_Profile.gameObject.SetActive(true);
        Area_Friend_Profile = GameObject.Find("Nebula_Profile");
        SkyView_UI_Manager.instance.Area_Friend_Profile.gameObject.SetActive(false);
        LoadImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Btn_OnClickFriend()
    {
        //Friend_info: 친구의 현재 섬의 위치 
        SkyView_UI_Manager.instance.Area_Friend_info.gameObject.SetActive(true);
        //친구의 이름을 텍스트로 출력
        SkyView_UI_Manager.instance.Area_Friend_info.gameObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = NickName;
        // 친구정보에는 그 친구가 현재 어느 섬에 있는지
        //만약 온라인이면
        if (CUI.onlineUsers.ContainsKey(NickName))
        {
            //여기서 현재 누구의 방에 있는지 확인 
            // 만약 유저가 하늘씬에 있다면
            if (CUI.onlineUsers[NickName] == "sky")
            {
                SkyView_UI_Manager.instance.Area_Friend_info.gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = "하늘바다 여행중";
            }
            //다른 유저의 섬에 있다면 같이놀기 버튼 생성
            else
            {
                SkyView_UI_Manager.instance.friendName_ForJoin = CUI.onlineUsers[NickName];
                foreach (string key in Island_Information.instance.Island_Dic.Keys)
                {
                    if (Island_Information.instance.Island_Dic[key].User_NickName == CUI.onlineUsers[NickName])
                    {
                        SkyView_UI_Manager.instance.friendIslandId_ForJoin = Island_Information.instance.Island_Dic[key].User_IslandId;
                        Debug.Log("같이가기: "+Island_Information.instance.Island_Dic[key].User_IslandId);
                        break;
                    }
                }
                SkyView_UI_Manager.instance.btn_JoinTogether.SetActive(true);
            }
        }
        //오프라인이면 오프라인이라고 출력
        else
        {
            SkyView_UI_Manager.instance.Area_Friend_info.gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = "오프라인";
        }
        // 팔로워가 몇명인지 
        //SkyView_UI_Manager.instance.Area_Friend_info.gameObject.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = followers.ToString() + "명";
        ShowProfile();
        SkyView_UI_Manager.instance.Area_Friend_Profile.gameObject.SetActive(false);
    }
    /// <summary>
    /// 유저 프로필 정보를 저장하는 함수
    /// </summary>
    public void ShowProfile()
    {
        //Area_Friend_Profile.transform.GetComponentInChildren<RawImage>().texture = texture;
        // 현재 클릭한 유저이름을 UI매니저에 전달(그 유저의 섬에 놀러가기 위한 목적)
        SkyView_UI_Manager.instance.SelectedFriendName = NickName;
        SkyView_UI_Manager.instance.SelectedFriendIslandId = islandId;
        Debug.Log("check:" + NickName);
        //프로필 정보를 삽입한다.
        Area_Friend_Profile.GetComponent<Profile_Manager_New>().user_Nickname.text = NickName;
        Area_Friend_Profile.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<RawImage>().texture = texture;
        Area_Friend_Profile.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = keyword1;
        Area_Friend_Profile.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = keyword2;
        Area_Friend_Profile.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = keyword3;
        Area_Friend_Profile.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = keyword4;

        //----------> 유저 키워드 추가 부분
        //for (int i = 0; i < 2; i++)
        //{
        //    Area_Friend_Profile.GetComponent<Profile_Manager_New>().keywords[i]=
        //}
        //Area_Friend_Profile.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = NickName+"의 월드";
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
