using System.Text;

/// <summary>
/// Tightly linked with Console. Renders contents of the buffer right on the screen. 
/// </summary>
public class BufferedConsoleRenderer : IConsoleRenderer
{
    private ScreenBuffer _front = new(1, 1);
    private ScreenBuffer _back = new(1, 1);

    /// <summary>
    /// Just for easier testing. Allows to disable escape-sequences while rendering to test in old console,
    /// as old console doesn't support Ansi сolors.
    /// </summary>
    private bool _disableColors = false;

    /// <summary>
    /// Should be set to true when full screen should be re-rendered. For example, when both buffers are cleared.
    /// </summary>
    private bool _forceRender = false;

    public ScreenBuffer Buffer => _front;

    public BufferedConsoleRenderer()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public void SetSize(int width, int height)
    {
        ConsoleUtils.SetConsoleSizeSafe(width, height);

        _front.SetSize(width, height);
        _back.SetSize(width, height);
    }

    private readonly ScreenCell _emptyCell = new(' ', AnsiColor.White, AnsiColor.Black);

    public void Render()
    {
        var curr = _front.Surface;
        var prev = _back.Surface;
        int height = _front.Height;
        int width = _front.Width;

        var sb = new StringBuilder();

        for (int y = 0; y < height; y++)
        {
            bool changed = false;

            for (int x = 0; x < width; x++)
            {
                if (_forceRender || curr[y, x] != prev[y, x])
                {
                    changed = true;
                    break;
                }
            }

            if (!changed)
            {
                continue;
            }
            
            Console.SetCursorPosition(0, y);

            int? lastFg = null;
            int? lastBg = null;

            for (int x = 0; x < width; x++)
            {
                var cell = curr[y, x];

                if (cell == null)
                {
                    cell = _emptyCell;
                }

                if (!_disableColors) {
                    if (cell.Value.Color != lastFg || cell.Value.BgColor != lastBg) {
                        sb.Append(AnsiColor.GetCode(cell.Value.Color, cell.Value.BgColor));
                        lastFg = cell.Value.Color;
                        lastBg = cell.Value.BgColor;
                    }
                }

                sb.Append(cell.Value.Char);
            }

            if (!_disableColors) {
                sb.Append(AnsiColor.Reset);                
            }
            
            // Doesn't work with `AnsiConsole` from `Spectre.Console`, so leaving as designed.
            Console.Write(sb.ToString());
            sb.Clear();
        }

        // Swap buffers and clear front buffer, so we can properly check difference with previous frame
        // and re-render only changed cells.
        (_front, _back) = (_back, _front);
        _front.Clear();

        _forceRender = false;
    }

    public void Clear()
    {
        _front.Clear();
        _back.Clear();
        _forceRender = true;
    }
}