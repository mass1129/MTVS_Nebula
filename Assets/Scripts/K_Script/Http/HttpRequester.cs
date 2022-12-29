using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
public class HttpRequester
{
    public Action<HttpRequester> onComplete;
    public Action<HttpRequester> onError;

    private readonly ISerializationOption _serializionOption;

    public HttpRequester(ISerializationOption serializionOption)
    {
        _serializionOption = serializionOption;
    }
    
    

    
    public async UniTask<TResultType> Get<TResultType>(string url)
    {
        using var request = UnityWebRequest.Get(url);
        try
        {
            string token = PlayerPrefs.GetString("PlayerToken");


            request.SetRequestHeader("Content-Type", _serializionOption.ContentType);
            request.SetRequestHeader("Authorization", "Bearer " + token);


            var operation = await request.SendWebRequest();

            var result = _serializionOption.Deserialize<TResultType>(request.downloadHandler.text);


            return result;
            //SceneManager.LoadScene(1);
        }

        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
#endif
            return default;
        }
        finally
        {
            request.Dispose();
        }

    }
    protected static double timeout = 300;
    public async UniTask Post(string url, string json) //<TResultType> Get<TResultType>(string url)
    {
        using var request = UnityWebRequest.Post(url, json);
        try
        {
            string token = PlayerPrefs.GetString("PlayerToken");
            

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", _serializionOption.ContentType);
            if (token != null)
                request.SetRequestHeader("Authorization", "Bearer " + token);


            var operation = await request.SendWebRequest();

           
             SetToken(request.downloadHandler.text);
            onComplete?.Invoke(this);
        }

        catch (Exception ex) //when (ex.Message != "Index was outside the bounds of the array.")
        {
#if UNITY_EDITOR
            Debug.LogError($"{nameof(Post)} failed: {ex.Message} Json : {json}");
#endif
                onError?.Invoke(this);
        }
        finally
        {
            request.Dispose();
        }

    }
    public async UniTask<TResultType> Post1<TResultType>(string url, string json)
    {
        using var request = UnityWebRequest.Post(url, json);
        try
        {
            string token = PlayerPrefs.GetString("PlayerToken");

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", _serializionOption.ContentType);
            if (token != null)
                request.SetRequestHeader("Authorization", "Bearer " + token);


            var operation = await request.SendWebRequest();

            var result = _serializionOption.Deserialize<TResultType>(request.downloadHandler.text);
            
            return result;

        }

        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"{nameof(Post1)} failed: {ex.Message}");
#endif
            return default;
        }
        finally
        {
            request.Dispose();
        }
    }
    int SetToken(string _input)
    {
        if (_input == null)
        {
            return 0;
        }

        string[] temp = _input.Split('"');

        if (temp.Length < 9  || temp[9] != "token")
        {
#if UNITY_EDITOR
            Debug.Log("NotRequestToken");
#endif
            return 0;
        }
            
        PlayerPrefs.SetString("PlayerToken", temp[11]);
        
        return 0;
    }

}
