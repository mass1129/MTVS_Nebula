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
    // ����ġ ����
    public Vector3 island_Pos = new Vector3();
    // �� Ÿ��
    public string island_Type;
    // ���� ������ �̹��� 
    public string User_image;
    // �ϴü� ������Ʈ
    public GameObject User_Obj;
}
public class LoadJson : MonoBehaviour
{
    // �̱���
    public static LoadJson instance;
    private void Awake()
    {
        instance = this;
    }

    // json���� ������ �����͸� �����ϴ� ��ųʸ� ���� (Key:UserName, Value:JsonInfo Ŭ����)
    public Dictionary<string, JsonInfo> Island_Dic = new Dictionary<string, JsonInfo>();
    // ���� �̸� �̱���
    public List<string> User_name = new List<string>();
    // ī�װ� �� �ϴ� �迭
    string[] compare_category = { "cat", "dog", "animation", "celeb", "car" };
    string[] island_category = { "island 1", "island 2", "island 3", "island 4", "island 5" };
    //�� ���� ���� ����
    float dis_multiplier = 100;
    private void Start()
    {
        LoadFromJson();
    }

    // Json ���� �ε� 
    void LoadFromJson()
    {
        var info = Resources.Load<TextAsset>("DataSet/subset_30_v3");
        string jsonData = info.ToString();
        JObject jObject = JObject.Parse(jsonData);


        for (int i = 0; i < jObject.Count; i++)
        {
            //json�� �ϳ��� �ε����� �����Դ�.
            JObject objPerIndex = jObject[i.ToString()].ToObject<JObject>();

            // �ӽ÷� ������ ��ǥ����, url
            float x = 0, y = 0, z = 0;
            string url;
            //x,y,z ��ǥ�� �޾ƿ´�. 
            for (int j = 0; j < 3; j++)
            {
                string pc = "pc" + (j + 1).ToString();
                objPerIndex[pc].ToObject<float>();
                if (j == 0)
                { x = objPerIndex[pc].ToObject<float>(); }
                else if (j == 1)
                { y = objPerIndex[pc].ToObject<float>(); }
                else
                { z = objPerIndex[pc].ToObject<float>(); }
            }
            //���� ��ǥ���ڰ��� ���� Vector3 �� �����Ѵ�. 
            Vector3 pos = new Vector3(x * dis_multiplier, y * dis_multiplier, z * dis_multiplier);
            // URL �ּ� ���ڿ� ����
            url = objPerIndex["image_url"].ToString();
            //������ �ӽ÷� ���� �� Ŭ���� ����
            JsonInfo dic = new JsonInfo();
            //��ųʸ� �ε��� ����
            Island_Dic.Add(i.ToString(), dic);
            //��ųʸ��� ������ ��� �ִ´�. 
            dic = Island_Dic[i.ToString()];
            dic.island_Type = Return_IslandType(url);
            dic.island_Pos = pos;
            dic.User_image = url;
        }
    }
    // �ϴü� ��ġ �Լ� 
    public void Spawn()
    {
        foreach (string i in Island_Dic.Keys)
        {
            
            JsonInfo info = Island_Dic[i];
            GameObject island = InstantiateIsland(info.island_Type);
            info.User_Obj = island;
            island.transform.position = info.island_Pos;
            island.transform.GetChild(0).GetChild(0).GetComponent<Island_Profile>().user_name = i;
        }
    }
    // �ϴü� ���� �ڵ�
    GameObject InstantiateIsland(string IslandType)
    {
        GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/" + IslandType));
        return land;
    }
    string Return_IslandType(string s)
    {
        string s1 = null;
        for (int i = 0; i < compare_category.Length; i++)
        {
            //���� ī�װ��� ��ġ�Ѵٸ�
            if (Parsing(s)== compare_category[i])
            {
                s1 = island_category[i];
                break;
            }
        }
        return s1;
    }
    string Parsing(string s)
    {
        string[] s1 = s.Split('/');
        string[] s2 = s1[4].Split('-');
        return s2[1];
        
    }
}
