using Spectre.Console;

public class HubMenu : Menu
{
    private PlayerSave _save;
    private Button[] _buttons;
    private Panel[] _buttonVisual;
    private Layout[] _buttonLayouts;
    private int _selectedIndex = -1;
    private int _previousIndex = -1;

    public HubMenu(PlayerSave save) : base()
    {
        _save = save;
    }

    public override void OnBeforeInitialize()
    {
        base.OnBeforeInitialize();

        _buttons = new Button[7];
        _buttonVisual = new Panel[7];
        _buttonLayouts = new Layout[7];
    }

    protected override void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
               new Layout("Gameplay").Ratio(1)
               .SplitRows(
                   new Layout("General").Ratio(2)
                   .SplitRows(
                       new Layout("Fight").Ratio(1),
                       new Layout("Shop").Ratio(1)
                       ),
                   new Layout("Management").Ratio(3).
                   SplitRows(
                       new Layout("Inventory").Ratio(1),
                       new Layout("Skill").Ratio(1),
                       new Layout("Team").Ratio(1)
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

        var location = JsonStorage<LocationSave>.Load("Londinium");

        _buttons[0] = new Button(Fight);
        _buttonVisual[0] = new Panel(JsonStorage<RichText>.Load("hub_fight").Content).Expand();
        _buttonLayouts[0] = _layout["Root"]["Gameplay"]["General"]["Fight"];
        _buttonLayouts[0].Update(_buttonVisual[0]);

        _buttons[1] = new Button(() => throw new Exception("shop"));
        _buttonVisual[1] = new Panel(JsonStorage<RichText>.Load("hub_shop").Content).Expand();
        _buttonLayouts[1] = _layout["Root"]["Gameplay"]["General"]["Shop"];
        _buttonLayouts[1].Update(_buttonVisual[1]);

        _buttons[2] = new Button(Exit);
        _buttonVisual[2] = new Panel(JsonStorage<RichText>.Load("hub_inventory").Content).Expand();
        _buttonLayouts[2] = _layout["Root"]["Gameplay"]["Management"]["Inventory"];
        _buttonLayouts[2].Update(_buttonVisual[2]);

        _buttons[3] = new Button(() => throw new Exception("test2"));
        _buttonVisual[3] = new Panel(JsonStorage<RichText>.Load("hub_skill").Content).Expand();
        _buttonLayouts[3] = _layout["Root"]["Gameplay"]["Management"]["Skill"];
        _buttonLayouts[3].Update( _buttonVisual[3]);

        _buttons[4] = new Button(() => throw new Exception("team"));
        _buttonVisual[4] = new Panel(JsonStorage<RichText>.Load("hub_team").Content).Expand();
        _buttonLayouts[4] = _layout["Root"]["Gameplay"]["Management"]["Team"];
        _buttonLayouts[4].Update(_buttonVisual[4]);

        _buttons[5] = new Button(() => throw new Exception("save"));
        _buttonVisual[5] = new Panel(JsonStorage<RichText>.Load("hub_save").Content).Expand();
        _buttonLayouts[5] = _layout["Root"]["Misc"]["SaveExit"]["Save"];
        _buttonLayouts[5].Update(_buttonVisual[5]);

        _buttons[6] = new Button(Exit);
        _buttonVisual[6] = new Panel(JsonStorage<RichText>.Load("hub_exit").Content).Expand();
        _buttonLayouts[6] = _layout["Root"]["Misc"]["SaveExit"]["Exit"];
        _buttonLayouts[6].Update(_buttonVisual[6]);

        _layout["Root"]["Misc"]["Title"].Update(new Panel(location.Name).Expand());
        _layout["Root"]["Misc"]["Description"].Update(new Panel(location.Description).Expand());

        _layout["Root"]["Empty1"].Update(new Panel("").NoBorder());
        _layout["Root"]["Misc"]["About"]["Empty2"].Update(new Panel("").NoBorder());
    }

    public override void OnAfterInitialize()
    {
        base.OnAfterInitialize();

        Select(1);
    }

    private void Fight()
    {
        Program.Scene = new GameplayScene(_save, "Title");
        Confirm();
    }

    private void Exit()
    {
        Program.Scene = new MainMenuScene();
        Confirm();
    }

    public override void OnAfterStop()
    {
        base.OnAfterStop();
        Program.Scene.Start();
    }

    private void Select(int offset)
    {
        _previousIndex = _selectedIndex;
        _selectedIndex += offset;

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i == _selectedIndex && !_buttons[i].IsSelected)
            {
                _buttonVisual[i].BorderStyle = Color.Green;
                _buttonLayouts[i].Update(_buttonVisual[i]);
                _buttons[i].Select();
            }

            if (i == _previousIndex && _buttons[i].IsSelected)
            {
                _buttonVisual[i].BorderStyle = Color.White;
                _buttonLayouts[i].Update(_buttonVisual[i]);
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

        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.None)
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    ConfirmSelect();
                }

                if (key.Key == ConsoleKey.W)
                {
                    Select(-1);
                }

                if (key.Key == ConsoleKey.S)
                {
                    Select(1);
                }
            }
        }
    }
}