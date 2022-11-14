using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UserProfile;

// �÷��̾� ������ ������ ������ Ŭ���� 

public class Profile_Info
{ 
    public UnityEngine.Texture rawImage;
    public List<string> Keywords = new List<string>();
}
public class Profile_Manager : MonoBehaviour
{
    
    #region �Ӽ� ����
    // ���� ��ư
    public GameObject btn_create;
    // �̹��� ���ε� ��ư
    public GameObject btn_UploadImage;
    // Ű���� ���� ��ư 
    public GameObject btn_SelectKeywords;
    //�г��� ���� ��ư
    public GameObject btn_AddNickName;
    // ���� �Ϸ� ��ư
    public GameObject btn_Done;
    //���������� �Ѿ�� ��ư
    [Header("�̹��� ���ε� ���� ������")]
    string[] paths;
    public RawImage rawImage;
    [Header("Ű���� ����â")]
    public GameObject[] input_keywords;
    public GameObject btn_keywordsDone;
    [Header("���ε� �� ������")]
    public Transform Area_Load_Profile;
    [Header("�г��� �Է�")]
    public Transform Area_NickName;
    public GameObject btn_NickNameDone;
    public Text Text_Nickname;
    GameObject input_NickName;
    [Header("������ ����")]
    public GameObject Btn_ReviceProfile;
    public GameObject Btn_DeleteProfile;
    #endregion
    //�ϴ� ��Ʈ��ũ�� �������� ���� �����̹Ƿ� �ӽ÷� ������ ����  Ŭ������ �������Ѽ� �����غ��� 
    new_ProfileInfo new_profileInfo = new new_ProfileInfo();
    Profile_Info profile = new Profile_Info();
    public ProfileInfo temp_Info = new ProfileInfo();
    //�ط� ������ ������ ��ư ��ġ �������ִ� �Լ�
    public void transfer(GameObject obj)
    {
        btn_create = obj.transform.GetChild(0).gameObject;
        btn_UploadImage = obj.transform.GetChild(1).GetChild(0).gameObject;
        btn_SelectKeywords = obj.transform.GetChild(1).GetChild(1).gameObject;
        btn_AddNickName = obj.transform.GetChild(1).GetChild(2).gameObject;
        btn_Done =obj.transform.GetChild(1).GetChild(3).gameObject;
        for (int i = 0; i < obj.transform.GetChild(2).GetChild(0).childCount; i++)
        {
            input_keywords[i] = obj.transform.GetChild(2).GetChild(0).GetChild(i).gameObject;
        }  
        btn_keywordsDone= obj.transform.GetChild(2).GetChild(1).gameObject;
        Area_NickName = obj.transform.transform.GetChild(3);
        input_NickName= obj.transform.transform.GetChild(3).GetChild(0).gameObject;
        btn_NickNameDone = obj.transform.GetChild(3).GetChild(1).gameObject;
        Area_Load_Profile = obj.transform.GetChild(4);
        Btn_ReviceProfile= obj.transform.GetChild(5).GetChild(0).gameObject;
        Btn_DeleteProfile= obj.transform.GetChild(5).GetChild(1).gameObject;
        Text_Nickname = obj.transform.GetChild(6).gameObject.GetComponent<Text>();
    }
    void Start()
    {
        Btn_ReviceProfile.SetActive(false);
        Btn_DeleteProfile.SetActive(false);
    }

 
    void Update()
    {
        
    }
    #region 1. �ʱ⿡ �ش� ���� ������ �� ����Ʈ�� ������ �� ������
    public void Initialize()
    {
        
        // create ��ư Ȱ��ȭ
        btn_create.SetActive(true);
        //������ ��ư ��� ��Ȱ��ȭ 
        OnMainSelect(false);
        btn_Done.SetActive(false);
        //Ű����â ����
        OnKeywords(false);
        OnLoadInfo(false);
        // ��� �г��� �Էº�
        Text_Nickname.gameObject.SetActive(false);
        OnNickname(false);
        //����, ���� ��ư �Ⱥ��̵���
        Btn_ReviceProfile.SetActive(false);
        Btn_DeleteProfile.SetActive(false);
    }
    #endregion
    #region 2. ���� �÷��̾ btn_create �� �������� �۵���
    public void AddProfile(bool b)
    {
        //btn_create ��ư�� �������
        btn_create.SetActive(false);
        // ���ε�, Ű���� ��ư�� ������. 
        if (b == true)
        {
            OnMainSelect(true);
        }
        else
        { OnMainSelect(false); }
        if (new_profileInfo.ProfileImage != null && new_profileInfo.HashTag.Count > 0 && new_profileInfo.User_Name != null)
        {
            btn_Done.SetActive(true);
        }
    }
    void OnMainSelect(bool b)
    {
        //�̹��� ���ε� ��ư
        btn_UploadImage.SetActive(b);
        //Ű���� ��ư 
        btn_SelectKeywords.SetActive(b);
        //�г��� �߰� �κ�
        btn_AddNickName.SetActive(b);
    }
    // ����� ��������
    void OnLoadInfo(bool b)
    {
        Area_Load_Profile.gameObject.SetActive(b);
        Text_Nickname.gameObject.SetActive(b);
    }

