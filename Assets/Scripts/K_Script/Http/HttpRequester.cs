using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequester
{
    //
    private readonly ISerializationOption _serializionOption;

    public HttpRequester(ISerializationOption serializionOption)
    {
        _serializionOption = serializionOption;
    }
    
    

    
    public async Task<TResultType> Get<TResultType>(string url)
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
    public async Task Post(string url, string json) //<TResultType> Get<TResultType>(string url)
    {
        try
        {
            string token = PlayerPrefs.GetString("PlayerToken");
            using var request = UnityWebRequest.Post(url, json);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", _serializionOption.ContentType);
            if(token != null)
            request.SetRequestHeader("Authorization", "Bearer " + token);


            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();



            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed: {request.error}");
                Debug.LogError($"Failed: {request.downloadHandler.text}");
            }
            var result = _serializionOption.Serialize(request.downloadHandler.text);
            if (token != null)
                SetToken(request.downloadHandler.text);


           

        }

        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");


        }

    }
   // private string token = null;
    int SetToken(string _input)
    {
        // 로그아웃시 토큰 초기화
        if (_input == null)
        {
            //token = null;
            return 0;
        }

        // 로그인시 토큰 설정
        string[] temp = _input.Split('"');

        if (temp[9] != "token")
            Debug.Log("ErrorCheck(-1001)"); // 토큰 형식 에러

        //token = temp[11];
        PlayerPrefs.SetString("PlayerToken", temp[11]);
        return 0;
    }

}
