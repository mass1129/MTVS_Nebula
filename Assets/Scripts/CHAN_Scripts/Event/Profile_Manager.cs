using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UserProfile;

// 플레이어 프로필 정보를 저장할 클래스 

public class Profile_Info
{ 
    public UnityEngine.Texture rawImage;
    public List<string> Keywords = new List<string>();
}
public class Profile_Manager : MonoBehaviour
{
    
    #region 속성 모음
    // 생성 버튼
    public GameObject btn_create;
    // 이미지 업로드 버튼
    public GameObject btn_UploadImage;
    // 키워드 선택 버튼 
    public GameObject btn_SelectKeywords;
    //닉네임 생성 버튼
    public GameObject btn_AddNickName;
    // 생성 완료 버튼
    public GameObject btn_Done;
    //다음씬으로 넘어가는 버튼
    [Header("이미지 업로드 관련 설정들")]
    string[] paths;
    public RawImage rawImage;
    [Header("키워드 선택창")]
    public GameObject[] input_keywords;
    public GameObject btn_keywordsDone;
    [Header("업로드 된 프로필")]
    public Transform Area_Load_Profile;
    [Header("닉네임 입력")]
    public Transform Area_NickName;
    public GameObject btn_NickNameDone;
    public Text Text_Nickname;
    GameObject input_NickName;
    [Header("프로필 수정")]
    public GameObject Btn_ReviceProfile;
    public GameObject Btn_DeleteProfile;
    #endregion
    //일단 네트워크를 적용하지 않은 상태이므로 임시로 정보를 받을  클래스를 생성시켜서 저장해보자 
    new_ProfileInfo new_profileInfo = new new_ProfileInfo();
    Profile_Info profile = new Profile_Info();
    public ProfileInfo temp_Info = new ProfileInfo();
    //해로 생성된 프로필 버튼 위치 지정해주는 함수
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
    #region 1. 초기에 해당 씬에 들어왔을 때 디폴트로 나오게 할 설정들
    public void Initialize()
    {
        
        // create 버튼 활성화
        btn_create.SetActive(true);
        //나머지 버튼 모두 비활성화 
        OnMainSelect(false);
        btn_Done.SetActive(false);
        //키워드창 꺼짐
        OnKeywords(false);
        OnLoadInfo(false);
        // 상단 닉네임 입력부
        Text_Nickname.gameObject.SetActive(false);
        OnNickname(false);
        //수정, 삭제 버튼 안보이도록
        Btn_ReviceProfile.SetActive(false);
        Btn_DeleteProfile.SetActive(false);
    }
    #endregion
    #region 2. 만약 플레이어가 btn_create 를 눌렀을때 작동됨
    public void AddProfile(bool b)
    {
        //btn_create 버튼이 사라지고
        btn_create.SetActive(false);
        // 업로드, 키워드 버튼이 켜진다. 
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
        //이미지 업로드 버튼
        btn_UploadImage.SetActive(b);
        //키워드 버튼 
        btn_SelectKeywords.SetActive(b);
        //닉네임 추가 부분
        btn_AddNickName.SetActive(b);
    }
    // 저장된 정보모음
    void OnLoadInfo(bool b)
    {
        Area_Load_Profile.gameObject.SetActive(b);
        Text_Nickname.gameObject.SetActive(b);
    }

    #endregion
    #region 3. 만약 플레이어가 이미지 업로드 버튼을 눌렀을 경우 발생하는 함수
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
    #region 4. 키워드 관련 기능 목록
    public void AddKeywords()
    {
        OnMainSelect(false);
        // 키워드1234 버튼, 키워드 선택 완료 버튼 활성화
        OnKeywords(true);
        // done 버튼 비활성화
        btn_Done.SetActive(false);
        
    }
    //키워드 창을 켜는 함수
    void OnKeywords(bool b)
    {
        foreach (GameObject i in input_keywords)
        {
            i.SetActive(b);
        }
        btn_keywordsDone.SetActive(b);
    }
    //만약 키워드를 모두 입력하면 클래스에 저장한다. 
    public void DoneInputKeyWords()
    {
        //여기서 4곳의 input이 모두 들어왔는지 검사한다. 
        foreach (GameObject i in input_keywords)
        {
            // 만약 입력란에 아무것도 없으면 저장 불가하다고 경고 메세지 출력
            if (i.GetComponent<InputField>().text.Length <= 0)
            {
                print("입력란을 모두 채워주세요!!");
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
    #region 5.닉네임 관련 기능 목록
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
            print("입력란을 모두 채워주세요!!");
        }
        Initialize();
        AddProfile(true);
    }
    #endregion
    #region 6. 최종적으로 프로필을 생성시키는 함수
    public void Btn_CreateProfile()
    {
        //이미지가 있는가?
        //keyword가 저장되어 있는가?
        if (new_profileInfo.ProfileImage != null&& new_profileInfo.HashTag.Count>0&&new_profileInfo.User_Name!=null)
        {
            //저장 완료!
            //서버에 json으로 해당 정보를 보내자!
            UserProfile_Utility.instance.Post(new_profileInfo.User_Name, new_profileInfo.HashTag,new_profileInfo.ProfileImage);
        }
        //프로필 생성 함수 
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
        //클래스에 저장된 정보를 해당  UI에 저장한다. 
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
    #region 7. 만약 플레이어가 등록된 프로필을 눌렀을 때 발생하는 함수
    //다음 씬으로 넘어가는 버튼을 킨다.
    public void OnNextSceneBtn()
    {
        Profile_Main_Manager.instance.btn_MoveNextScene.SetActive(!Btn_ReviceProfile.activeSelf);
        Btn_ReviceProfile.SetActive(!Btn_ReviceProfile.activeSelf);
        Btn_DeleteProfile.SetActive(!Btn_DeleteProfile.activeSelf);
        Profile_Main_Manager.instance.avatarName=Text_Nickname.text;
        Profile_Main_Manager.instance.islandID = temp_Info.User_Island_ID.ToString();
        if (temp_Info.texture_info == null)
        {
            // 아바타 생선씬으로 넘어간다.
            Profile_Main_Manager.instance.hasAvatar = false;
        }
        else
        {
            //서버 접속할 수 있도록 (서버접속 씬으로 넘어간다.)
            Profile_Main_Manager.instance.hasAvatar = true;
        }

    }
    #endregion
    #region 7. 프로필 수정, 삭제 
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

    #endregion 프로필 수정, 삭제

}

