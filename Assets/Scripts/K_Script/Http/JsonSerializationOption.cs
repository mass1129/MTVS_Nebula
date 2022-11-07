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
            //JObject jObject = JObject.Parse(jsonResponse);
            //IList<JToken> results = jObject["results"]["placeObjects"].Children().ToList();
            //IList<TResultType> searchResults = new List<TResultType>();
            //foreach(JToken result in results)
            //{
            //    var v = result.ToObject<TResultType>();
            //    searchResults.Add(v);
            //}
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
