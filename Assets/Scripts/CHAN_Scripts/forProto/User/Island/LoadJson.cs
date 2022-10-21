using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// Json ���� Ŀ�������� �ޱ� ���� ���
using Newtonsoft.Json.Linq;

// json �� ���� ����ϰڴٴ� ��
[Serializable]
public class JsonInfo
{
    // Json���� �����͸� �����ϰ� �����ؾ��ϴ� ������ ����
    public Vector3 island_Pos = new Vector3();
    public string island_Type;
    public string User_image;
    public GameObject User_Obj;
}
public class LoadJson : MonoBehaviour
{
    // json���� ������ �����͸� �����ϴ� ��ųʸ� ���� (Key:UserName, Value:JsonInfo Ŭ����)
    public Dictionary<string, JsonInfo> Island_Dic = new Dictionary<string, JsonInfo>();
    public List<string> User_name = new List<string>();
    // ī�װ� �� �ϴ� �迭
    string[] compare_category = { "cat", "dog", "animation", "celeb", "car" };
    string[] island_category = { "island 1", "island 2", "island 3", "island 4", "island 5" };
    //�� ���� ���� ����
    float dis_multiplier = 100;

    // Json ���� �ε� 
    void LoadFromJson()
    {
        var info = Resources.Load<TextAsset>("DataSet/subset30");
        string jsonData = info.ToString();
        JObject jObject = JObject.Parse(jsonData);


        JObject pc1 = jObject[0].ToObject<JObject>();
        JObject pc2 = jObject[1].ToObject<JObject>();
        JObject pc3 = jObject[2].ToObject<JObject>();
        JObject file_name = jObject[3].ToObject<JObject>();
        for (int i = 0; i < file_name.Count; i++)
        {
            User_name.Add(Parsing(file_name[i].ToString()));
        }
        Island_Dic


    }
    string Parsing(string s)
    {
        string[] s1 = s.Split('/');
        return s1[4].Remove(s1[4].IndexOf('.'));
        
    }
}
