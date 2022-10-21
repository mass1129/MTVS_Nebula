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
    // 섬배치 정보
    public Vector3 island_Pos = new Vector3();
    // 섬 타입
    public string island_Type;
    // 유저 프로필 이미지 
    public string User_image;
    // 하늘섬 오브젝트
    public GameObject User_Obj;
}
public class LoadJson : MonoBehaviour
{
    // 싱글톤
    public static LoadJson instance;
    private void Awake()
    {
        instance = this;
    }

    // json에서 추출한 데이터를 저장하는 딕셔너리 생성 (Key:UserName, Value:JsonInfo 클래스)
    public Dictionary<string, JsonInfo> Island_Dic = new Dictionary<string, JsonInfo>();
    // 유저 이름 싱글톤
    public List<string> User_name = new List<string>();
    // 카테고리 비교 하는 배열
    string[] compare_category = { "cat", "dog", "animation", "celeb", "car" };
    string[] island_category = { "island 1", "island 2", "island 3", "island 4", "island 5" };
    //섬 사이 간격 구배
    float dis_multiplier = 100;
    private void Start()
    {
        LoadFromJson();
    }

    // Json 파일 로드 
    void LoadFromJson()
    {
        var info = Resources.Load<TextAsset>("DataSet/subset_30_v3");
        string jsonData = info.ToString();
        JObject jObject = JObject.Parse(jsonData);


        for (int i = 0; i < jObject.Count; i++)
        {
            //json의 하나의 인덱스를 가져왔다.
            JObject objPerIndex = jObject[i.ToString()].ToObject<JObject>();

            // 임시로 저장할 좌표인자, url
            float x = 0, y = 0, z = 0;
            string url;
            //x,y,z 좌표값 받아온다. 
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
            //받은 좌표인자값을 통해 Vector3 에 저장한다. 
            Vector3 pos = new Vector3(x * dis_multiplier, y * dis_multiplier, z * dis_multiplier);
            // URL 주소 문자열 추출
            url = objPerIndex["image_url"].ToString();
            //데이터 임시로 저장 할 클래스 생성
            JsonInfo dic = new JsonInfo();
            //딕셔너리 인덱스 생성
            Island_Dic.Add(i.ToString(), dic);
            //딕셔너리에 값들을 모두 넣는다. 
            dic = Island_Dic[i.ToString()];
            dic.island_Type = Return_IslandType(url);
            dic.island_Pos = pos;
            dic.User_image = url;
        }
    }
    // 하늘섬 배치 함수 
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
    // 하늘섬 생성 코드
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
            //만약 카테고리가 일치한다면
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
