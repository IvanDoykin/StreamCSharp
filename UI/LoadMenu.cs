using Spectre.Console;

public class LoadMenu : Menu
{
    private Panel _emptyPanel;
    private Table _menuTable;
    private Panel _menuPanel;

    private int _selectedIndex = 0;
    public int SelectedIndex => _selectedIndex;

    private int _upperBorder = 0;
    private int _lowerBorder = 0;
    private string[] _saveNames;

    private bool _isCrossSelected = false;

    public string SelectedSaveName => _saveNames[_selectedIndex];

    public LoadMenu()
    {
        ResetSelection();
    }

    private void ResetSelection()
    {
        _saveNames = JsonStorage<PlayerSave>.GetAll();
        _selectedIndex = 0;
        _lowerBorder = 0;
        _upperBorder = Math.Min(_saveNames.Length - 1, 2);
    }

    protected override void InitializeLayout()
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

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.W)
            {
                SelectUp();
            }
            if (keyInfo.Key == ConsoleKey.S)
            {
                SelectDown();
            }
            if (keyInfo.Key == ConsoleKey.D)
            {
                SelectCross();
            }
            if (keyInfo.Key == ConsoleKey.A)
            {
                DeselectCross();
            }

            if (keyInfo.Key == ConsoleKey.Delete && _saveNames.Length > 0 && !_isCrossSelected)
            {
                if (ConfirmDelete(_selectedIndex))
                {
                    Delete(_saveNames[_selectedIndex]);
                    ResetSelection();
                    Reset();
                }
            }

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (_isCrossSelected)
                {
                    Discard();
                }
                else
                {
                    Confirm();
                }
            }
        }
    }

    private void Delete(string saveName)
    {
        JsonStorage<PlayerSave>.Delete(saveName);
    }

    private bool ConfirmDelete(int saveIndex)
    {
        Console.WriteLine($"Are you sure to DELETE save with name {_saveNames[_selectedIndex]}? (y/N)");
        Console.Write(">");
        var answer = Console.ReadLine();
        if (answer.ToLower() == "y")
        {
            Console.WriteLine($"Type save name MANUALLY to confirm: {_saveNames[_selectedIndex]}.");
            var confirmedAnswer = Console.ReadLine();
            if (confirmedAnswer == _saveNames[saveIndex])
            {
                return true;
            }
        }

        return false;
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
}
