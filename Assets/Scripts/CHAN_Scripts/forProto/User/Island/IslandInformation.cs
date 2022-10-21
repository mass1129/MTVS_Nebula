using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class IslandInformation :MonoBehaviour, Server_IslandInfo
{
    public static IslandInformation instance;
    private void Awake()
    {
        instance = this;
    }
    // 유저들이 배치 될 좌표정보를 저장할 딕셔너리(Key: 유저 네임,Value: 좌표)
    public Dictionary<string, Vector3> island_Pos = new Dictionary<string, Vector3>();
    // 유저들이 커스텀한 하늘섬 정보를 저장할 딕셔너리(Key: 유저 네임,Value: 섬 타입)
    public Dictionary<string, string> island_Type = new Dictionary<string, string>();
    // 유저들의 프로필 이미지를 저장할 딕셔너리(Key: 유저 네임,Value:이미지URL)
    public Dictionary<string, string> User_image = new Dictionary<string, string>();
    // 유저들의 닉네임을 저장할 딕셔너리(Key: 유저 네임,Value:닉네임)
    public List<string> User_name = new List<string>();
    // islandSpawner에서 생성한 섬 오브젝트를 저장하는딕셔너리(Key: 유저 네임,Value:오브젝트정보)
    public Dictionary<string, GameObject> UserObj = new Dictionary<string, GameObject>();
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
    //csv파일 정보 로드 함수
    // 처음 하늘뷰에 들어왔을 때 발동되는 함수
    public void LoadFromCSV(string fileName)
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/" + fileName);
        // csv파일 인덱스가 끝났는지 판별
        bool endOfFile = false;
        // csv 파일의 목차부분 생략하기 위해쓰는 변수
        bool turn = false;
        int count = 0;
        while (!endOfFile)
        {
            count++;
            
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
            //만약 추가하고자 하는 정보가 이미 딕셔너리에 있으면 넘어간다.
            if (User_name.Contains(Parsing(data_values[3])))
            {
                continue;
            }
            //카테고리 숫자만큼 반복
            for (int i = 0; i < compare_category.Length; i++)
            {
                //만약 카테고리가 일치한다면
                if (data_values[data_values.Length-1].Contains(compare_category[i]))
                {
                    
                    
                    //섬 카테고리 딕셔너리에 해당 정보 저장
                    island_Type.Add(Parsing(data_values[3]), island_category[i]);
                    //유저 이름 저장
                    User_name.Add(Parsing(data_values[3]));
                    break;
                }
            }
            //csv의x,y,z값을 받아내고 Vector로 저장하자
            Vector3 temp_pos = new Vector3(float.Parse(data_values[0])* dis_multiplier, float.Parse(data_values[1])* dis_multiplier, float.Parse(data_values[2])* dis_multiplier);
            island_Pos.Add(Parsing(data_values[3]), temp_pos);
            User_image.Add(Parsing(data_values[3]), data_values[3]);
        }

    }
    // 임시 배열모음 (삭제 할 정보, 추가 할 정보)
    public string[] temp_Delete;
    public string[] temp_Add;
    //유저가 나갔다고 간주
    string Parsing(string s)
    {
        string[] s1=s.Split('/');
        return s1[4].Remove(s1[4].IndexOf('.')); 
    }


    public void RemoveInfo()
    {
        //삭제하고자 하는 정보길이만큼 반복
        for (int i = 0; i < temp_Delete.Length; i++)
        {
            UserObj.Remove(temp_Delete[i]);
            User_image.Remove(temp_Delete[i]);
            print(temp_Delete[i]+"삭제");
        }
    }
    //유저가 들어왔다고 간주
    public void AddInfo()
    {
        for (int i = 0; i < temp_Add.Length; i++)
        {
            print(Parsing(temp_Add[i]));
        }
    }

}
