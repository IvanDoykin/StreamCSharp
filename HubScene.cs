using Spectre.Console;

public class HubScene : IScene
{
    private PlayerSave _save;
    private Layout _layout;
    private bool _destroy = false;

    public HubScene(PlayerSave save)
    {
        _save = save;

        InitializeLayout();
        Start();
    }

    private void InitializeLayout()
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

        _layout["Root"]["Misc"]["Title"].Update(new Panel(location.Name).Expand());
        _layout["Root"]["Misc"]["Description"].Update(new Panel(location.Description).Expand());
        _layout["Root"]["Empty1"].Update(new Panel("").NoBorder());
        _layout["Root"]["Misc"]["About"]["Empty2"].Update(new Panel("").NoBorder());
    }

    private void Stop()
    {
        _destroy = true;
    }

    private void Start()
    {
        Task.Run(() =>
        {
            while (!_destroy)
            {
                
            }
        });
        AnsiConsole.Live(_layout)
            .Start(ctx =>
            {
                ctx.Refresh();
                while (!_destroy) Thread.Sleep(1000);
            });
    }
}
