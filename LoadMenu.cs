using Spectre.Console;

public class LoadMenu
{
    private Layout _layout;
    private Panel _emptyPanel;

    private Table _menuTable;
    private Panel _menuPanel;

    private int _selectedIndex = 0;
    private int _upperBorder = 0;
    private int _lowerBorder = 0;
    private string[] _saveNames;

    private bool _isCrossSelected = false;

    public LoadMenu(string[] saveNames)
    {
        _saveNames = new string[saveNames.Length];
        saveNames.CopyTo(_saveNames, 0);

        _lowerBorder = 0;
        _upperBorder = Math.Min(saveNames.Length - 1, 2);

        InitializeLayout();
        Start();
    }

    public bool TryGetSelectedLoad(out string selectedLoadName)
    {
        if (_isCrossSelected)
        {
            selectedLoadName = "";
            return false;
        }

        selectedLoadName = _saveNames[_selectedIndex];
        return true;
    }

    private void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
            new Layout("Empty1").Ratio(1),
            new Layout("MenuColumn").Ratio(1)
                .SplitRows(
                new Layout("Empty3").Ratio(1),
                new Layout("MenuRoot").Ratio(1)
                .SplitColumns(
                    new Layout("Menu").Ratio(2),
                    new Layout("Misc").Ratio(1)
                    .SplitRows(
                        new Layout("Cross").Ratio(1),
                        new Layout("Scrollbar").Ratio(1)
                    )
                    ),
                new Layout("Empty4").Ratio(1)
            ),
            new Layout("Empty2").Ratio(1)
         );

        Reset();
    }

    public int GetSelectedIndex()
    {
        return _selectedIndex;
    }

    private void SelectUp()
    {
        if (_saveNames.Length == 0 || _isCrossSelected)
        {
            return;
        }

        if (_selectedIndex == _lowerBorder && _selectedIndex != 0)
        {
            _upperBorder--;
            _lowerBorder--;
        }

        _selectedIndex = Math.Clamp(_selectedIndex - 1, 0, _saveNames.Length - 1);
        Reset();
    }

    private void SelectDown()
    {
        if (_saveNames.Length == 0 || _isCrossSelected)
        {
            return;
        }

        if (_selectedIndex == _upperBorder && _selectedIndex != _saveNames.Length - 1)
        {
            _upperBorder++;
            _lowerBorder++;
        }

        _selectedIndex = Math.Clamp(_selectedIndex + 1, 0, _saveNames.Length - 1);
        Reset();
    }

    private void SelectCross()
    {
        _isCrossSelected = true;
        Reset();
    }

    private void DeselectCross()
    {
        _isCrossSelected = false;
        Reset();
    }

    private void Reset()
    {
        _emptyPanel = new Panel("").Header("").NoBorder().Expand();
        _layout["Empty1"].Update(_emptyPanel);
        _layout["Empty2"].Update(_emptyPanel);
        _layout["MenuColumn"]["Empty3"].Update(_emptyPanel);
        _layout["MenuColumn"]["Empty4"].Update(_emptyPanel);

        Panel crossPanel = new Panel("X");
        if (_saveNames.Length == 0)
        {
            _isCrossSelected = true;
        }

        if (_isCrossSelected)
        {
            crossPanel.BorderColor(Color.Green);
        }
        else
        {
            crossPanel.BorderColor(Color.White);
        }

        _layout["MenuColumn"]["MenuRoot"]["Misc"]["Cross"].Update(crossPanel);

        if (_saveNames.Length > 0)
        {
            _layout["MenuColumn"]["MenuRoot"]["Misc"]["Scrollbar"].Update(new Panel($"{_selectedIndex + 1} / {_saveNames.Length}") { Width = 14 }.NoBorder());

            _menuTable = new Table().Expand().BorderColor(Color.Black);
            _menuTable.AddColumn("");
            for (int i = _lowerBorder; i <= _upperBorder; i++)
            {
                if (i == _selectedIndex && !_isCrossSelected)
                {
                    _menuTable.AddRow(new Panel(_saveNames[i]) { Width = 40, BorderStyle = Color.Green });
                }
                else
                {
                    _menuTable.AddRow(new Panel(_saveNames[i]) { Width = 40, BorderStyle = Color.White });
                }
            }
            _menuPanel = new Panel(_menuTable).Header("My Saves").Expand();
            _layout["MenuColumn"]["MenuRoot"]["Menu"].Update(_menuPanel);
        }

        else
        {
            _layout["MenuColumn"]["MenuRoot"]["Misc"]["Scrollbar"].Update(new Panel($"_") { Width = 14 }.NoBorder());
            _menuPanel = new Panel("\n\n\n\n\n\n                No saves.").Header("My Saves").Expand();
            _layout["MenuColumn"]["MenuRoot"]["Menu"].Update(_menuPanel);
        }

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
                    if (keyInfo.Key == ConsoleKey.D)
                    {
                        SelectCross();
                        ctx.Refresh();
                    }
                    if (keyInfo.Key == ConsoleKey.A)
                    {
                        DeselectCross();
                        ctx.Refresh();
                    }
                }
                while (keyInfo.Key != ConsoleKey.Enter);
            });
    }
}
