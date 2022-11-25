using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UserProfile;


public class UserProfile_Utility : MonoBehaviour
{
    public static UserProfile_Utility instance;
    List<Result> values = new List<Result>();

    public GameObject profile_prefab;
    public GameObject new_profile_prefab;
    public Transform profile_pos;
    UnityEngine.Texture image;
    public GameObject LoadingScene;



    private void Awake()
    {
        instance = this;
    }
    // 1.해당씬에 들어왔을때, 유저 정보를 로드한다.
    // 1-1.profileInfo 오브젝트 생성 
    // 2.받아온 프로필 모음갯수만큼 반복
    // 2-1 로드한 정보를 Profile Info에 저장한다. 
    // 2-2.Profile Object 생성 
    // 2-3.생성된 Profile를 ListProfile 에 위치시킨다. 
    // 2-4.로드해서 가져온 정보를 생성 프로필 오브젝트의 각 값에 저장한다. 
    private void Start()
    {
        LoadingScene.SetActive(true);
        Load();
        Load_Agora();
    }

    public async void Load()
    {
        var url = "https://resource.mtvs-nebula.com/avatar";
        var httpRequest = new HttpRequester(new JsonSerializationOption());
        Root result = await httpRequest.Get<Root>(url);
        values = result.results;
        //CreateProfile();
        CreateProfile_New();
    }
    public async void Load_Agora()
    {
        var url = "http://ec2-43-201-5-193.ap-northeast-2.compute.amazonaws.com:8001/token";
        var httpRequest = new HttpRequester(new JsonSerializationOption());
        Root_Agora result = await httpRequest.Get<Root_Agora>(url);
        Profile_Main_Manager.instance.A_Value = result;
    }

