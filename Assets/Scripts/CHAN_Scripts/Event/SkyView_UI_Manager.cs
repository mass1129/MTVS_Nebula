using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ������ �������� UI�� �����ϴ� ��ũ��Ʈ
/// </summary>
class Friend_Info
{
    public string friendImage;
    public string friendKeyword;
}
public class SkyView_UI_Manager : MonoBehaviour
{
    public static SkyView_UI_Manager instance;
    private void Awake()
    {
        instance = this;
    }
    // ģ������Ʈ Ȯ�� ��ư
    public GameObject btn_Freinds;
    // ���ã�� ��ư
    public GameObject btn_Favorites;
    // ���� �ϴü����� ���ư��� ��ư
    public GameObject btn_MyWorld;
    // ģ�� ����Ʈ 
    public Transform  Area_FriendsList;
    // ģ�� ���� ����Ʈ 
    public Transform  Area_Friend_info;
    public Transform Area_Friend_Profile;
    public GameObject Btn_Obj;
    public Transform Create_Location;

    GameObject cur_Btn;
    List<string> Friend_Nicknames = new List<string>();
    //Friend_Info f_info = new Friend_Info();
    Dictionary<string, Friend_Info> Dic_Friend = new Dictionary<string, Friend_Info>();
    private void Start()
    {
        
        Initialize();
    }
    // ó���� ģ�������� ������ ��� �����´ٰ� ���� 
    // ��ư�� ����� �Լ��� ������ 
    void MakeFriendsBtn()
    {
        for (int i = 0; i < Dic_Friend.Count; i++)
        {
            GameObject btn = Instantiate(Btn_Obj, Create_Location);
            //��ư�� �ؽ�Ʈ�� �ٲ۴�. 
            btn.GetComponent<Btn_FriendInfo>().image = Dic_Friend[Friend_Nicknames[i]].friendImage;
            btn.GetComponent<Btn_FriendInfo>().keyword = Dic_Friend[Friend_Nicknames[i]].friendKeyword; 
            btn.GetComponent<Btn_FriendInfo>().NickName = Friend_Nicknames[i];
        }
    }
    // �ʱ�ȭ ���
    public void Initialize()
    {
        //�ϴü� ���ư��� ��ư Ȱ��ȭ
        btn_MyWorld.SetActive(true);
        // ���ã�� ��ư
        btn_Favorites.SetActive(true);
        // ģ�� ��ư Ȱ��ȭ
        btn_Freinds.SetActive(true);
        // ģ������Ʈ ��Ȱ��ȭ
        Area_FriendsList.gameObject.SetActive(false);
        // ģ�� ���� ��Ȱ��ȭ
        Area_Friend_info.gameObject.SetActive(false);
        Area_Friend_Profile.gameObject.SetActive(false);
    }
    // ���� �ϴü����� ���ư��� ���
    public void Btn_BackToMyWorld()
    {
        //����� single play ���ؼ� �������� �� ��Ʈ��ũ �߰��� �� �ٲ�� �� ������
        CHAN_GameManager.instance.Go_User_Scene(CHAN_GameManager.instance.nick);
    }
    // ���ã�� ��� ������ ���
    public void Btn_FavoriteList()
    { 
        //���� ������ �־�� ������ ����
    }
    #region
    // ģ������Ʈ ��� ������ �ϴ� ���
    public void Btn_FriendLists()
    {
        Dic_Friend.Clear();
        foreach (string i in Island_Information.instance.Island_Dic.Keys)
        {
            
            JsonInfo info = Island_Information.instance.Island_Dic[i];
            Friend_Info f_info=new Friend_Info();
            Dic_Friend.Add(info.User_NickName, f_info);
            Friend_Nicknames.Add(info.User_NickName);
            f_info.friendImage = info.User_image;
            f_info.friendKeyword = info.island_Type;
            Dic_Friend[info.User_NickName] = f_info;
        }
        MakeFriendsBtn();
        //ģ�� ��ư ��Ȱ��ȭ
        btn_Freinds.SetActive(false);
        //ģ������Ʈ Ȱ��ȭ
        Area_FriendsList.gameObject.SetActive(true);

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

    public void Btn_CloseFriendProfile()
    {
        Area_Friend_Profile.gameObject.SetActive(false);
    }



    #endregion

  



}
