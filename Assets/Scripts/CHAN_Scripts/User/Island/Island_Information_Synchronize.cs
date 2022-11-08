using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;

[Serializable]
public class JsonInfo_Synchronize
{
    // ����ġ ����
    public Vector3 island_Pos = new Vector3();
    // �� Ÿ��
    public string island_Type;
    // ���� ������ �̹��� 
    public string User_image;
    public string User_NickName;
    // �ϴü� ������Ʈ
    public GameObject User_Obj;
}

public class Island_Information_Synchronize : MonoBehaviourPun
{
    public static Island_Information_Synchronize instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //���� ������ Ŭ���̾�Ʈ�� �� �ش� ��������� �߻��ϵ��� �Ѵ�.
        if (PhotonNetwork.IsMasterClient)
        {
            Spawn("subset_30_v3_fin.csv");
        }
        //
        else
        {
        }


    }
    public Dictionary<string, JsonInfo_Synchronize> Island_Dic = new Dictionary<string, JsonInfo_Synchronize>();
    // �������� �г����� ������ ����Ʈ(Key:�г���)
    public List<string> User_name = new List<string>();
    // islandSpawner���� ������ �� ������Ʈ�� �����ϴµ�ųʸ�(Key: ���� ����,Value:������Ʈ����)
    // ī�װ� �� �ϴ� �迭
    string[] compare_category = { "cat", "dog", "animation", "celeb", "car" };
    string[] island_category = { "island 1", "island 2", "island 3", "island 4", "island 5" };
    //�� ���� ���� ����
    float dis_multiplier = 100;
    #region �������� ������ �������� �Լ� ����
    public void LoadIslandInfo()
    {
        // �������� �������� �ҷ��´�.
    }
    #endregion
    #region �������� ���� �����û��Ű�� �Լ� ����
    public void SaveIslandInfo()
    {
        //�������� �ϴü� ������ �����Ų��. 
    }
    #endregion
    #region �߰��� ������ �߰��ǰų� �������� ��, �̿�Ǵ� �Լ�����
    // �ӽ� �迭���� (���� �� ����, �߰� �� ����)
    public string[] temp_Delete;
    public string[] temp_Add;
    //������ �����ٰ� ����

    #endregion
    //csv���� ���� �ε� �Լ�, ó�� �ϴú信�� ������ �� �ߵ��ȴ�.
    public bool Done;
    public Transform Islands;
    public async Task LoadFromCSV(string fileName)
    {
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + fileName);
        // csv���� �ε����� �������� �Ǻ�
        bool endOfFile = false;
        // csv ������ �����κ� �����ϱ� ���ؾ��� ����
        bool turn = false;
        int count = 0;
        string nickname = null;
        while (!endOfFile)
        {
            // csv������ �� ���� ���� ���� data_string�� ����
            string data_string = sr.ReadLine();
            // ���� ���� ���ٸ�
            if (data_string == null)
            {
                //csv ���� �б� ��
                endOfFile = true;
                break;
            }
            // ������ ���ڿ��� , ���� ����� ����
            string[] data_values = data_string.Split(',');
            //�ѹ��� �����ҰŴ�.
            //������ data_value���� �������� �ʰ� �׳� ������.(�ʿ����.)
            if (!turn)
            {
                turn = true;
                continue;
            }
            Vector3 temp_pos = new Vector3(float.Parse(data_values[0]) * dis_multiplier, float.Parse(data_values[1]) * dis_multiplier, float.Parse(data_values[2]) * dis_multiplier);
            nickname = data_values[4];
            InsertData(count, data_values[3], temp_pos, nickname);
            //csv��x,y,z���� �޾Ƴ��� Vector�� ��������
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
            InsertData(i, url, pos);
        }
    }
    // �ϴü� ��ġ �Լ� 
    public async void Spawn(string fileName)
    {
        //LoadFromJson();
        await LoadFromCSV(fileName);
        await InsertInfo();
        Done = true;
    }
    public async void JustLoad(string fileName)
    {
        await LoadFromCSV(fileName);
        Done = true;
    }
    public async Task InsertInfo()
    {

        foreach (string i in Island_Dic.Keys)
        {

            JsonInfo_Synchronize info = Island_Dic[i];
            //GameObject island = InstantiateIsland(info.island_Type);.
            GameObject island = PhotonNetwork.Instantiate("CHAN_Resources/" + info.island_Type, info.island_Pos, Quaternion.identity);
            info.User_Obj = island;
            island.transform.position = info.island_Pos;
            //island.transform.GetChild(0).GetChild(0).GetComponent<Island_Profile>().user_name = i;
            island.transform.GetComponent<Island_Profile>().user_name = i;
            await Task.Yield();
        }
        //������ �̹��� url
        //user_name;

    }
    // �ϴü� ���� �ڵ�
    GameObject InstantiateIsland(string IslandType)
    {
        float randomScale = UnityEngine.Random.RandomRange(0.3f, 3);
        GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/" + IslandType), Islands);
        land.transform.GetChild(1).gameObject.transform.localScale *= randomScale;
        return land;
    }
    // �ϴü� Ÿ�� ���� �ڵ�
    string Return_IslandType(string s)
    {
        string s1 = null;
        for (int i = 0; i < compare_category.Length; i++)
        {
            //���� ī�װ��� ��ġ�Ѵٸ�
            if (Parsing(s) == compare_category[i])
            {
                s1 = island_category[i];
                break;
            }
        }
        return s1;
    }
    // url �ּҿ��� ī�װ� ���ڿ��� �����ϴ� �ڵ� 
    string Parsing(string s)
    {
        string[] s1 = s.Split('/');
        string[] s2 = s1[4].Split('-');
        return s2[1];

    }
    void InsertData(int i, string url, Vector3 pos, string nickname = "")
    {
        JsonInfo_Synchronize dic = new JsonInfo_Synchronize();
        //��ųʸ� �ε��� ����
        Island_Dic.Add(i.ToString(), dic);
        //��ųʸ��� ������ ��� �ִ´�. 
        dic = Island_Dic[i.ToString()];
        dic.island_Type = Return_IslandType(url);
        dic.island_Pos = pos;
        dic.User_image = url;
        dic.User_NickName = nickname;
    }

    // �߰��� ���ο� �����Ͱ� ������ ��, ��� �������� �����غ��� 
    // ������ �ʱ⿡ �������� ���� ��ǥ�� �޾ƿ´�. 
    // �� ��ǥ�� ������� ���� ��ġ�� �ȴ�. 

    //������ �ִ� ������ �������� ��� ������Ű�� �Լ�
    //�ϴü� ������Ʈ�� ��� �����Ѵ�.
    public void DeleteData()
    {
        foreach (JsonInfo_Synchronize i in Island_Dic.Values)
        {
            Destroy(i.User_Obj);
        }
        Island_Dic.Clear();
    }

}