    #endregion
    #region 3. ���� �÷��̾ �̹��� ���ε� ��ư�� ������ ��� �߻��ϴ� �Լ�
    [System.Obsolete]
    public void AddImage()
    {
        paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "png", true);
        StartCoroutine(GetTexture());
    }

    [System.Obsolete]
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + paths[0]);

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            UnityEngine.Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            new_profileInfo.ProfileImage = myTexture;
        }
    }
    #endregion
    #region 4. Ű���� ���� ��� ���
    public void AddKeywords()
    {
        OnMainSelect(false);
        // Ű����1234 ��ư, Ű���� ���� �Ϸ� ��ư Ȱ��ȭ
        OnKeywords(true);
        // done ��ư ��Ȱ��ȭ
        btn_Done.SetActive(false);
        
    }
    //Ű���� â�� �Ѵ� �Լ�
    void OnKeywords(bool b)
    {
        foreach (GameObject i in input_keywords)
        {
            i.SetActive(b);
        }
        btn_keywordsDone.SetActive(b);
    }
    //���� Ű���带 ��� �Է��ϸ� Ŭ������ �����Ѵ�. 
    public void DoneInputKeyWords()
    {
        //���⼭ 4���� input�� ��� ���Դ��� �˻��Ѵ�. 
        foreach (GameObject i in input_keywords)
        {
            // ���� �Է¶��� �ƹ��͵� ������ ���� �Ұ��ϴٰ� ��� �޼��� ���
            if (i.GetComponent<InputField>().text.Length <= 0)
            {
                print("�Է¶��� ��� ä���ּ���!!");
                return;
            }
        }
        for (int i = 0; i < input_keywords.Length; i++)
        {
            new_profileInfo.HashTag.Add(input_keywords[i].GetComponent<InputField>().text);
        }
        Initialize();
        AddProfile(true);
    }
    #endregion
    #region 5.�г��� ���� ��� ���
    public void AddNickName()
    {
        OnMainSelect(false);
        OnNickname(true);
        btn_Done.SetActive(false);
    }
    void OnNickname(bool b)
    {
        Area_NickName.gameObject.SetActive(b);
        input_NickName.SetActive(true);
        
    }
    public void OnNickNameDone()
    {
        if (input_NickName.GetComponent<InputField>().text.Length > 0)
        {
            new_profileInfo.User_Name = input_NickName.GetComponent<InputField>().text;
            Text_Nickname.text= input_NickName.GetComponent<InputField>().text;
        }
        else
        {
            print("�Է¶��� ��� ä���ּ���!!");
        }
        Initialize();
        AddProfile(true);
    }
    #endregion
    #region 6. ���������� �������� ������Ű�� �Լ�
    public void Btn_CreateProfile()
    {
        //�̹����� �ִ°�?
        //keyword�� ����Ǿ� �ִ°�?
        if (new_profileInfo.ProfileImage != null&& new_profileInfo.HashTag.Count>0&&new_profileInfo.User_Name!=null)
        {
            //���� �Ϸ�!
            //������ json���� �ش� ������ ������!
            UserProfile_Utility.instance.Post(new_profileInfo.User_Name, new_profileInfo.HashTag,new_profileInfo.ProfileImage);
        }
        //������ ���� �Լ� 
        UpdateProfile();
        OnLoadInfo(true);
        AddProfile(false);
        if (!isRevice)
        { 
            Profile_Main_Manager.instance.AddProfileBlock();
        }
        isRevice = true;
        //Test_UserProfile.instance.Save(profile);
    }
    void UpdateProfile()
    {
        //Ŭ������ ����� ������ �ش�  UI�� �����Ѵ�. 
        Area_Load_Profile.GetChild(0).GetComponent<RawImage>().texture = new_profileInfo.ProfileImage;
       
        profile.rawImage = new_profileInfo.ProfileImage;

        for (int i = 0; i < Area_Load_Profile.GetChild(1).childCount; i++)
        {
            Area_Load_Profile.GetChild(1).GetChild(i).GetComponentInChildren<Text>().text = new_profileInfo.HashTag[i];
            profile.Keywords.Add(new_profileInfo.HashTag[i]);
        }
    }
    void InitialProfiles()
    {
        Area_Load_Profile.GetChild(0).GetComponent<RawImage>().texture = null;

        for (int i = 0; i < Area_Load_Profile.GetChild(1).childCount; i++)
        {
            Area_Load_Profile.GetChild(1).GetChild(i).GetChild(0).GetComponent<Text>().text = null;
        }
    }
    #endregion
    #region 7. ���� �÷��̾ ��ϵ� �������� ������ �� �߻��ϴ� �Լ�
    //���� ������ �Ѿ�� ��ư�� Ų��.
    public void OnNextSceneBtn()
    {
        Profile_Main_Manager.instance.btn_MoveNextScene.SetActive(!Btn_ReviceProfile.activeSelf);
        Btn_ReviceProfile.SetActive(!Btn_ReviceProfile.activeSelf);
        Btn_DeleteProfile.SetActive(!Btn_DeleteProfile.activeSelf);
        Profile_Main_Manager.instance.avatarName=Text_Nickname.text;
        Profile_Main_Manager.instance.islandID = temp_Info.User_Island_ID.ToString();
        if (temp_Info.texture_info == null)
        {
            // �ƹ�Ÿ ���������� �Ѿ��.
            Profile_Main_Manager.instance.hasAvatar = false;
        }
        else
        {
            //���� ������ �� �ֵ��� (�������� ������ �Ѿ��.)
            Profile_Main_Manager.instance.hasAvatar = true;
        }

    }
    #endregion
    #region 7. ������ ����, ���� 
    bool isRevice;
    public void Revice()
    {
        Initialize();
        AddProfile(true);
        isRevice = true;
    }
    public void Delate()
    {
        UserProfile_Utility.instance.Delete(Text_Nickname.text);
        Profile_Main_Manager.instance.btn_MoveNextScene.SetActive(false);
        Destroy(transform.parent.gameObject);
    }

    #endregion ������ ����, ����

}

