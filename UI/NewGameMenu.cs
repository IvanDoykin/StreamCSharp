using Spectre.Console;

public class NewGameMenu : Menu
{
    private readonly string text = "Y o u   a r e   a   f o r m e r   c e n t u r i o n ,   b e t r a y e d   b y   y o u r\n\nl e g a t e   a n d   s o l d   i n t o   s l a v e r y .   Y o u r   n e w   h o m e   i s\n\nt h e   d a m p   u n d e r b e l l y   o f   a   p r o v i n c i a l   a m p h i t h e a t e r.\n\nT o   s u r v i v e   a n d   e x a c t   r e v e n g e ,   y o u   m u s t   l e a d   a\n\ns q u a d   o f   f e l l o w   o u t c a s t s .   S t a i n   t h e   a r e n a   s a n d s\n\nw i t h   e n e m y   [red]b l o o d[/] ,   w i n   t h e   c r o w d ’ s   l o v e ,   a n d\n\nc a r v e   y o u r   p a t h   t o   [green]f r e e d o m[/]   a n d   t h e   s u m m i t s   o f\n\nR o m e .".ToUpperInvariant();
    private readonly string enterText = "                              P R E S S  [green]E N T E R[/]  T O  S T A R T";

    private PlayerSave _save;

    public NewGameMenu(PlayerSave save)
    {
        _save = save;
    }

    protected override void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
               new Layout("Empty1").Ratio(1),
               new Layout("Main").Ratio(2)
                .SplitRows(
                   new Layout("Empty3").Ratio(1),
                   new Layout("Text").Ratio(5),
                   new Layout("Empty4").Ratio(1),
                   new Layout("TextEnter").Ratio(1)
                   ),
               new Layout("Empty2").Ratio(1)
           );

        _layout["Root"]["Empty1"].Update(new Panel("").NoBorder().Expand());
        _layout["Root"]["Empty2"].Update(new Panel("").NoBorder().Expand());

        _layout["Root"]["Main"]["Empty3"].Update(new Panel("").NoBorder().Expand());
        _layout["Root"]["Main"]["Empty4"].Update(new Panel("").NoBorder().Expand());

        _layout["Root"]["Main"]["Text"].Update(new Panel(text).Expand());
        _layout["Root"]["Main"]["TextEnter"].Update(new Panel(enterText).Expand());
    }

    public override void OnUpdate()
    {
        if (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                Confirm();
            }
        }
    }

    public override void OnAfterStop()
    {
        base.OnAfterStop();
        Program.Scene = new HubScene(_save);
        Program.Scene.Start();
    }
}