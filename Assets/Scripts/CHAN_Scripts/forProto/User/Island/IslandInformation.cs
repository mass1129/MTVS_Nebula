using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class IslandInformation : Server_IslandInfo
{
    //Vector3.x
    public List<float> x = new List<float>();
    //Vector3.y
    public List<float> y = new List<float>();
    //Vector3.z
    public List<float> z = new List<float>();
    // csv에서 저장시킬 딕셔너리 모음
    public Dictionary<string, Vector3> island_Pos = new Dictionary<string, Vector3>();
    public Dictionary<string, string> island_Type = new Dictionary<string, string>();
    public Dictionary<string,string> User_image = new Dictionary<string, string>();
    // 카테고리 비교 하는 배열
    public string[] compare_category = { "cat", "dog", "animation", "celeb", "car" };
    string[] island_category = { "island 1", "island 2", "island 3", "island 4", "island 5" };
    #region 서버에게 정보를 가져오는 함수 모음
    public void LoadIslandInfo()
    {
        // 서버에게 섬정보를 저장시킨다.
    }
    #endregion
    #region 서버에게 정보 저장요청시키는 함수 모음
    public void SaveIslandInfo()
    {
        //서버에게 하늘섬 정보를 저장시킨다. 
    }
    #endregion

    void LoadFromCSV(string fileName)
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
            //카테고리 숫자만큼 반복
            for (int i = 0; i < compare_category.Length; i++)
            {
                //만약 카테고리가 발견한다면
                if (data_values[4].Contains(compare_category[i]))
                {
                    //섬 카테고리 딕셔너리에 해당 정보 저장
                    island_Type.Add(count.ToString(), island_category[i]);
                }

            }

        }
    }

}
