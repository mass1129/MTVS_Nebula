using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

[System.Serializable]
public class JsonInfo
{
    // 섬배치 정보
    public Vector3 island_Pos = new Vector3();
    // 섬 타입
    public string island_Type;
    // 유저 프로필 이미지 
    public string User_image;
    public string User_NickName;
    // 하늘섬 오브젝트
    public GameObject User_Obj;
    
}

public class IslandInformation :MonoBehaviour, Server_IslandInfo
{
    public static IslandInformation instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Spawn();
    }
    public Dictionary<string, JsonInfo> Island_Dic = new Dictionary<string, JsonInfo>();
    // 유저들의 닉네임을 저장할 리스트(Key:닉네임)
    public List<string> User_name = new List<string>();
    // islandSpawner에서 생성한 섬 오브젝트를 저장하는딕셔너리(Key: 유저 네임,Value:오브젝트정보)
    // 카테고리 비교 하는 배열
    string[] compare_category = { "cat", "dog", "animation", "celeb", "car" };
    string[] island_category = { "island 1", "island 2", "island 3", "island 4", "island 5" };
    //섬 사이 간격 구배
    float dis_multiplier=100;
    #region 서버에게 정보를 가져오는 함수 모음
    public void LoadIslandInfo()
    {
        // 서버에게 섬정보를 불러온다.
    }
    #endregion
    #region 서버에게 정보 저장요청시키는 함수 모음
    public void SaveIslandInfo()
    {
        //서버에게 하늘섬 정보를 저장시킨다. 
    }
    #endregion
    #region 중간에 정보가 추가되거나 삭제됐을 때, 이용되는 함수모음
    // 임시 배열모음 (삭제 할 정보, 추가 할 정보)
    public string[] temp_Delete;
    public string[] temp_Add;
    //유저가 나갔다고 간주

    #endregion
    //csv파일 정보 로드 함수, 처음 하늘뷰에서 들어왔을 때 발동된다.
    public bool Done;
    public Transform Islands;
    public async Task LoadFromCSV(string fileName)
    {
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + fileName);
        // csv파일 인덱스가 끝났는지 판별
        bool endOfFile = false;
        // csv 파일의 목차부분 생략하기 위해쓰는 변수
        bool turn = false;
        int count = 0;
        string nickname = null;
        while (!endOfFile)
        {
            // csv파일의 한 줄을 읽은 값을 data_string에 저장
            string data_string = sr.ReadLine();
            // 만약 값이 없다면
            if (data_string == null)
            {
                //csv 파일 읽기 끝
                endOfFile = true;
                break;
            }
            // 추출한 문자열을 , 별로 나누어서 저장
            string[] data_values = data_string.Split(',');
            //한번만 실행할거다.
            //최초의 data_value값을 저장하지 않고 그냥 버린다.(필요없다.)
            if (!turn)
            {
                turn = true;
                continue;
            }
            Vector3 temp_pos = new Vector3(float.Parse(data_values[0])* dis_multiplier, float.Parse(data_values[1])* dis_multiplier, float.Parse(data_values[2])* dis_multiplier);
            nickname = data_values[4];
            InsertData(count, data_values[3], temp_pos, nickname);
              //csv의x,y,z값을 받아내고 Vector로 저장하자
            count++;
            await Task.Yield();

        }

    }
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
            InsertData(i, url, pos);
        }
    }
    // 하늘섬 배치 함수 
    public async void Spawn()
    {
        //LoadFromJson();
        await LoadFromCSV("test_FriendList.csv");
        await InsertInfo();
        Done = true;
    }
    public async Task InsertInfo()
    {
        
        foreach (string i in Island_Dic.Keys)
        {

            JsonInfo info = Island_Dic[i];
            GameObject island = InstantiateIsland(info.island_Type);
            info.User_Obj = island;
            island.transform.position = info.island_Pos;
            //island.transform.GetChild(0).GetChild(0).GetComponent<Island_Profile>().user_name = i;
            island.transform.GetComponent<Island_Profile>().user_name = i;
            await Task.Yield();
        }
    }
    // 하늘섬 생성 코드
    GameObject InstantiateIsland(string IslandType)
    {
        float randomScale=UnityEngine.Random.RandomRange(0.3f, 3);
        GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/" + IslandType), Islands);
        land.transform.GetChild(1).gameObject.transform.localScale *= randomScale;
        return land;
    }
    // 하늘섬 타입 결정 코드
    string Return_IslandType(string s)
    {
        string s1 = null;
        for (int i = 0; i < compare_category.Length; i++)
        {
            //만약 카테고리가 일치한다면
            if (Parsing(s) == compare_category[i])
            {
                s1 = island_category[i];
                break;
            }
        }
        return s1;
    }
    // url 주소에서 카테고리 문자열만 추출하는 코드 
    string Parsing(string s)
    {
        string[] s1 = s.Split('/');
        string[] s2 = s1[4].Split('-');
        return s2[1];

    }
    void InsertData(int i, string url, Vector3 pos,string nickname="")
    {
        JsonInfo dic = new JsonInfo();
        //딕셔너리 인덱스 생성
        Island_Dic.Add(i.ToString(), dic);
        //딕셔너리에 값들을 모두 넣는다. 
        dic = Island_Dic[i.ToString()];
        dic.island_Type = Return_IslandType(url);
        dic.island_Pos = pos;
        dic.User_image = url;
        dic.User_NickName = nickname;
    }

    // 중간에 새로운 데이터가 들어왔을 때, 어떻게 갱신할지 생각해보자 
    // 게임은 초기에 서버에게 섬의 좌표를 받아온다. 
    // 그 좌표를 기반으로 섬은 배치가 된다. 



}



