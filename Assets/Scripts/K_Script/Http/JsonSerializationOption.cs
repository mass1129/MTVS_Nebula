using System;
using Newtonsoft.Json;
using UnityEngine;

public class JsonSerializationOption : ISerializationOption
{
    public string ContentType => "application/json";

    public T Deserialize<T>(string text)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(text);
            Debug.Log($"Success: {text}");
            return result;
            
        }

        catch (Exception ex)
        {
            Debug.LogError($"{this}Could not parse response {text}. {ex.Message}");
            return default;
        }
    }
    public string Serialize(string text)
    {
        try
        {
            var result = text;
            Debug.Log($"Success: {text}");
            return result;
            //SceneManager.LoadScene(1);
        }

        catch (Exception ex)
        {
            Debug.LogError($"{this}Could not parse response {text}. {ex.Message}");
            return default;
        }
    }
}
