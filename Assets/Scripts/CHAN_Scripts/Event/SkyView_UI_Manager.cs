using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using User_Info;
using CUI = User_Info.CHAN_Users_Info;

/// <summary>
/// 월드의 전반적인 UI를 제어하는 스크립트
/// </summary>

public class SkyView_UI_Manager : MonoBehaviour
{
    public static SkyView_UI_Manager instance;
    private void Awake()
    {
        instance = this;
    }
    // 친구리스트 확인 버튼
    public GameObject btn_Freinds;
    // 나의 하늘섬으로 돌아가기 버튼
    public GameObject btn_MyWorld;
    // 친구 리스트 
    public Transform  Area_FriendsList;
    // 친구 정보 리스트 
    public Transform  Area_Friend_info;
    public Transform Area_Friend_Profile;
    public GameObject Btn_Obj_Online;
    public GameObject Btn_Obj_Offline;
    public GameObject btn_JoinTogether;

    public Transform online_Location;
    public Transform offline_Location;
    // 선택된 친구의 정보
    public string SelectedFriendName;
    public string SelectedFriendIslandId;
    // 현재 친구가 들어가있는 친구섬의 정보
    public string friendName_ForJoin;
    public string friendIslandId_ForJoin;

    private void Start()
    {
        Initialize();
    }
    // 처음에 친구정보를 서버에 모두 가져온다고 가정 
    // 버튼을 만드는 함수를 만들자 
    void MakeFriendsBtn()
    {
        //초기에 contents에 있는 요소들을 모두 지운다. 
        DestroyElement(online_Location);
        DestroyElement(offline_Location);
        Island_Information iInfo = Island_Information.instance;
        //서버내 모든 유저수 만큼 반복
        foreach (string key in iInfo.Island_Dic.Keys)
        {
            GameObject btn = null;
            //만약 key값의 유저이름이 온라인 리스트에 있을 경우
                if (CUI.onlineUsers.ContainsKey(Island_Information.instance.Island_Dic[key].User_NickName))
                { 
                    btn = Instantiate(Btn_Obj_Online, online_Location); 
                }
                else
                { 
                    btn = Instantiate(Btn_Obj_Offline, offline_Location); 
                }
            Btn_FriendInfo fInfo = btn.GetComponent<Btn_FriendInfo>();

            //버튼의 텍스트를 바꾼다. 
            fInfo.image = iInfo.Island_Dic[key].User_image;
            fInfo.islandId = iInfo.Island_Dic[key].User_IslandId;
            fInfo.NickName = iInfo.Island_Dic[key].User_NickName;
            fInfo.keyword1 = iInfo.Island_Dic[key].User_IslandKeyword1;
            fInfo.keyword2 = iInfo.Island_Dic[key].User_IslandKeyword2;
            fInfo.keyword3 = iInfo.Island_Dic[key].User_IslandKeyword3;
            fInfo.keyword4 = iInfo.Island_Dic[key].User_IslandKeyword4;
            fInfo.followers = iInfo.Island_Dic[key].User_followers;

            //--------------------------------> 여기에 유저 키워드값 추가 해야 함.
        }
    }
    void DestroyElement(Transform contents)
    {
        for (int i = 0; i < contents.childCount; i++)
        {
            Destroy(contents.GetChild(i).gameObject);
        }
    }
    // 초기화 기능
    public void Initialize()
    {
        //하늘섬 돌아가기 버튼 활성화
        btn_MyWorld.SetActive(true);
        // 친구 버튼 활성화
        btn_Freinds.SetActive(true);
        // 친구리스트 비활성화
        Area_FriendsList.gameObject.SetActive(false);
        // 친구 정보 비활성화
        Area_Friend_info.gameObject.SetActive(false);
        Area_Friend_Profile.gameObject.SetActive(false);
        btn_JoinTogether.SetActive(false);
    }
    // 나의 하늘섬으로 돌아가는 기능
    public void Btn_BackToMyWorld()
    {
        //현재는 single play 위해서 설정했을 뿐 네트워크 추가할 시 바꿔야 할 사항임
        CHAN_GameManager.instance.Go_User_Scene(PlayerPrefs.GetString("AvatarName"));
        PlayerPrefs.SetString("User_Island_ID", PlayerPrefs.GetString("Island_ID"));
    }

    #region
    // 친구리스트 목록 나오게 하는 기능
    public void Btn_Menu()
    {
        MakeFriendsBtn();
        //친구 버튼 비활성화
        //btn_Freinds.SetActive(false);
        //친구리스트 활성화
        Area_FriendsList.gameObject.SetActive(!Area_FriendsList.gameObject.activeSelf);
        //btn_MyWorld.SetActive(!btn_MyWorld.activeSelf);

    }
    public void Btn_FriendProfile()
    {
        Area_Friend_Profile.gameObject.SetActive(true);
    }
    //뒤로가기 기능
    public void Btn_CloseFriendList()
    {
        //친구 버튼 활성화
        btn_Freinds.SetActive(true);
        //친구리스트 비활성화
        Area_FriendsList.gameObject.SetActive(false);
        Area_Friend_info.gameObject.SetActive(false);
        Area_Friend_Profile.gameObject.SetActive(false);
    }
    public void Btn_CloseFriendInfo()
    {
        Area_Friend_info.gameObject.SetActive(!Area_Friend_info.gameObject.activeSelf);
        btn_JoinTogether.SetActive(false);
    }
    public void Btn_CloseFriendProfile()
    {
        Area_Friend_Profile.gameObject.SetActive(false);
    }
    public void Btn_GoUserProfile()
    {
        CHAN_GameManager.instance.Btn_GoProfile();
    }
    //현재 친구가 있는 섬으로 놀러가는 기능
    public void Btn_JoinTogether()
    {
        PlayerPrefs.SetString("User_Island_ID", friendIslandId_ForJoin);
        Debug.Log("놀러가기 입장: "+PlayerPrefs.GetString("User_Island_ID"));
        CHAN_GameManager.instance.Go_User_Scene(friendName_ForJoin);
    }
    #endregion
    //친구섬 놀러가기
    public void Go_FriendIsland()
    {
        PlayerPrefs.SetString("User_Island_ID", SelectedFriendIslandId);
        CHAN_GameManager.instance.Go_User_Scene(SelectedFriendName);
    }




}
