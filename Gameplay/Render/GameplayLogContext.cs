public struct GameplayLogContext
{
    public string Text;
    public ConsoleColor ForegroundColor;
    public ConsoleColor BackgroundColor;

    public GameplayLogContext(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        Text = text;
        ForegroundColor = foregroundColor;
        BackgroundColor = backgroundColor;
    }

    public GameplayLogContext(string text, ConsoleColor foregroundColor)
    {
        Text = text;
        ForegroundColor = foregroundColor;
        BackgroundColor = ConsoleColor.Black;
    }

    public GameplayLogContext(string text)
    {
        Text = text;
        ForegroundColor = ConsoleColor.White;
        BackgroundColor = ConsoleColor.Black;
    }
}