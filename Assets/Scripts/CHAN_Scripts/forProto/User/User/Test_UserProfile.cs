using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Test_UserProfile : MonoBehaviour
{
    public static Test_UserProfile instance;
    private void Awake()
    {
        instance = this;
    }
    // �������� ���������� �ε� �Ǵ��� �׽�Ʈ  

    public void Save(Profile_Info Container)
    {

        string json = JsonUtility.ToJson(Container, true);
        print(json);
        PlayerPrefs.SetString("ProfileSave", json);
        K_SaveSystem.Save("ProfileSave", json, true);

    }
    public void Load(Profile_Info Container)
    {
        if (PlayerPrefs.HasKey("ProfileSave"))
        {
            string json = PlayerPrefs.GetString("ProfileSave");
            json = K_SaveSystem.Load("ProfileSave");

            JsonUtility.FromJsonOverwrite(json, Container);

        }
    }
}
