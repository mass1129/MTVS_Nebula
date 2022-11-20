using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class TestLoginInfo
{
    public string username;
    public string password;

}

public class K_TestLogin : MonoBehaviour
{

    public InputField Input_Id;
    public InputField Input_Pass;
    public Text errorMassage;
    public string preUserName;
    public string prePassWord;


    private void Start()
    {
        
        //GetAvatorInfo();
    }
    public void ClickedLogInBtn()
    {

        
        GetAvatorInfo();
       

       
    }
    public void login()
    {
        API_Login();
    }
    #region API_Func



    public async void API_Login()
    {
        TestLoginInfo user = new TestLoginInfo
        {
            username = "choigwuhyung2",
            password = "pwd"
        };
        string json = JsonUtility.ToJson(user, true);

        var url = "ec2-43-201-62-61.ap-northeast-2.compute.amazonaws.com:8001/auth/login";

        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);
        
        //SceneManager.LoadScene(1);
    }
    public async void API_Login2()
    {
        TestLoginInfo user = new TestLoginInfo
        {
            username = "choigwuhyung1",
            password = "pwd"
        };
        string json = JsonUtility.ToJson(user, true);

        var url = "ec2-43-201-62-61.ap-northeast-2.compute.amazonaws.com:8001/auth/login";

        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);

        //SceneManager.LoadScene(1);
    }
    public async void GetAvatorInfo()
    {
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/avatar";
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Av_Root result2 = await httpReq.Get<H_Av_Root>(url);

        PlayerPrefs.SetString("AvatarName", result2.results[0].name);
        PlayerPrefs.SetString("Island_ID", result2.results[0].id.ToString());
        PlayerPrefs.SetString("User_Island_ID", result2.results[0].id.ToString());
        //SceneManager.LoadScene(1);
    

}


    public class H_Av_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public List<H_Av_Result> results { get; set; }
    }
    public class H_Av_Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public List<string> hashTags { get; set; }
        public int skyIslandId { get; set; }
        public H_Av_Texture texture { get; set; }
    }

    public class H_Av_Texture
    {
        public List<double> TeethColor { get; set; }
        public List<double> OralCavityColor { get; set; }
        public H_Av_SelectedElements selectedElements { get; set; }
        public List<double> HairColor { get; set; }
        public int MaxLod { get; set; }
        public int MinLod { get; set; }
        public List<double> EyeColor { get; set; }
        public List<double> SkinColor { get; set; }
        public List<double> UnderpantsColor { get; set; }
        public string settingsName { get; set; }
        public double HeadSize { get; set; }
        public double Height { get; set; }
        public List<H_Av_Blendshape> blendshapes { get; set; }
    }

    public class H_Av_Blendshape
    {
        public string blendshapeName { get; set; }
        public int type { get; set; }
        public double value { get; set; }
        public int group { get; set; }
    }




    public class H_Av_SelectedElements
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




    #endregion
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("PlayerToken", null);
    }

    #region Occur Error
    int ErrorCheck(int _code)
    {
        if (_code > 0) return 0;
        else if (_code == -1000) Debug.LogError(_code + ", Internet Connect Error");
        else if (_code == -1001) Debug.LogError(_code + ", Occur token type Error");
        else if (_code == -1002) Debug.LogError(_code + ", Category type Error");
        else if (_code == -1003) Debug.LogError(_code + ", Item type Error");
        else Debug.LogError(_code + ", Undefined Error");
        return _code;
    }

    int ErrorCheck(int _code, string _funcName)
    {
        if (_code > 0) return 0;
        else if (_code == -400) Debug.LogError(_code + ", Invalid request in " + _funcName);
        else if (_code == -401) Debug.LogError(_code + ", Unauthorized in " + _funcName);
        else if (_code == -404) Debug.LogError(_code + ", not found in " + _funcName);
        else if (_code == -500) Debug.LogError(_code + ", Internal Server Error in " + _funcName);
        else Debug.LogError(_code + ", Undefined Error");
        return _code;
    }
    #endregion
}
