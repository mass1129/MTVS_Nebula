using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// 플레이어 프로필 정보를 저장할 클래스 
class ProfileInfo
{
    public Texture User_Texture;
    public List<string> User_Keywords = new List<string>();
}
public class Profile_Manager : MonoBehaviour
{
    #region 속성 모음
    //뒤로가기 버튼
    public GameObject btn_backToStart;
    // 생성 이미지
    public Image create;
    // 생성된 이미지
    public Image created;
    // 생성 버튼
    public GameObject btn_create;
    // 이미지 업로드 버튼
    public GameObject btn_UploadImage;
    // 키워드 선택 버튼 
    public GameObject btn_SelectKeywords;
    // 생성 완료 버튼
    public GameObject btn_Done;
    [Header("이미지 업로드 관련 설정들")]
    string[] paths;
    public RawImage rawImage;
    [Header("키워드 선택창")]
    public GameObject[] input_keywords;
    public GameObject btn_keywordsDone;
    [Header("업로드 된 프로필")]
    public Transform Area_Created;
    #endregion
    //일단 네트워크를 적용하지 않은 상태이므로 임시로 정보를 받을  클래스를 생성시켜서 저장해보자 
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
    #region 초기에 해당 씬에 들어왔을 때 디폴트로 나오게 할 설정들
    public void InitializeUIs()
    {
        
        //뒤로가기 버튼 활성화
        btn_backToStart.SetActive(true);
        // create 이미지 활성화
        create.enabled = true;
        // create 버튼 활성화
        btn_create.SetActive(true);
        //나머지 버튼 모두 비활성화 
        OnMainSelect(false);
        btn_Done.SetActive(false);
        //키워드창 꺼짐
        OnKeywords(false);
        print("꺼짐");
    }
    #endregion
    #region 만약 플레이어가 btn_create 를 눌렀을때 작동됨
    public void AddProfile()
    {
        //btn_create 버튼이 사라지고
        btn_create.SetActive(false);
        // 업로드, 키워드 버튼이 켜진다. 
        OnMainSelect(true);
    }
    void OnMainSelect(bool b)
    {
        //이미지 업로드 버튼
        btn_UploadImage.SetActive(b);
        //키워드 버튼 
        btn_SelectKeywords.SetActive(b);
    }

    #endregion
    #region 최종적으로 프로필을 생성시키는 함수
    public void Btn_CreateProfile()
    {
        //이미지가 있는가?
        //keyword가 저장되어 있는가?
        if (!info.User_Texture == null&& info.User_Keywords.Count>0)
        { 
            //저장 완료!
            //서버에 json으로 해당 정보를 보내자!
        }
        //프로필 생성 함수 
        UpdateProfile();
    }
    void UpdateProfile()
    {
        //클래스에 저장된 정보를 해당  UI에 저장한다. 
        Texture upload_texture = Area_Created.GetChild(0).GetComponent<RawImage>().texture;
        upload_texture = info.User_Texture;
        for (int i = 0; i < Area_Created.GetChild(1).childCount; i++)
        {
            Area_Created.GetChild(1).GetChild(0).GetChild(i).GetComponent<Text>().text = info.User_Keywords[i];
        }
        

    }
    #endregion
    #region 만약 플레이어가 이미지 업로드 버튼을 눌렀을 경우 발생하는 함수
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
    #region 키워드 관련 기능 목록
    public void AddKeywords()
    {
        OnMainSelect(false);
        // done 버튼 비활성화
        btn_Done.SetActive(false);
        // 키워드1234 버튼, 키워드 선택 완료 버튼 활성화
        OnKeywords(true);
        
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
            info.User_Keywords.Add(input_keywords[i].GetComponent<InputField>().text);
        }
        InitializeUIs();
        AddProfile();
    }
    #endregion

}
