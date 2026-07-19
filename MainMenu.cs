using Spectre.Console;

public class MainMenu
{
    private readonly string[] _menuNames = new string[]{"Новая игра", "Загрузить", "Выйти" };

    private Layout _layout;
    private Panel _emptyPanel;

    private Table _menuTable;
    private Panel _menuPanel;

    private int _selectedIndex = 0;

    public MainMenu()
    {
        InitializeLayout();
        Start();
    }

    private void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
            new Layout("Empty1").Ratio(2),
            new Layout("MenuColumn").Ratio(1)
                .SplitRows(
                new Layout("Empty3").Ratio(1),
                new Layout("MenuRow").Ratio(1),
                new Layout("Empty4").Ratio(1)
            ), 
            new Layout("Empty2").Ratio(2)
         );

        Reset();
    }

    public int GetSelectedIndex()
    {
        return _selectedIndex;
    }

    private void SelectUp()
    {
        _selectedIndex = Math.Clamp(_selectedIndex - 1, 0, _menuNames.Length - 1);
        Reset();
    }

    private void SelectDown()
    {
        _selectedIndex = Math.Clamp(_selectedIndex + 1, 0, _menuNames.Length - 1);
        Reset();
    }

    private void Reset()
    {
        _emptyPanel = new Panel("").Header("").BorderColor(Color.Black).Expand();
        _layout["Empty1"].Update(_emptyPanel);
        _layout["Empty2"].Update(_emptyPanel);
        _layout["MenuColumn"]["Empty3"].Update(_emptyPanel);
        _layout["MenuColumn"]["Empty4"].Update(_emptyPanel);

        _menuTable = new Table().Expand().BorderColor(Color.Black);
        _menuTable.AddColumn("");
        for (int i = 0; i < _menuNames.Length; i++)
        {
            if (i == _selectedIndex)
            {
                _menuTable.AddRow(new Panel(_menuNames[i]) { Width = 40, BorderStyle = Color.Green });
            }
            else
            {
                _menuTable.AddRow(new Panel(_menuNames[i]) { Width = 40, BorderStyle = Color.White });
            }
        }    
        _menuPanel = new Panel(_menuTable).Header("Gladiator").Expand();
        _layout["MenuColumn"]["MenuRow"].Update(_menuPanel);
    }

    private void Start()
    {
        AnsiConsole.Live(_layout)
            .Start(ctx =>
            {
                ctx.Refresh();
                ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
                do
                { 
                    keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.W)
                    {
                        SelectUp();
                        ctx.Refresh();
                    }
                    if (keyInfo.Key == ConsoleKey.S)
                    {
                        SelectDown();
                        ctx.Refresh();
                    }
                }
                while (keyInfo.Key != ConsoleKey.Enter);
            });
    }
}