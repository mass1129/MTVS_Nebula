using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System;
[System.Serializable]
public class LoginInfo
{
    public string username;
    public string password;

}

public class Login_Manager : MonoBehaviour
{
    // 로그인 정보를 서버에 보낸다. 
    // 정보가 서버에 있는지 검사한다. 
    // 만약 정보가 서버에 있으면 유저 정보를 가져온다. 
    // 틀렸으면 잘못 입력했다는 경고( "올바른 정보를 입력해 주세요")

    // 입력기
    public InputField Input_Id;
    public InputField Input_Pass;
    public Text errorMassage;
    private string token = null;

    public void ClickedLogInBtn()
    {
        LoginInfo user = new LoginInfo
        {
            username = Input_Id.text,
            password = Input_Pass.text
        };
        string json = JsonUtility.ToJson(user,true);

        
        StartCoroutine(API_Login("ec2-43-201-62-61.ap-northeast-2.compute.amazonaws.com:8001/auth/login", json));

    }




    #region API_Func
    /// <summary>
    /// API로 로그인하여 토큰을 가져오는 함수
    /// 이때 가져온 토큰은 token 변수에 저장
    /// </summary>
    /// <returns>token = Gettoken</returns>


    IEnumerator API_Login(string URL, string json)
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Post(URL, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            Debug.Log(jsonToSend);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                SetToken(request.downloadHandler.text);
                Debug.Log(token);
                //if (request.responseCode != 200)
                   // ErrorCheck(-(int)request.responseCode, "API_Login");
            }
        }
    }

    /// <summary>
    /// API로 Logout을 하는 함수.
    /// 로그아웃시 가지고 있던 토큰값은 초기화됨.
    /// </summary>
    /// <returns>token = null</returns>
    IEnumerator API_Logout()
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Get("http://___/logout"))
        {   
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                SetToken(null);
                if (request.responseCode != 200)
                    ErrorCheck(-(int)request.responseCode, "API_Logout");
            }
        }
    }

   

    int SetToken(string _input)
    {
        // 로그아웃시 토큰 초기화
        if (_input == null)
        {
            token = null;
            return 0;
        }
        Debug.Log(_input);
        // 로그인시 토큰 설정
        string[] temp = _input.Split('"');

        if ( temp[9] != "token")
            ErrorCheck(-1001); // 토큰 형식 에러

        token = temp[11];
        return 0;
    }
    #endregion

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



   