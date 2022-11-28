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
/// ������ �������� UI�� �����ϴ� ��ũ��Ʈ
/// </summary>

public class SkyView_UI_Manager : MonoBehaviour
{
    public static SkyView_UI_Manager instance;
    private void Awake()
    {
        instance = this;
    }
    // ģ������Ʈ Ȯ�� ��ư
    public GameObject btn_Freinds;
    // ���� �ϴü����� ���ư��� ��ư
    public GameObject btn_MyWorld;
    // ģ�� ����Ʈ 
    public Transform  Area_FriendsList;
    // ģ�� ���� ����Ʈ 
    public Transform  Area_Friend_info;
    public Transform Area_Friend_Profile;
    public GameObject Btn_Obj_Online;
    public GameObject Btn_Obj_Offline;
    public GameObject btn_JoinTogether;

    public Transform online_Location;
    public Transform offline_Location;
    // ���õ� ģ���� ����
    public string SelectedFriendName;
    public string SelectedFriendIslandId;
    // ���� ģ���� ���ִ� ģ������ ����
    public string friendName_ForJoin;
    public string friendIslandId_ForJoin;

    private void Start()
    {
        Initialize();
    }
    // ó���� ģ�������� ������ ��� �����´ٰ� ���� 
    // ��ư�� ����� �Լ��� ������ 
    void MakeFriendsBtn()
    {
        //�ʱ⿡ contents�� �ִ� ��ҵ��� ��� �����. 
        DestroyElement(online_Location);
        DestroyElement(offline_Location);
        Island_Information iInfo = Island_Information.instance;
        //������ ��� ������ ��ŭ �ݺ�
        foreach (string key in iInfo.Island_Dic.Keys)
        {
            GameObject btn = null;
            //���� key���� �����̸��� �¶��� ����Ʈ�� ���� ���
                if (CUI.onlineUsers.ContainsKey(Island_Information.instance.Island_Dic[key].User_NickName))
                { 
                    btn = Instantiate(Btn_Obj_Online, online_Location); 
                }
                else
                { 
                    btn = Instantiate(Btn_Obj_Offline, offline_Location); 
                }
            Btn_FriendInfo fInfo = btn.GetComponent<Btn_FriendInfo>();

            //��ư�� �ؽ�Ʈ�� �ٲ۴�. 
            fInfo.image = iInfo.Island_Dic[key].User_image;
            fInfo.islandId = iInfo.Island_Dic[key].User_IslandId;
            fInfo.NickName = iInfo.Island_Dic[key].User_NickName;
            fInfo.keyword1 = iInfo.Island_Dic[key].User_IslandKeyword1;
            fInfo.keyword2 = iInfo.Island_Dic[key].User_IslandKeyword2;
            fInfo.keyword3 = iInfo.Island_Dic[key].User_IslandKeyword3;
            fInfo.keyword4 = iInfo.Island_Dic[key].User_IslandKeyword4;
            fInfo.followers = iInfo.Island_Dic[key].User_followers;

            //--------------------------------> ���⿡ ���� Ű���尪 �߰� �ؾ� ��.
        }
    }
    void DestroyElement(Transform contents)
    {
        for (int i = 0; i < contents.childCount; i++)
        {
            Destroy(contents.GetChild(i).gameObject);
        }
    }
    // �ʱ�ȭ ���
    public void Initialize()
    {
        //�ϴü� ���ư��� ��ư Ȱ��ȭ
        btn_MyWorld.SetActive(true);
        // ģ�� ��ư Ȱ��ȭ
        btn_Freinds.SetActive(true);
        // ģ������Ʈ ��Ȱ��ȭ
        Area_FriendsList.gameObject.SetActive(false);
        // ģ�� ���� ��Ȱ��ȭ
        Area_Friend_info.gameObject.SetActive(false);
        Area_Friend_Profile.gameObject.SetActive(false);
        btn_JoinTogether.SetActive(false);
    }
    // ���� �ϴü����� ���ư��� ���
    public void Btn_BackToMyWorld()
    {
        //����� single play ���ؼ� �������� �� ��Ʈ��ũ �߰��� �� �ٲ�� �� ������
        CHAN_GameManager.instance.Go_User_Scene(PlayerPrefs.GetString("AvatarName"));
        PlayerPrefs.SetString("User_Island_ID", PlayerPrefs.GetString("Island_ID"));
    }

    #region
    // ģ������Ʈ ��� ������ �ϴ� ���
    public void Btn_Menu()
    {
        MakeFriendsBtn();
        //ģ�� ��ư ��Ȱ��ȭ
        //btn_Freinds.SetActive(false);
        //ģ������Ʈ Ȱ��ȭ
        Area_FriendsList.gameObject.SetActive(!Area_FriendsList.gameObject.activeSelf);
        //btn_MyWorld.SetActive(!btn_MyWorld.activeSelf);

    }
    public void Btn_FriendProfile()
    {
        Area_Friend_Profile.gameObject.SetActive(true);
    }
    //�ڷΰ��� ���
    public void Btn_CloseFriendList()
    {
        //ģ�� ��ư Ȱ��ȭ
        btn_Freinds.SetActive(true);
        //ģ������Ʈ ��Ȱ��ȭ
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
    //���� ģ���� �ִ� ������ ����� ���
    public void Btn_JoinTogether()
    {
        PlayerPrefs.SetString("User_Island_ID", friendIslandId_ForJoin);
        Debug.Log("����� ����: "+PlayerPrefs.GetString("User_Island_ID"));
        CHAN_GameManager.instance.Go_User_Scene(friendName_ForJoin);
    }
    #endregion
    //ģ���� �����
    public void Go_FriendIsland()
    {
        PlayerPrefs.SetString("User_Island_ID", SelectedFriendIslandId);
        CHAN_GameManager.instance.Go_User_Scene(SelectedFriendName);
    }




}
