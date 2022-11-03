using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//�Խù� ����
[Serializable]
public class PostData
{
    public int userId;
    public int id;
    public string title;
    public string body;
}

[Serializable]
public class PostDataArray
{
    public List<PostData> data;
}

public class UserData
{
    public string name;
    public string id;
   
  
}

public enum RequestType
{
    POST,
    GET,
    PUT,
    DELETE
}

public class HttpRequester : MonoBehaviour
{
    //��û Ÿ�� (GET, POST, PUT, DELETE)
    public RequestType requestType;
    //url   
    public string url;
   

    //Post Data 
    public string postData;//(body)

    //������ ���� �� ȣ������ �Լ� (Action)
    //Action : �Լ��� ���� �� �ִ� �ڷ���
    public Action<DownloadHandler> onComplete;
}