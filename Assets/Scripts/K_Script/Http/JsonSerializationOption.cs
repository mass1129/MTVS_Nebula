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
#if UNITY_EDITOR
            Debug.Log($"Success: {text}");
#endif
            return result;
            
        }

        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"Could not parse response {text}. {ex.Message}");
#endif
            return default;
        }
    }

   
}
