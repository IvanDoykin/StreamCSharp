using System.Text.Json.Serialization;

public class LocationSave
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    //public List<ArenaModel> ArenaModels { get; private set; }

    [JsonConstructor]
    public LocationSave(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
