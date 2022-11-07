using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequester
{
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
}
