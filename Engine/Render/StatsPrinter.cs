using Spectre.Console;

public class StatsPrinter : IPrinter
{
    private LiveDisplayContext _displayContext;
    private Layout _layout;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
    }

    public void Reset()
    {
        var text = new Text("No selected unit", new Style(Color.Gray, null, Decoration.Bold, null));
        var panel = new Panel(text).Header("Stats").BorderColor(Color.White).Expand();
        _layout["Info"]["Stats"].Update(panel);
        _displayContext.Refresh();
    }

    public void Print(StatsContext context)
    {
        var table = new Table().AddColumn("").AddColumn("").Border(TableBorder.None);
        var panel = new Panel(table).Header("Stats").BorderColor(Color.White).Expand();

        table.AddRow("Имя: ", context.Unit.Model.Name);
        table.AddRow("Базовый урон: ", context.Unit.BaseDamage.ToString());
        table.AddRow("Атака: ", context.Unit.Attack.ToString());
        table.AddRow("Защита: ", context.Unit.Defense.ToString());
        table.AddRow("Скорость: ", context.Unit.Speed.ToString());
        table.AddRow("Инициатива: ", context.Unit.Initiative.ToString());

        _layout["Info"]["Stats"].Update(panel);
        _displayContext.Refresh();
    }
}