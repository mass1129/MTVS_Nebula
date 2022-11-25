using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UserProfile;

public class Profile_Edit_new : MonoBehaviour
{
    new_ProfileInfo new_profileInfo = new new_ProfileInfo();
    public Button btn_upload_Image;
    //�������ڸ��� ������ ��ư ����� Ȱ��ȭ �Ѵ�.
    public Button btn_Create_Icon;
    public Button btn_Save;
    public Button btn_Close;
    public TMP_InputField user_NickName;
    public TMP_InputField[] user_Keywords = new TMP_InputField[4];
    string[] paths;
    InputFieldTabManager inputFieldTabMrg;

    private void Awake()
    {
        btn_Create_Icon.onClick.AddListener(OnEditDisplay);
        btn_Save.onClick.AddListener(SaveInputs);
        btn_upload_Image.onClick.AddListener(AddImage);
    }
    void Start()
    {
        OffEditDisplay();
    }
    private void Update()
    {
        inputFieldTabMrg.CheckFocus();
    }

    /// <summary>
    /// ������ �Է�
    /// </summary>
    void OnEditDisplay()
    {
        //��ư�� ����.
        //�����ʹ� Ų��. 
        transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

    }
    void OffEditDisplay()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        inputFieldTabMrg = new InputFieldTabManager();
        inputFieldTabMrg.Add(user_NickName);
        for (int i = 0; i < user_Keywords.Length; i++)
        {
            inputFieldTabMrg.Add(user_Keywords[i]);
        }
        user_NickName.Select();
        inputFieldTabMrg.SetFocus(0);
    }

    #region 3. ���� �÷��̾ �̹��� ���ε� ��ư�� ������ ��� �߻��ϴ� �Լ�
    [System.Obsolete]
    public void AddImage()
    {
        // ��ư ������ �ϴ� �̹����� �ʱ�ȭ �Ѵ�. 
        transform.GetComponentInChildren<RawImage>().texture = null;
        var extensions = new[] {
        new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            };
        paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (paths.Length <= 0)
        {
            Debug.Log("�Ȱ����");
            return;
        }

        PlayerPrefs.SetString("extension", Path.GetExtension(paths[0]));
        Debug.Log("�̹��� �ҷ���, Ȯ����: " + Path.GetExtension(paths[0]));
        GetTexture();
    }

    [System.Obsolete]
    async void GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + paths[0]);
        var operation = www.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        try
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            new_profileInfo.ProfileImage = myTexture;
            transform.GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetComponent<RawImage>().texture = myTexture;
            Debug.Log("���ε� �Ϸ�");
        }
        catch
        {
            Debug.Log(www.error);
        }
        //if (www.isNetworkError || www.isHttpError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    UnityEngine.Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        //    // ���� �̹����� �뷮�� ���뷮 �̻����� ���ε� ������ ���� �޼��� �˾� ��Ű�� �޼��� �����Ų��.
        //    if (PlayerPrefs.GetString("extension") == ".jpg")
        //    {
        //        byte[] image = myTexture.EncodeToJPG();
        //        if (image.Length >= 14000)
        //        {
        //            //���� �˾�â ��������
        //            yield break;
        //        }
        //    }
        //    else if (PlayerPrefs.GetString("extension") == ".png")
        //    {
        //        byte[] image = myTexture.EncodeToPNG();
        //        if (image.Length >= 14000)
        //        {
        //            //���� �˾�â ��������
        //            yield break;
        //        }
        //    }


        //}
    }
    #endregion
    public void SaveInputs()
    {
        new_profileInfo.User_Name = user_NickName.text;
        //���⼭ 4���� input�� ��� ���Դ��� �˻��Ѵ�. 
        foreach (TMP_InputField i in user_Keywords)
        {
            // ���� �Է¶��� �ƹ��͵� ������ ���� �Ұ��ϴٰ� ��� �޼��� ���
            if (i.text.Length <= 0)
            {
                print("�Է¶��� ��� ä���ּ���!!");
                return;
            }
        }
        for (int i = 0; i < user_Keywords.Length; i++)
        {
            new_profileInfo.HashTag.Add(user_Keywords[i].text);
        }
        if (new_profileInfo.ProfileImage != null && new_profileInfo.HashTag.Count > 0 && new_profileInfo.User_Name != null)
        {
            //���� �Ϸ�!
            //������ json���� �ش� ������ ������!
            UserProfile_Utility.instance.Post(new_profileInfo.User_Name, new_profileInfo.HashTag, new_profileInfo.ProfileImage);
            Debug.Log("����Ϸ�");
            UserProfile_Utility.instance.UpdateProfile(new_profileInfo.User_Name, new_profileInfo.ProfileImage, new_profileInfo.HashTag.ToArray());
            UserProfile_Utility.instance.Create_ProfileArea();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("�������");
        }


    }

}
