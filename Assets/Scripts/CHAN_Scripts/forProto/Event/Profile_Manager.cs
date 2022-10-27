using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// �÷��̾� ������ ������ ������ Ŭ���� 
class ProfileInfo
{
    public Texture User_Texture;
    public List<string> User_Keywords = new List<string>();
}
public class Profile_Manager : MonoBehaviour
{
    #region �Ӽ� ����
    //�ڷΰ��� ��ư
    public GameObject btn_backToStart;
    // ���� �̹���
    //public Image create;
    // ������ �̹���
    public Image created;
    // ���� ��ư
    public GameObject btn_create;
    // �̹��� ���ε� ��ư
    public GameObject btn_UploadImage;
    // Ű���� ���� ��ư 
    public GameObject btn_SelectKeywords;
    // ���� �Ϸ� ��ư
    public GameObject btn_Done;
    //���������� �Ѿ�� ��ư
    public GameObject btn_MoveNextScene;
    [Header("�̹��� ���ε� ���� ������")]
    string[] paths;
    public RawImage rawImage;
    [Header("Ű���� ����â")]
    public GameObject[] input_keywords;
    public GameObject btn_keywordsDone;
    [Header("���ε� �� ������")]
    public Transform Area_Load_Profile;
    [Header("������ ����")]
    public GameObject Btn_ReviceProfile;
    public GameObject Btn_DeleteProfile;
    #endregion
    //�ϴ� ��Ʈ��ũ�� �������� ���� �����̹Ƿ� �ӽ÷� ������ ����  Ŭ������ �������Ѽ� �����غ��� 
    ProfileInfo info = new ProfileInfo();
    void Start()
    {
        InitializeUIs();
    }

 
    void Update()
    {
        if (info.User_Texture != null && info.User_Keywords.Count > 0)
        {
            btn_Done.SetActive(true);
        }
    }
    #region 1. �ʱ⿡ �ش� ���� ������ �� ����Ʈ�� ������ �� ������
    public void InitializeUIs()
    {
        
        //�ڷΰ��� ��ư Ȱ��ȭ
        btn_backToStart.SetActive(true);
        btn_MoveNextScene.SetActive(false);
        // create �̹��� Ȱ��ȭ
        //create.enabled = true;
        // create ��ư Ȱ��ȭ
        btn_create.SetActive(true);
        //������ ��ư ��� ��Ȱ��ȭ 
        OnMainSelect(false);
        btn_Done.SetActive(false);
        //Ű����â ����
        OnKeywords(false);
        OnLoadInfo(false);


        print("����");
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
    }
    void OnMainSelect(bool b)
    {
        //�̹��� ���ε� ��ư
        btn_UploadImage.SetActive(b);
        //Ű���� ��ư 
        btn_SelectKeywords.SetActive(b);
    }
    // ����� ��������
    void OnLoadInfo(bool b)
    {
        Area_Load_Profile.gameObject.SetActive(b);
    }

    #endregion
    #region 3. ���� �÷��̾ �̹��� ���ε� ��ư�� ������ ��� �߻��ϴ� �Լ�
    public void AddImage()
    {
        paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "png", true);
        StartCoroutine(GetTexture());
    }
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
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //rawImage.texture = myTexture;
            info.User_Texture = myTexture;
        }
    }
    #endregion
    #region 4. Ű���� ���� ��� ���
    public void AddKeywords()
    {
        OnMainSelect(false);
        // done ��ư ��Ȱ��ȭ
        btn_Done.SetActive(false);
        // Ű����1234 ��ư, Ű���� ���� �Ϸ� ��ư Ȱ��ȭ
        OnKeywords(true);
        
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
            info.User_Keywords.Add(input_keywords[i].GetComponent<InputField>().text);
        }
        InitializeUIs();
        AddProfile(true);
    }
    #endregion
    #region 5. ���������� �������� ������Ű�� �Լ�
    public void Btn_CreateProfile()
    {
        //�̹����� �ִ°�?
        //keyword�� ����Ǿ� �ִ°�?
        if (!info.User_Texture == null&& info.User_Keywords.Count>0)
        { 
            //���� �Ϸ�!
            //������ json���� �ش� ������ ������!
        }
        //������ ���� �Լ� 
        UpdateProfile();
        OnLoadInfo(true);
        AddProfile(false);
    }
    void UpdateProfile()
    {
        //Ŭ������ ����� ������ �ش�  UI�� �����Ѵ�. 
        Area_Load_Profile.GetChild(0).GetComponent<RawImage>().texture = info.User_Texture;
        
        for (int i = 0; i < Area_Load_Profile.GetChild(1).childCount; i++)
        {
            Area_Load_Profile.GetChild(1).GetChild(i).GetChild(0).GetComponent<Text>().text = info.User_Keywords[i];
        }
        

    }
    #endregion
    #region 6. ���� �÷��̾ ��ϵ� �������� ������ �� �߻��ϴ� �Լ�
    public void OnNextSceneBtn()
    {
        btn_MoveNextScene.SetActive(true);
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(2);
    }
    #endregion
}
