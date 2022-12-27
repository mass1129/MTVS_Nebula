using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
[System.Serializable]
public class LoginInfo
{
    public string username;
    public string password;

}

public class Login_Manager : MonoBehaviour
{

    public TMP_InputField Input_Id;
    public TMP_InputField Input_Pass;
    public Text errorMassage;
    public string preUserName;
    public string prePassWord;
    InputFieldTabManager inputFieldTabMrg;
    bool isLoaing;

    private void Start()
    {
        inputFieldTabMrg = new InputFieldTabManager();
        inputFieldTabMrg.Add(Input_Id);
        inputFieldTabMrg.Add(Input_Pass);
        Input_Id.Select();
        inputFieldTabMrg.SetFocus(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Input_Id.text = preUserName;
            Input_Pass.text = prePassWord;
        }
        inputFieldTabMrg.CheckFocus();
    }
    public void ClickedLogInBtn()
    {

        API_Login().Forget();
  
    }
    #region API_Func



    public async UniTask API_Login()
    {
        LoginInfo user = new LoginInfo
        {
            username = Input_Id.text,
            password = Input_Pass.text
        };
        string json = JsonUtility.ToJson(user, true);

        var url = "https://auth.mtvs-nebula.com/auth/login";
 

        var httpReq = new HttpRequester(new JsonSerializationOption());

        
        httpReq.onError = () =>
        {
            //여기서 오류 팝업 나오도록 설정
            Debug.Log("로그인 실패");
            errorMassage.text = "아이디/비밀번호 입력이 틀립니다. 다시 입력해 주십시오.";
            Input_Id.text = "";
            Input_Pass.text = "";

        };
        httpReq.onComplete = () =>
        {
            SceneManager.LoadScene(1);
        };
        await httpReq.Post(url, json);

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



   
