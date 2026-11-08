using Spectre.Console;

public class MainMenu : Menu
{
    private readonly string[] _menuNames = new string[] { "Новая игра", "Загрузить", "Выйти" };

    private Panel _emptyPanel;

    private Table _menuTable;
    private Panel _menuPanel;

    private int _selectedIndex = 0;
    public int SelectedIndex => _selectedIndex;

    protected override void InitializeLayout()
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

    public override void OnUpdate()
    {
        base.OnUpdate();

        ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

        if (Console.KeyAvailable)
        {
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.W)
            {
                SelectUp();
            }
            if (keyInfo.Key == ConsoleKey.S)
            {
                SelectDown();
            }
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Confirm();
            }
        }
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
        _emptyPanel = new Panel("").Header("").NoBorder().Expand();
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
}