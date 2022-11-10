using System.Collections;
using System.Collections.Generic;
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
    public void OnClickBtn()
    {
        // �����κ��� �޾ƿ´�.
        Load();
        
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
    void CreateProfile()
    { 
        foreach(Result i in values)
        {
            ProfileInfo info = new ProfileInfo();
            info.User_Name = i.name;
            info.HashTag = i.hashTags;
            info.User_Image_Url = i.imageUrl;
            info.User_Island_ID = i.skyIslandId;
            GameObject profile = Instantiate(profile_prefab, profile_pos);
            Transform Area_Insert =profile.transform.GetChild(4);
            GetTexture(info.User_Image_Url);
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
