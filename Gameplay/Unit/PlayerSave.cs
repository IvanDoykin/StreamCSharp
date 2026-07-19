using System.Text.Json.Serialization;

public class PlayerSave
{
    public string Name { get; private set; }
    public List<UnitSave> Units { get; private set; }

    [JsonConstructor]
    public PlayerSave(List<UnitSave> units, string name)
    {
        Units = new List<UnitSave>(units);
        Name = name;
    }

    public PlayerSave(string name)
    {
        Name = name;
        Units = new List<UnitSave>();
        Units.Add(SaveLoad<UnitSave>.Load("Тимур"));
    }
}