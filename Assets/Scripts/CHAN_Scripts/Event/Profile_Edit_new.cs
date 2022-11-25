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
    //시작하자마자 아이콘 버튼 기능을 활성화 한다.
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
    /// 프로필 입력
    /// </summary>
    void OnEditDisplay()
    {
        //버튼은 끈다.
        //에디터는 킨다. 
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

    #region 3. 만약 플레이어가 이미지 업로드 버튼을 눌렀을 경우 발생하는 함수
    [System.Obsolete]
    public void AddImage()
    {
        // 버튼 누르면 일단 이미지는 초기화 한다. 
        transform.GetComponentInChildren<RawImage>().texture = null;
        var extensions = new[] {
        new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            };
        paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (paths.Length <= 0)
        {
            Debug.Log("안골라짐");
            return;
        }

        PlayerPrefs.SetString("extension", Path.GetExtension(paths[0]));
        Debug.Log("이미지 불러옴, 확장자: " + Path.GetExtension(paths[0]));
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
            Debug.Log("업로드 완료");
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
        //    // 만약 이미지의 용량이 허용용량 이상으로 업로드 됐으면 오류 메세지 팝업 시키는 메세지 송출시킨다.
        //    if (PlayerPrefs.GetString("extension") == ".jpg")
        //    {
        //        byte[] image = myTexture.EncodeToJPG();
        //        if (image.Length >= 14000)
        //        {
        //            //오류 팝업창 나오도록
        //            yield break;
        //        }
        //    }
        //    else if (PlayerPrefs.GetString("extension") == ".png")
        //    {
        //        byte[] image = myTexture.EncodeToPNG();
        //        if (image.Length >= 14000)
        //        {
        //            //오류 팝업창 나오도록
        //            yield break;
        //        }
        //    }


        //}
    }
    #endregion
    public void SaveInputs()
    {
        new_profileInfo.User_Name = user_NickName.text;
        //여기서 4곳의 input이 모두 들어왔는지 검사한다. 
        foreach (TMP_InputField i in user_Keywords)
        {
            // 만약 입력란에 아무것도 없으면 저장 불가하다고 경고 메세지 출력
            if (i.text.Length <= 0)
            {
                print("입력란을 모두 채워주세요!!");
                return;
            }
        }
        for (int i = 0; i < user_Keywords.Length; i++)
        {
            new_profileInfo.HashTag.Add(user_Keywords[i].text);
        }
        if (new_profileInfo.ProfileImage != null && new_profileInfo.HashTag.Count > 0 && new_profileInfo.User_Name != null)
        {
            //저장 완료!
            //서버에 json으로 해당 정보를 보내자!
            UserProfile_Utility.instance.Post(new_profileInfo.User_Name, new_profileInfo.HashTag, new_profileInfo.ProfileImage);
            Debug.Log("저장완료");
            UserProfile_Utility.instance.UpdateProfile(new_profileInfo.User_Name, new_profileInfo.ProfileImage, new_profileInfo.HashTag.ToArray());
            UserProfile_Utility.instance.Create_ProfileArea();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("저장실패");
        }


    }

}
