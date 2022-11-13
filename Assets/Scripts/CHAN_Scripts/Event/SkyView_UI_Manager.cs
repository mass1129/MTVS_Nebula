using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject Btn_Obj;
    public Transform Create_Location;

    private void Start()
    {
        
        Initialize();
    }
    // 처음에 친구정보를 서버에 모두 가져온다고 가정 
    // 버튼을 만드는 함수를 만들자 
    void MakeFriendsBtn()
    {
        foreach(string key in Island_Information.instance.Island_Dic.Keys)
        {
            GameObject btn = Instantiate(Btn_Obj, Create_Location);
            //버튼의 텍스트를 바꾼다. 
            btn.GetComponent<Btn_FriendInfo>().image = Island_Information.instance.Island_Dic[key].User_image;
            btn.GetComponent<Btn_FriendInfo>().NickName = Island_Information.instance.Island_Dic[key].User_NickName;
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
    }
    // 나의 하늘섬으로 돌아가는 기능
    public void Btn_BackToMyWorld()
    {
        //현재는 single play 위해서 설정했을 뿐 네트워크 추가할 시 바꿔야 할 사항임
        CHAN_GameManager.instance.Go_User_Scene(CHAN_GameManager.instance.nick);
    }

    #region
    // 친구리스트 목록 나오게 하는 기능
    public void Btn_Menu()
    {
        MakeFriendsBtn();
        //친구 버튼 비활성화
        btn_Freinds.SetActive(false);
        //친구리스트 활성화
        Area_FriendsList.gameObject.SetActive(!Area_FriendsList.gameObject.activeSelf);
        btn_MyWorld.SetActive(!btn_MyWorld.activeSelf);

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

    public void Btn_CloseFriendProfile()
    {
        Area_Friend_Profile.gameObject.SetActive(false);
    }
    #endregion





}
