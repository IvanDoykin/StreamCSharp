
public class SkipSkill : ISkill, ISelfSkill
{
    public SkillMenu Menu => _menu;
    public string Name => _name;
    public string Description => _description;

    public Unit Origin => _origin;

    public IReadOnlyList<Unit> Targets => new List<Unit>();

    private Unit _origin;
    private SkillMenu _menu;
    private string _name;
    private string _description;

    public SkipSkill(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void Initialize(SkillMenu menu, Unit origin, IEnumerable<Unit> targets)
    {
        _origin = origin;
        _menu = menu;
    }

    public bool TryExecute(GameplayLogPrinter printer)
    {
        _menu.Update("Skip", new string[] {"Хотите пропустить ход?"});
        if (!_menu.TryGetChoice(out int selectedIndex))
        {
            return false;
        }

        printer.Print(new LogContext("Пропускаем ход"));
        return true;
    }
}