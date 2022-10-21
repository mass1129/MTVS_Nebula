using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// Json 파일 커스텀으로 받기 위해 사용
using Newtonsoft.Json.Linq;

// json 과 직렬 통신하겠다는 뜻
[Serializable]
public class JsonInfo
{
    // Json에서 데이터를 추출하고 저장해야하는 데이터 모음
    public Vector3 island_Pos = new Vector3();
    public string island_Type;
    public string User_image;
    public GameObject User_Obj;
}
public class LoadJson : MonoBehaviour
{
    // json에서 추출한 데이터를 저장하는 딕셔너리 생성 (Key:UserName, Value:JsonInfo 클래스)
    public Dictionary<string, JsonInfo> Island_Dic = new Dictionary<string, JsonInfo>();
    public List<string> User_name = new List<string>();
    // 카테고리 비교 하는 배열
    string[] compare_category = { "cat", "dog", "animation", "celeb", "car" };
    string[] island_category = { "island 1", "island 2", "island 3", "island 4", "island 5" };
    //섬 사이 간격 구배
    float dis_multiplier = 100;

    // Json 파일 로드 
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
