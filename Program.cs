internal class Program
{
    private const bool skipStartup = true;
    public static IScene Scene;

    private static async Task Main()
    {
        try
        {
            WindowSettings.Initialize();
            Logger.enabled = true;

            if (skipStartup)
            {
                Scene = new MainMenuScene();
                Scene.Start();
            }
            else
            {
                Scene = new StartupScene();
                Scene.Start();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return;
        }
    }
}