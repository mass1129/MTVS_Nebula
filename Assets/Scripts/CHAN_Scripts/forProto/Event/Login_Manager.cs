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
    // �α��� ������ ������ ������. 
    // ������ ������ �ִ��� �˻��Ѵ�. 
    // ���� ������ ������ ������ ���� ������ �����´�. 
    // Ʋ������ �߸� �Է��ߴٴ� ���( "�ùٸ� ������ �Է��� �ּ���")

    // �Է±�
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
    /// API�� �α����Ͽ� ��ū�� �������� �Լ�
    /// �̶� ������ ��ū�� token ������ ����
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
    /// API�� Logout�� �ϴ� �Լ�.
    /// �α׾ƿ��� ������ �ִ� ��ū���� �ʱ�ȭ��.
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
        // �α׾ƿ��� ��ū �ʱ�ȭ
        if (_input == null)
        {
            token = null;
            return 0;
        }
        Debug.Log(_input);
        // �α��ν� ��ū ����
        string[] temp = _input.Split('"');

        if ( temp[9] != "token")
            ErrorCheck(-1001); // ��ū ���� ����

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



   