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
    // 1.�ش���� ��������, ���� ������ �ε��Ѵ�.
    // 1-1.profileInfo ������Ʈ ���� 
    // 2.�޾ƿ� ������ ����������ŭ �ݺ�
    // 2-1 �ε��� ������ Profile Info�� �����Ѵ�. 
    // 2-2.Profile Object ���� 
    // 2-3.������ Profile�� ListProfile �� ��ġ��Ų��. 
    // 2-4.�ε��ؼ� ������ ������ ���� ������ ������Ʈ�� �� ���� �����Ѵ�. 
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
            Debug.Log("������ ���� ����");
        }
        www.Dispose();
    }
    public async void Post(string avatarName,List<string> hashTag, UnityEngine.Texture2D Image )
    {
        var url = "https://resource.mtvs-nebula.com/avatar";
        WWWForm formData = new WWWForm();
        // �ƹ�Ÿ �г��� �Էº�
        formData.AddField("AvatarName", avatarName);
        // ���� �ܾ� �Էº�(4��)
         for (int i = 0; i < hashTag.Count; i++)
        {
            string tagName = "tag" + (i+1).ToString();
            formData.AddField(tagName, hashTag[i]);
        }
        if (PlayerPrefs.GetString("extension") == ".png")
        {
            byte[] bytes = Image.EncodeToPNG();
            formData.AddBinaryData("image", bytes, avatarName+".png", "image/png");
            Debug.Log("PNG ���ڵ�");
        }
        else if (PlayerPrefs.GetString("extension") == ".jpg")
        {
            byte[] bytes = Image.EncodeToJPG();
            formData.AddBinaryData("image", bytes, avatarName + ".jpg", "image/jpg");
            Debug.Log("jpg ���ڵ�");
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
            Debug.Log("Form ��� ���ε� ����");
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
            //������ ������Ʈ ���� �κ�
            GameObject profile = Instantiate(profile_prefab, profile_pos);
            profile.GetComponentInChildren<Profile_Manager>().transfer(profile);
            profile.GetComponentInChildren<Profile_Manager>().temp_Info.texture_info = info.texture_info;
            profile.GetComponentInChildren<Profile_Manager>().temp_Info.User_Island_ID = info.User_Island_ID;
            Transform Area_Insert =profile.transform.GetChild(4);
            //�̹��� �ҷ����� �Լ� 
            GetTexture(info.User_Image_Url);
            //�̹����� �ҷ��� �� ���� ��� ����
            while(image==null)
            {
                await Task.Yield();
            }
            //�̹��� �ҷ������� �̹��� ���Զ��� ����
            Area_Insert.GetChild(0).GetComponent<RawImage>().texture = image;
            //Ű���� �ִ� �ڵ�
            for (int j = 0; j < Area_Insert.GetChild(1).childCount; j++)
            {
                Area_Insert.GetChild(1).GetChild(j).GetComponentInChildren<Text>().text = info.HashTag[j];
            }
            profile.transform.GetChild(6).gameObject.GetComponent<Text>().text = info.User_Name;

            await Task.Yield();
        }
        // ���ο� ������ ������ ����
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
            //������ ������Ʈ ���� �κ�
            GameObject profile = Instantiate(profile_prefab, profile_pos);
            Profile_Manager_New mgr = profile.GetComponentInChildren<Profile_Manager_New>();
            mgr.temp_Info.texture_info = info.texture_info;
            mgr.temp_Info.User_Island_ID = info.User_Island_ID;
            //�̹��� �ҷ����� �Լ� 
            GetTexture(info.User_Image_Url);
            //�̹����� �ҷ��� �� ���� ��� ����
            while (image == null)
            {
                await Task.Yield();
            }
            //�̹��� �ҷ������� �̹��� ���Զ��� ����
            profile.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<RawImage>().texture = image;
            //mgr.user_Image = image;
            //Ű���� �ִ� �ڵ�
            for (int j = 0; j < mgr.keywords.Length; j++)
            {
                mgr.keywords[j].text= info.HashTag[j];
            }
            mgr.user_Nickname.text = info.User_Name;
            await Task.Yield();
        }
        // ���ο� ������ ������ ����
        Create_ProfileArea();
    }
    public void UpdateProfile(string userName, Texture texture, string[] _keywords)
    {
        GameObject profile = Instantiate(profile_prefab, profile_pos);
        Profile_Manager_New info = profile.GetComponent<Profile_Manager_New>();
        //������Ʈ ����� 
        // ������Ʈ�� �� �κп� edit���� ������������ ���
        //Ŭ������ ����� ������ �ش�  UI�� �����Ѵ�. 
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
            // ������ �߰� �������� �����Ѵ�. (���� ������)
          
            GameObject profile = Instantiate(new_profile_prefab, profile_pos);
            //profile.GetComponentInChildren<Profile_Manager>().transfer(profile);
            //profile.SetActive(true);
            //profile.GetComponentInChildren<Profile_Manager>().Initialize();
        }
        
        //LoadingScene.SetActive(false);
    }
}
