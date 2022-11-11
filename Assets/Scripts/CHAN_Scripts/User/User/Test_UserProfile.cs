using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UserProfile;


public class Test_UserProfile : MonoBehaviour
{
    public static Test_UserProfile instance;
    List<Result> values = new List<Result>();
    public GameObject profile_prefab;
    public Transform profile_pos;
    UnityEngine.Texture image;


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
    public void OnClickLoadBtn()
    {
        // 서버로부터 받아온다.
        Load(); 
    }
    public void OnClickPostBtn()
    {
        Post();
    }
    //public void Save(Profile_Info Container)
    //{

    //    string json = JsonUtility.ToJson(Container, true);
    //    print(json);
    //    PlayerPrefs.SetString("ProfileSave", json);
    //    K_SaveSystem.Save("ProfileSave", json, true);

    //}
    public async void Load()
    {
        
        
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/avatar";
        var httpRequest = new HttpRequester(new JsonSerializationOption());
        Root result = await httpRequest.Get<Root>(url);
        values = result.results;
        CreateProfile();
    }
    public async void Post()
    {
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/avatar";
        WWWForm formData = new WWWForm();
        string avatarName = "kwon";
        string tag1 = "라이프스타일/취미";
        string tag2 = "캠핑/등산";
        string dummy1 = "dummy";
        string dummy2 = "dummy";
        formData.AddField("AvatarName", avatarName);
        formData.AddField("tag1", tag1);
        formData.AddField("tag2", tag2);
        formData.AddField("tag3",dummy1);
        formData.AddField("tag4",dummy2);
        formData.AddBinaryData("image", File.ReadAllBytes(Application.streamingAssetsPath + "/kwon.png"), "kwon.png", "image/png");



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
            GameObject profile = Instantiate(profile_prefab, profile_pos);
            Transform Area_Insert =profile.transform.GetChild(4);
            GetTexture(info.User_Image_Url);
            while(image==null)
            {
                await Task.Yield();
            }
            Area_Insert.GetChild(0).GetComponent<RawImage>().texture = image;
            for (int j = 0; j < Area_Insert.GetChild(1).childCount; j++)
            {
                Area_Insert.GetChild(1).GetChild(j).GetChild(0).GetComponent<Text>().text = info.HashTag[j];
            }
            profile.transform.GetChild(6).gameObject.GetComponent<Text>().text = info.User_Name;
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

    
}
//class new_PostData
//{
//    public string avatarName = null;
//    public string tag1 = "라이프스타일/취미";
//    public string tag2 = "캠핑/등산";
//    public string dummy1 = "dummy";
//    public string dummy2 = "dummy";
//    public b
//}