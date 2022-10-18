using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class JsonData
{
    //public JsonData(float _x, float _y, float _z, string _jpg_Path)
    //{
    //    x = _x;
    //    y = _y;
    //    z = _z;
    //    jpg_Path = _jpg_Path;
    //}
}
class PositionData
{
    public List<float> x = new List<float>();
    public List<float> y = new List<float>();
    public List<float> z = new List<float>();
    public List<string> controls = new List<string>();
    public string[] devides = { "cat", "dog", "animation", "celeb", "car" };
}
public class LoadJson : MonoBehaviour
{
    public static LoadJson instance;

    public List<Vector3> positions = new List<Vector3>();
    public string[] islands = { "island 1", "island 2", "island 3", "island 4", "island 5" };
    public List<string> Islands = new List<string>();
    void Awake()
    {
        instance = this;
        csvLoad();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //void  dataLoad()
    //{
    //    // json파일을  모두 string으로 가져오고
    //    // 
    //    string loadFilePath = "Resources/DataSet(Test)/subset30";
    //    string loadFile = File.ReadAllText(loadFilePath);
    //    jsonData = JsonUtility.FromJson<JsonData>(loadFile); 
    //}
    void csvLoad()
    {
        PositionData posData = new PositionData();
        StreamReader sr = new StreamReader(Application.dataPath + "/" + "subset500_pc4_v2.csv");
        bool endOfFile=false;
        bool turn=false;
        string control=null;
        while (!endOfFile)
        {
            string data_String = sr.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
            string[] data_values = data_String.Split(',');
            if (!turn)
            {
                turn = true;
                continue;
            }
            
            for (int i = 0; i < posData.devides.Length; i++)
            {
                if (data_values[4].Contains(posData.devides[i]))
                {
                    control = islands[i];
                    break;
                }
            }
            
            posData.x.Add(float.Parse(data_values[0]));
            posData.y.Add(float.Parse(data_values[1]));
            posData.z.Add(float.Parse(data_values[2]));
            //posData.controls.Add(control);
            Islands.Add(control);
            
            
        }
        for (int i = 0; i < posData.x.Count; i++)
        {
            //print($"x:{posData.x[i]} y:{posData.y[i]} z:{posData.z[i]}");
            Vector3 vector = new Vector3(posData.x[i]*50, posData.y[i]*50, posData.z[i]*50);
            positions.Add(vector);
            //print(posData.controls[i]);
        }

    }
}
