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
        //�̸��� �Է��Ѵ�. 
        transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = NickName;
        //�ش� ��ư ����� Ȱ��ȭ �Ѵ�.
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
        //Friend_info: ģ���� ���� ���� ��ġ 
        SkyView_UI_Manager.instance.Area_Friend_info.gameObject.SetActive(true);
        //ģ���� �̸��� �ؽ�Ʈ�� ���
        SkyView_UI_Manager.instance.Area_Friend_info.gameObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = NickName;
        // ģ���������� �� ģ���� ���� ��� ���� �ִ���
        //���� �¶����̸�
        if (CUI.onlineUsers.ContainsKey(NickName))
        {
            //���⼭ ���� ������ �濡 �ִ��� Ȯ�� 
            // ���� ������ �ϴþ��� �ִٸ�
            if (CUI.onlineUsers[NickName] == "sky")
            {
                SkyView_UI_Manager.instance.Area_Friend_info.gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = "�ϴùٴ� ������";
            }
            //�ٸ� ������ ���� �ִٸ� ���̳�� ��ư ����
            else
            {
                SkyView_UI_Manager.instance.friendName_ForJoin = CUI.onlineUsers[NickName];
                foreach (string key in Island_Information.instance.Island_Dic.Keys)
                {
                    if (Island_Information.instance.Island_Dic[key].User_NickName == CUI.onlineUsers[NickName])
                    {
                        SkyView_UI_Manager.instance.friendIslandId_ForJoin = Island_Information.instance.Island_Dic[key].User_IslandId;
                        Debug.Log("���̰���: "+Island_Information.instance.Island_Dic[key].User_IslandId);
                        break;
                    }
                }
                SkyView_UI_Manager.instance.btn_JoinTogether.SetActive(true);
            }
        }
        //���������̸� ���������̶�� ���
        else
        {
            SkyView_UI_Manager.instance.Area_Friend_info.gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = "��������";
        }
        // �ȷο��� ������� 
        //SkyView_UI_Manager.instance.Area_Friend_info.gameObject.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = followers.ToString() + "��";
        ShowProfile();
        SkyView_UI_Manager.instance.Area_Friend_Profile.gameObject.SetActive(false);
    }
    /// <summary>
    /// ���� ������ ������ �����ϴ� �Լ�
    /// </summary>
    public void ShowProfile()
    {
        //Area_Friend_Profile.transform.GetComponentInChildren<RawImage>().texture = texture;
        // ���� Ŭ���� �����̸��� UI�Ŵ����� ����(�� ������ ���� ����� ���� ����)
        SkyView_UI_Manager.instance.SelectedFriendName = NickName;
        SkyView_UI_Manager.instance.SelectedFriendIslandId = islandId;
        Debug.Log("check:" + NickName);
        //������ ������ �����Ѵ�.
        Area_Friend_Profile.GetComponent<Profile_Manager_New>().user_Nickname.text = NickName;
        Area_Friend_Profile.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<RawImage>().texture = texture;
        Area_Friend_Profile.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = keyword1;
        Area_Friend_Profile.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = keyword2;
        Area_Friend_Profile.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = keyword3;
        Area_Friend_Profile.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = keyword4;

        //----------> ���� Ű���� �߰� �κ�
        //for (int i = 0; i < 2; i++)
        //{
        //    Area_Friend_Profile.GetComponent<Profile_Manager_New>().keywords[i]=
        //}
        //Area_Friend_Profile.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = NickName+"�� ����";
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
