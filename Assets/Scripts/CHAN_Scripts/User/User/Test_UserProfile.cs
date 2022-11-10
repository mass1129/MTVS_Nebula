using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Test_UserProfile : MonoBehaviour
{
    public static Test_UserProfile instance;
    private void Awake()
    {
        instance = this;
    }
    // 유저정보 정상적으로 로드 되는지 테스트  
    public void OnClickBtn()
    {
        Load();
    }
    public void Save(Profile_Info Container)
    {

        string json = JsonUtility.ToJson(Container, true);
        print(json);
        PlayerPrefs.SetString("ProfileSave", json);
        K_SaveSystem.Save("ProfileSave", json, true);

    }
    public async void Load()
    {
        List<Result> values = new List<Result>();
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/avatar";
        var httpRequest = new HttpRequester(new JsonSerializationOption());
        Root result = await httpRequest.Get<Root>(url);

        values = result.results;
        foreach (Result i in values)
        {
            print(i.id);
            print(i.name);
            print(i.imageUrl);
            print(i.hashTags);
            print(i.skyIslandId);
        }
        
    }
    public class Blendshape
    {
        public string blendshapeName { get; set; }
        public int type { get; set; }
        public double value { get; set; }
        public int group { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public List<string> hashTags { get; set; }
        public int skyIslandId { get; set; }
        public Texture texture { get; set; }
    }

    public class Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public List<Result> results { get; set; }
    }

    public class SelectedElements
    {
        public int Hair { get; set; }
        public int Beard { get; set; }
        public int Accessory { get; set; }
        public int Shirt { get; set; }
        public int Item1 { get; set; }
        public int Pants { get; set; }
        public int Hat { get; set; }
        public int Shoes { get; set; }
    }

    public class Texture
    {
        public List<double> TeethColor { get; set; }
        public List<double> OralCavityColor { get; set; }
        public SelectedElements selectedElements { get; set; }
        public List<double> HairColor { get; set; }
        public int MaxLod { get; set; }
        public int MinLod { get; set; }
        public List<double> EyeColor { get; set; }
        public List<double> SkinColor { get; set; }
        public List<double> UnderpantsColor { get; set; }
        public string settingsName { get; set; }
        public double HeadSize { get; set; }
        public double Height { get; set; }
        public List<Blendshape> blendshapes { get; set; }
    }

}
