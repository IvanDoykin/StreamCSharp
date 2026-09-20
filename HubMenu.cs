using Spectre.Console;

public class HubMenu : Menu
{
    private Button[] _buttons;
    private int _selectedIndex = 0;

    public override void OnBeforeInitialize()
    {
        base.OnBeforeInitialize();

        _buttons = new Button[3];
    }

    protected override void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
               new Layout("Gameplay").Ratio(1)
               .SplitRows(
                   new Layout("Management").Ratio(3).
                   SplitRows(
                       new Layout("Inventory").Ratio(1),
                       new Layout("Skill").Ratio(1),
                       new Layout("Team").Ratio(1)
                       ),
                   new Layout("General").Ratio(2)
                   .SplitRows(
                       new Layout("Fight").Ratio(1),
                       new Layout("Shop").Ratio(1)
                       )
                   ),
               new Layout("Empty1").Ratio(1),
               new Layout("Misc").Ratio(1).
               SplitRows(
                   new Layout("About").Ratio(3)
                   .SplitRows(
                       new Layout("Title").Ratio(8),
                       new Layout("Empty2").Ratio(1),
                       new Layout("Description").Ratio(15)
                       ),
                   new Layout("SaveExit").Ratio(2)
                   .SplitRows(
                       new Layout("Save").Ratio(1),
                       new Layout("Exit").Ratio(1)
                       )
                   )
           );

        var location = SaveLoad<LocationSave>.Load("Londinium");

        _buttons[0] = new Button(Exit);
        _buttons[1] = new Button(() => throw new Exception("test2"));
        _buttons[2] = new Button(() => throw new Exception("team"));

        _layout["Root"]["Misc"]["Title"].Update(new Panel(location.Name).Expand());
        _layout["Root"]["Misc"]["Description"].Update(new Panel(location.Description).Expand());
        _layout["Root"]["Empty1"].Update(new Panel("").NoBorder());
        _layout["Root"]["Misc"]["About"]["Empty2"].Update(new Panel("").NoBorder());
    }

    public override void OnAfterInitialize()
    {
        base.OnAfterInitialize();

        Select();
    }

    private void Exit()
    {
        Stop();
    }

    public override void OnAfterStop()
    {
        base.OnAfterStop();
        Program.Scene = new NewGameScene(SaveLoad<PlayerSave>.Load("Ivan"));
    }

    private void Select()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i == _selectedIndex && !_buttons[i].IsSelected)
            {
                _buttons[i].Select();
            }

            if (i != _selectedIndex && _buttons[i].IsSelected)
            {
                _buttons[i].Deselect();
            }
        }
    }

    private void ConfirmSelect()
    {
        _buttons[_selectedIndex].Click();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.None)
        {
            if (key.Key == ConsoleKey.Enter)
            {
                ConfirmSelect();
                Stop();
            }

            if (key.Key == ConsoleKey.S)
            {
                _selectedIndex++;
                Select();
            }
        }
        
    }
}