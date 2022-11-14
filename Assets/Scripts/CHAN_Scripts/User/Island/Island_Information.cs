using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using IslandInfo;
[Serializable]
public class JsonInfo
{
    // 섬배치 정보
    public Vector3 island_Pos = new Vector3();
    // 섬 타입
    public string island_Type;
    // 유저 프로필 이미지 
    public string User_image;
    public string User_NickName;
    public string User_IslandId;
    // 하늘섬 오브젝트
    public GameObject User_Obj;


}

public class Island_Information :MonoBehaviourPun
{
    public static Island_Information instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Load();
        //LoadTest();
    }
    Parsing parsing = new Parsing();
    public Dictionary<string, JsonInfo> Island_Dic = new Dictionary<string, JsonInfo>();
    // 유저들의 닉네임을 저장할 리스트(Key:닉네임)
    public List<string> User_name = new List<string>();
    float dis_multiplier = 100;
    public string jsonFile;

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
            //InsertData(count, data_values[3], temp_pos, nickname);
              //csv의x,y,z값을 받아내고 Vector로 저장하자
            count++;
            await Task.Yield();
        }

    }
    // 이것은 테스트 전용 Json Load 함수

    public async void Load()
    {
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/skyisland";
        var httpRequest = new HttpRequester(new JsonSerializationOption());
        Root result = await httpRequest.Get<Root>(url);
        await parsing.LoadFromJson(result.results, Island_Dic, dis_multiplier);
        await parsing.InsertInfo(Island_Dic, Islands);
        Done = true;
        if (Done)
        {
            CHAN_GameManager.instance.LoadingObject.SetActive(false);
        }
    }
    public async void LoadTest()
    {
        Parsing parse = new Parsing();
        await parse.LoadFromJson_Test(Island_Dic, jsonFile, dis_multiplier);
        await parse.InserInfo_Test(Island_Dic, Islands);
        Done = true;
        if (Done)
        {
            CHAN_GameManager.instance.LoadingObject.SetActive(false);
        }
    }



    public void DeleteData()
    {
            foreach(JsonInfo i in Island_Dic.Values)
        {
            Destroy(i.User_Obj);
        }
        Island_Dic.Clear();
    }

}



