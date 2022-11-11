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
    // 1.�ش���� ��������, ���� ������ �ε��Ѵ�.
    // 1-1.profileInfo ������Ʈ ���� 
    // 2.�޾ƿ� ������ ����������ŭ �ݺ�
    // 2-1 �ε��� ������ Profile Info�� �����Ѵ�. 
    // 2-2.Profile Object ���� 
    // 2-3.������ Profile�� ListProfile �� ��ġ��Ų��. 
    // 2-4.�ε��ؼ� ������ ������ ���� ������ ������Ʈ�� �� ���� �����Ѵ�. 
    public void OnClickLoadBtn()
    {
        // �����κ��� �޾ƿ´�.
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
        string tag1 = "��������Ÿ��/���";
        string tag2 = "ķ��/���";
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
            Debug.Log("Form ��� ���ε� ����");
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
//    public string tag1 = "��������Ÿ��/���";
//    public string tag2 = "ķ��/���";
//    public string dummy1 = "dummy";
//    public string dummy2 = "dummy";
//    public b
//}