    public async void Delete(string avatarName)
    {
        var uri ="https://resource.mtvs-nebula.com/avatar/delete/"+avatarName;
        UnityWebRequest www = UnityWebRequest.Delete(uri);
        string token = PlayerPrefs.GetString("PlayerToken");
        www.SetRequestHeader("Authorization", "Bearer " + token);
        var operation = www.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("프로필 삭제 성공");
        }
        www.Dispose();
    }
    public async void Post(string avatarName,List<string> hashTag, UnityEngine.Texture2D Image )
    {
        var url = "https://resource.mtvs-nebula.com/avatar";
        WWWForm formData = new WWWForm();
        // 아바타 닉네임 입력부
        formData.AddField("AvatarName", avatarName);
        // 취향 단어 입력부(4개)
         for (int i = 0; i < hashTag.Count; i++)
        {
            string tagName = "tag" + (i+1).ToString();
            formData.AddField(tagName, hashTag[i]);
        }
        if (PlayerPrefs.GetString("extension") == ".png")
        {
            byte[] bytes = Image.EncodeToPNG();
            formData.AddBinaryData("image", bytes, avatarName+".png", "image/png");
            Debug.Log("PNG 엔코딩");
        }
        else if (PlayerPrefs.GetString("extension") == ".jpg")
        {
            byte[] bytes = Image.EncodeToJPG();
            formData.AddBinaryData("image", bytes, avatarName + ".jpg", "image/jpg");
            Debug.Log("jpg 엔코딩");
        }
        



        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        string token = PlayerPrefs.GetString("PlayerToken");
        www.SetRequestHeader("Authorization", "Bearer " + token);
        var operation = www.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form 양식 업로드 성공");
            Debug.Log(formData);
        }
        www.Dispose();

    }

    async void CreateProfile()
    { 
        foreach(Result i in values)
        {
            image = null;
            ProfileInfo info = new ProfileInfo();
            info.User_Name = i.name;
            info.HashTag = i.hashTags;
            info.User_Image_Url = i.imageUrl;
            info.User_Island_ID = i.skyIslandId;
            info.texture_info = i.texture;
            //프로필 오브젝트 생성 부분
            GameObject profile = Instantiate(profile_prefab, profile_pos);
            profile.GetComponentInChildren<Profile_Manager>().transfer(profile);
            profile.GetComponentInChildren<Profile_Manager>().temp_Info.texture_info = info.texture_info;
            profile.GetComponentInChildren<Profile_Manager>().temp_Info.User_Island_ID = info.User_Island_ID;
            Transform Area_Insert =profile.transform.GetChild(4);
            //이미지 불러오는 함수 
            GetTexture(info.User_Image_Url);
            //이미지가 불러올 때 까지 잠깐 쉬기
            while(image==null)
            {
                await Task.Yield();
            }
            //이미지 불러왔으면 이미지 삽입란에 삽입
            Area_Insert.GetChild(0).GetComponent<RawImage>().texture = image;
            //키워드 넣는 코드
            for (int j = 0; j < Area_Insert.GetChild(1).childCount; j++)
            {
                Area_Insert.GetChild(1).GetChild(j).GetComponentInChildren<Text>().text = info.HashTag[j];
            }
            profile.transform.GetChild(6).gameObject.GetComponent<Text>().text = info.User_Name;

            await Task.Yield();
        }
        // 새로운 프로필 아이콘 생성
        Create_ProfileArea();
    }
    async void CreateProfile_New()
    {
        foreach (Result i in values)
        {
            image = null;
            ProfileInfo info = new ProfileInfo();
            info.User_Name = i.name;
            info.HashTag = i.hashTags;
            info.User_Image_Url = i.imageUrl;
            info.User_Island_ID = i.skyIslandId;
            info.texture_info = i.texture;
            //프로필 오브젝트 생성 부분
            GameObject profile = Instantiate(profile_prefab, profile_pos);
            Profile_Manager_New mgr = profile.GetComponentInChildren<Profile_Manager_New>();
            mgr.temp_Info.texture_info = info.texture_info;
            mgr.temp_Info.User_Island_ID = info.User_Island_ID;
            //이미지 불러오는 함수 
            GetTexture(info.User_Image_Url);
            //이미지가 불러올 때 까지 잠깐 쉬기
            while (image == null)
            {
                await Task.Yield();
            }
            //이미지 불러왔으면 이미지 삽입란에 삽입
            profile.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<RawImage>().texture = image;
            //mgr.user_Image = image;
            //키워드 넣는 코드
            for (int j = 0; j < mgr.keywords.Length; j++)
            {
                mgr.keywords[j].text= info.HashTag[j];
            }
            mgr.user_Nickname.text = info.User_Name;
            await Task.Yield();
        }
        // 새로운 프로필 아이콘 생성
        Create_ProfileArea();
    }
    public void UpdateProfile(string userName, Texture texture, string[] _keywords)
    {
        GameObject profile = Instantiate(profile_prefab, profile_pos);
        Profile_Manager_New info = profile.GetComponent<Profile_Manager_New>();
        //오브젝트 만들고 
        // 오브젝트의 각 부분에 edit에서 생성한정보를 삽닙
        //클래스에 저장된 정보를 해당  UI에 저장한다. 
        info.user_Nickname.text = userName;
        profile.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<RawImage>().texture = texture;
        for (int i = 0; i < info.keywords.Length; i++)
        {
            info.keywords[i].text = _keywords[i];
        }
    }
    async void GetTexture(string url)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        var operation = www.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        try
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            image = myTexture;
        }
        catch
        {
            Debug.Log(www.error);
        }


    }
    public void Create_ProfileArea()
    {
        if (values.Count < 3)
        { 
            // 프로필 추가 아이콘을 생성한다. (구름 아이콘)
          
            GameObject profile = Instantiate(new_profile_prefab, profile_pos);
            //profile.GetComponentInChildren<Profile_Manager>().transfer(profile);
            //profile.SetActive(true);
            //profile.GetComponentInChildren<Profile_Manager>().Initialize();
        }
        
        //LoadingScene.SetActive(false);
    }
}
