using Spectre.Console;

public class StartupScene : IScene
{
    private const int C4 = 262;
    private const int D4 = 294;
    private const int G4 = 392;
    private const int A4 = 440;
    private const int B4 = 494;
    private const int C5 = 523;
    private const int E5 = 659;
    private const int F5 = 698;
    private const int G5 = 784;
    private const int A5 = 880;

    private const int beat = 400;    
    private const int halfBeat = 200;

    public StartupScene()
    {
    }

    public async Task Play()
    {
        var image = new CanvasImage("logo.png");
        AnsiConsole.Write(image);

        PlayNote(C4, halfBeat);
        PlayNote(C4, halfBeat);

        PlayNote(D4, halfBeat);
        PlayNote(D4, halfBeat);

        PlayNote(G4, beat);
        PlayNote(A4, beat);
        PlayNote(B4, beat * 2);

        PlayNote(C5, beat);
        PlayNote(E5, beat);
        PlayNote(G5, beat * 2);

        PlayNote(F5, halfBeat);
        PlayNote(G5, halfBeat);
        PlayNote(A5, beat * 3);
    }

    private void PlayNote(int frequency, int duration)
    {
        try
        {
            Console.Beep(frequency, duration);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка воспроизведения звука: {ex.Message}");
        }
    }
}