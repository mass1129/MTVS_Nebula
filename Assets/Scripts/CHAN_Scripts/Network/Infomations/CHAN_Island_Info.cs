using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IslandInfo
{
    class Categories
    {
        public string[] compare_category = { "요리/레시피", "해외여행", "캠핑/등산" };
        public string[] island_category = { "Island_Backyard", "Island_Beach", "Island_Cave", "Island_House", "Island_Pond" };
        public string[] temp_UserName = {"우앙" };
    }
    public class Result
    {
        public string avatarName { get; set; }
        public double pc1 { get; set; }
        public double pc2 { get; set; }
        public double pc3 { get; set; }
        public string keyword1 { get; set; }
        public string keyword2 { get; set; }
        public string image_url { get; set; }
    }


    public class Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public Dictionary<string, Result> results { get; set; }
    }
    public class Parsing
    {
        // 테스트용 데이터 로드 함수
        public async Task LoadFromJson_Test(Dictionary<string, JsonInfo> Island_Dic,string fileName, float dis_multiplier)
        {
            // 저장된 json을 읽어온다.
            var info = Resources.Load<TextAsset>(fileName);
            // json을 문자열 자료형식으로 변환한다.
            string jsonData = info.ToString();
            // 커스텀으로 자료형식을 추출하기위한 밑작업
            JObject jObject = JObject.Parse(jsonData);
            for (int i = 0; i < jObject.Count; i++)
            {
                //json의 하나의 인덱스를 가져왔다.
                JObject objPerIndex = jObject[i.ToString()].ToObject<JObject>();

                // 임시로 저장할 좌표인자, url
                float x = 0, y = 0, z = 0;
                string url;
                string keyword1 = null, keyword2 = null;
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
                keyword1 = objPerIndex["keyword1"].ToString();
                keyword2 = objPerIndex["keyword2"].ToString();
                string NickName = i.ToString();
                InsertData(Island_Dic, i.ToString(), url, NickName, pos, keyword1, keyword2);
                await Task.Yield();
            }
        }
        public async Task LoadFromJson(Dictionary<string, Result> results, Dictionary<string, JsonInfo> Island_Dic,float dis_multiplier)
        {
            foreach (KeyValuePair<string, Result> item in results)
            {
                string key = item.Key;
                Result value = item.Value;
                string nickName = value.avatarName;
                Vector3 pos = new Vector3((float)value.pc1 * dis_multiplier, (float)value.pc2 * dis_multiplier, (float)value.pc3 * dis_multiplier);
                string url = value.image_url;
                string keyword1 = value.keyword1;
                string keyword2 = value.keyword2;
                InsertData(Island_Dic, key,nickName, url, pos, keyword1, keyword2);
                await Task.Yield();
            }
           
        }
        public void InsertData(Dictionary<string,JsonInfo> Island_Dic,string key,string nickName, string url, Vector3 pos, string keyword1 = "", string keyword2 = "")
        {
            JsonInfo dic = new JsonInfo();
            //딕셔너리 인덱스 생성
            Island_Dic.Add(key, dic);
            //딕셔너리에 값들을 모두 넣는다. 
            dic = Island_Dic[key];
            dic.User_IslandId = key;
            dic.island_Type = Return_IslandType(keyword2);
            dic.island_Pos = pos;
            dic.User_image = url;
            dic.User_NickName = nickName;
        }
        string Return_IslandType(string s)
        {
            string s1 = null;
            Categories category = new Categories();
            for (int i = 0; i < category.compare_category.Length; i++)
            {
                //만약 카테고리가 일치한다면
                if (s == category.compare_category[i])
                {
                    s1 = category.island_category[i];
                    break;
                }
            }
            return s1;
        }
        public async Task InsertInfo( Dictionary<string, JsonInfo> Island_Dic,Transform Islands)
        {

            foreach (string i in Island_Dic.Keys)
            {

                JsonInfo info = Island_Dic[i];
                string temp_user = info.User_NickName;
                if (info.island_Type == null)
                {
                    Island_Dic.Remove(i);
                    break;
                }
                GameObject island = InstantiateIsland(info.island_Type, Islands, temp_user);
                info.User_Obj = island;
                island.transform.position = info.island_Pos;
                island.transform.GetComponent<Island_Profile>().user_name = info.User_NickName;
                island.transform.GetComponent<Island_Profile>().user_Url = info.User_image;
                island.transform.GetComponent<Island_Profile>().user_IslandID = info.User_IslandId;
                await Task.Yield();
            }
        }
        public async Task InserInfo_Test(Dictionary<string, JsonInfo> Island_Dic, Transform Islands)
        {
            foreach (string i in Island_Dic.Keys)
            {
                JsonInfo info = Island_Dic[i];
                
                GameObject island = InstantiateIsland(info.island_Type, Islands, "temp_user");
                info.User_Obj = island;
                island.transform.position = info.island_Pos;
                island.transform.GetComponent<Island_Profile>().user_name = i;
                
                await Task.Yield();
            }
        }
        GameObject InstantiateIsland(string IslandType,Transform Islands,string username)
        {
            GameObject land = GameObject.Instantiate(Resources.Load<GameObject>("CHAN_Resources/" + IslandType), Islands);
            if (username == "요희")
            {
                land.transform.gameObject.transform.localScale *= 3;
            }
            //float randomScale = Random.Range(0.3f, 3);
            //land.transform.GetChild(0).gameObject.transform.localScale *= randomScale;
            return land;
        }
    }



}