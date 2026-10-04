using System.Text.Json.Serialization;

public class RichText
{
    public string Name { get; private set; }
    public string Content { get; private set; }

    [JsonConstructor]
    public RichText(string name, string content)
    {
        Name = name;
        Content = content;
    }
}