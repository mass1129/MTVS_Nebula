using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
public class HttpRequester
{
    public Action onComplete;
    public Action onError;
    //
    private readonly ISerializationOption _serializionOption;

    public HttpRequester(ISerializationOption serializionOption)
    {
        _serializionOption = serializionOption;
    }
    
    

    
    public async UniTask<TResultType> Get<TResultType>(string url)
    {
        try
        {
            string token = PlayerPrefs.GetString("PlayerToken");
            using var request = UnityWebRequest.Get(url);

            request.SetRequestHeader("Content-Type", _serializionOption.ContentType);
            request.SetRequestHeader("Authorization", "Bearer " + token);


            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();



            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed: {request.error}");
            }



            var result = _serializionOption.Deserialize<TResultType>(request.downloadHandler.text);


            return result;
            //SceneManager.LoadScene(1);
        }

        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }


    }
    protected static double timeout = 300;
    public async UniTask Post(string url, string json) //<TResultType> Get<TResultType>(string url)
    {
        try
        {
            string token = PlayerPrefs.GetString("PlayerToken");
            using var request = UnityWebRequest.Post(url, json);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", _serializionOption.ContentType);
            if (token != null)
                request.SetRequestHeader("Authorization", "Bearer " + token);


            var operation = await request.SendWebRequest();

            while (!operation.isDone)
                await UniTask.Yield();



            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed: {request.error}");
                Debug.LogError($"Failed: {request.downloadHandler.text}");
                onError();
            }
            else
            {
                var result = _serializionOption.Serialize(request.downloadHandler.text);
                if (token != null)
                    SetToken(request.downloadHandler.text);
                onComplete();
            }





        }

        catch (Exception ex) when (ex.Message != "Index was outside the bounds of the array.")
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message} Json : {json}");


        }

    }
    public async UniTask<TResultType> Post1<TResultType>(string url, string json) //<TResultType> Get<TResultType>(string url)
    {
        try
        {
            string token = PlayerPrefs.GetString("PlayerToken");
            using var request = UnityWebRequest.Post(url, json);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", _serializionOption.ContentType);
            if (token != null)
                request.SetRequestHeader("Authorization", "Bearer " + token);


            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();



            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed: {request.error}");
            }
            var result = _serializionOption.Deserialize<TResultType>(request.downloadHandler.text);
            

            if (token != null)
                SetToken(request.downloadHandler.text);

            return result;


        }

        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
      
    }
    int SetToken(string _input)
    {
        // �α׾ƿ��� ��ū �ʱ�ȭ
        if (_input == null)
        {
            //token = null;
            return 0;
        }

        // �α��ν� ��ū ����
        string[] temp = _input.Split('"');

        if (temp[9] != "token")
            Debug.Log("ErrorCheck(-1001)"); // ��ū ���� ����

        //token = temp[11];
        PlayerPrefs.SetString("PlayerToken", temp[11]);
        return 0;
    }

}
