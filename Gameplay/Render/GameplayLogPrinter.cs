using Spectre.Console;

public class GameplayLogPrinter : IPrinter
{
    private static readonly Dictionary<ConsoleColor, string> consoleColorsToSpectre = new Dictionary<ConsoleColor, string>
    {
        {ConsoleColor.Red, "[red]"},
        {ConsoleColor.Green, "[green]"}
    };

    private LiveDisplayContext _displayContext;
    private Layout _layout;
    private Panel _outputPanel;
    private Table _outputTable;
    private int _rows;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
        _rows = 0;

        _outputTable = new Table().AddColumn("").AddColumn("").Border(TableBorder.None);
        _outputPanel = new Panel(_outputTable).Header("Printer").BorderColor(Color.White).Expand();
        _layout["Battle"]["Output"].Update(_outputPanel);
    }

    public void PrintWinMessage(BattleState state)
    {
        if (state == BattleState.PlayerWins)
        {
            Print(new GameplayLogContext("Player win", ConsoleColor.Green));
        }
        else if (state == BattleState.EnemyWins)
        {
            Print(new GameplayLogContext("Enemy win", ConsoleColor.Red));
        }

        Print(new GameplayLogContext("Press any key to exit", ConsoleColor.White));
    }

    public void Print(GameplayLogContext context)
    {
        context.Text = context.Text.Replace("[", "[[").Replace("]", "]]");

        _rows++;
        if (_rows == 20)
        {
            _outputTable.RemoveRow(0);
            _rows--;
        }

        if (consoleColorsToSpectre.ContainsKey(context.ForegroundColor))
        {
            _outputTable.AddRow($"{consoleColorsToSpectre[context.ForegroundColor]}{context.Text}[/]", DateTime.Now.ToString());
        }
        else
        {
            _outputTable.AddRow(context.Text, DateTime.Now.ToString());
        }

        _layout["Battle"]["Output"].Update(_outputPanel);
        _displayContext.Refresh();
    }
}