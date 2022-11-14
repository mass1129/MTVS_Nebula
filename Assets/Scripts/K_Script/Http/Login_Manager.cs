using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class LoginInfo
{
    public string username;
    public string password;

}

public class Login_Manager : MonoBehaviour
{

    public InputField Input_Id;
    public InputField Input_Pass;
    public Text errorMassage;
    public string preUserName;
    public string prePassWord;


    private void Start()
    {
        API_Login();
    }
    public void ClickedLogInBtn()
    {

        GetAvatorInfo();

        //API_Login("ec2-43-201-62-61.ap-northeast-2.compute.amazonaws.com:8001/auth/login", json);
        SceneManager.LoadScene(1);
        
    }
    #region API_Func
    /// <summary>
    /// API로 로그인하여 토큰을 가져오는 함수
    /// 이때 가져온 토큰은 token 변수에 저장
    /// </summary>
    /// <returns>token = Gettoken</returns>




    public async void API_Login()
    {
        LoginInfo user = new LoginInfo
        {
            username = preUserName,
            password = prePassWord
        };
        string json = JsonUtility.ToJson(user, true);

        var url = "ec2-43-201-62-61.ap-northeast-2.compute.amazonaws.com:8001/auth/login";

        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);

    }
    public async void GetAvatorInfo()
    {
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/avatar";
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Av_Root result2 = await httpReq.Get<H_Av_Root>(url);
      
       PlayerPrefs.SetString("AvatarName", result2.results[0].name);
        
    }
    //public async void GetAvatorInfo()
    //{ 
        
    
    
    //}
    //public async void API_Login(string URL, string json)
    //{

        //    using var request = UnityWebRequest.Post(URL, json);
        //    {
        //        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        //        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        //        request.SetRequestHeader("Content-Type", "application/json");

        //        var operation = request.SendWebRequest();

        //        while (!operation.isDone)
        //            await Task.Yield();

        //        var jsonResponse = request.downloadHandler.text;

        //        if (request.result != UnityWebRequest.Result.Success)
        //        {
        //            Debug.LogError($"Failed: {request.error}");
        //        }

        //        try
        //        {
        //            Debug.Log($"Success: {request.downloadHandler.text}");
        //            SetToken(request.downloadHandler.text);
        //            SceneManager.LoadScene(1);
        //        }

        //        catch (Exception ex)
        //        {
        //            Debug.LogError($"{this}Could not parse response {jsonResponse}. {ex.Message}");
        //        }

        //    }

        //}

        /// <summary>
        /// API로 Logout을 하는 함수.
        /// 로그아웃시 가지고 있던 토큰값은 초기화됨.
        /// </summary>
        /// <returns>token = null</returns>
        //IEnumerator API_Logout()
        //{
        //    UnityWebRequest request;
        //    using (request = UnityWebRequest.Get("http://___/logout"))
        //    {   
        //        request.SetRequestHeader("Content-Type", "application/json");
        //        request.SetRequestHeader("Authorization", "Bearer " + token);
        //        yield return request.SendWebRequest();

        //        if (request.isNetworkError)
        //        {
        //            Debug.Log(request.error);
        //        }
        //        else
        //        {
        //            SetToken(null);
        //            if (request.responseCode != 200)
        //                ErrorCheck(-(int)request.responseCode, "API_Logout");
        //        }
        //    }
        //}
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



   
