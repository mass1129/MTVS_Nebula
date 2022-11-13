using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ������ �������� UI�� �����ϴ� ��ũ��Ʈ
/// </summary>
class Friend_Info
{
    public string friendImage;
    public string nickName;
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
    // ���� �ϴü����� ���ư��� ��ư
    public GameObject btn_MyWorld;
    // ģ�� ����Ʈ 
    public Transform  Area_FriendsList;
    // ģ�� ���� ����Ʈ 
    public Transform  Area_Friend_info;
    public Transform Area_Friend_Profile;
    public GameObject Btn_Obj;
    public Transform Create_Location;
    Image FriendImage;

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
        foreach(string key in Island_Information.instance.Island_Dic.Keys)
        {
            GameObject btn = Instantiate(Btn_Obj, Create_Location);
            //��ư�� �ؽ�Ʈ�� �ٲ۴�. 
            btn.GetComponent<Btn_FriendInfo>().image = Island_Information.instance.Island_Dic[key].User_image;
            btn.GetComponent<Btn_FriendInfo>().NickName = Island_Information.instance.Island_Dic[key].User_NickName;
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
    }
    // ���� �ϴü����� ���ư��� ���
    public void Btn_BackToMyWorld()
    {
        //����� single play ���ؼ� �������� �� ��Ʈ��ũ �߰��� �� �ٲ�� �� ������
        CHAN_GameManager.instance.Go_User_Scene(CHAN_GameManager.instance.nick);
    }

    #region
    // ģ������Ʈ ��� ������ �ϴ� ���
    public void Btn_Menu()
    {
        //Dic_Friend.Clear();
        //foreach (string i in Island_Information.instance.Island_Dic.Keys)
        //{
        //    Friend_Info f_info=new Friend_Info();
        //    Dic_Friend.Add(i, f_info);
        //    f_info.friendImage = Island_Information.instance.Island_Dic[i].User_image;
        //    f_info.nickName = Island_Information.instance.Island_Dic[i].User_NickName;
        //    Dic_Friend[i] = f_info;
        //}
        MakeFriendsBtn();
        //ģ�� ��ư ��Ȱ��ȭ
        btn_Freinds.SetActive(false);
        //ģ������Ʈ Ȱ��ȭ
        Area_FriendsList.gameObject.SetActive(!Area_FriendsList.gameObject.activeSelf);
        btn_MyWorld.SetActive(!btn_MyWorld.activeSelf);

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
