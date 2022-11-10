public interface ISerializationOption
{   
    string ContentType { get; }
    T Deserialize<T>(string text);

    string Serialize(string text);
}
