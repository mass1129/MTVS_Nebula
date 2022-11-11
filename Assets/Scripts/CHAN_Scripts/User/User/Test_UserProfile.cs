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
    private void Start()
    {
        Load();
    }
    public void OnClickLoadBtn()
    {
        // �����κ��� �޾ƿ´�.
         
    }

    public void OnClickDeleteBtn()
    {
        // �����κ��� �޾ƿ´�.
        
    }

    public async void Load()
    {
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/avatar";
        var httpRequest = new HttpRequester(new JsonSerializationOption());
        Root result = await httpRequest.Get<Root>(url);
        values = result.results;
        CreateProfile();
    }

    public async void Delete(string avatarName)
    {
        var uri ="http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/avatar/delete/"+avatarName;
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
            Debug.Log("������ ���� ����");
        }
    }
    public async void Post(string avatarName,List<string> hashTag, UnityEngine.Texture2D Image )
    {
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/avatar";
        WWWForm formData = new WWWForm();
        // �ƹ�Ÿ �г��� �Էº�
        formData.AddField("AvatarName", avatarName);
        // ���� �ܾ� �Էº�(4��)
         for (int i = 0; i < hashTag.Count; i++)
        {
            string tagName = "tag" + (i+1).ToString();
            formData.AddField(tagName, hashTag[i]);
        }
        byte[] bytes = Image.EncodeToPNG(); ;
        formData.AddBinaryData("image", bytes , "avatarName.png", "image/png");



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
            info.texture_info = i.texture;
            GameObject profile = Instantiate(profile_prefab, profile_pos);
            profile.GetComponentInChildren<Profile_Manager>().transfer(profile);
            profile.GetComponentInChildren<Profile_Manager>().temp_Info.texture_info = info.texture_info;
            profile.GetComponentInChildren<Profile_Manager>().temp_Info.User_Island_ID = info.User_Island_ID;
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
        Create_ProfileArea();
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

    void Create_ProfileArea()
    {
        if (values.Count < 3)
        { 
            GameObject profile = Instantiate(profile_prefab, profile_pos);
            profile.GetComponentInChildren<Profile_Manager>().transfer(profile);
            profile.SetActive(true);
            profile.GetComponentInChildren<Profile_Manager>().Initialize();
        }
    }
}
