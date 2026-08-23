using System.Security.Principal;

internal class Program
{
    public static IScene Scene;
    private static async Task Main()
    {
        try
        {
            WindowSettings.Initialize();
            Logger.enabled = true;

            //var startup = new StartupScene();
            //await startup.Play();

            bool isMenuConfirmed = false;
            do
            {
                var menu = new MainMenu();
                if (menu.GetSelectedIndex() == 0)
                {
                    Console.Write("Введите название сохранения (или q, чтобы выйти): ");

                    string name = "";
                    PlayerSave playerSave = new PlayerSave("");
                    do
                    {
                        name = Console.ReadLine();
                        if (name == "q")
                        {
                            isMenuConfirmed = false;
                            break;
                        }
                        playerSave = new PlayerSave(name);

                        if (name.Length < 3)
                        {
                            Console.Write($"Введите название с 3 символами или больше (или q, чтобы выйти): ");
                            continue;
                        }

                        if (SaveLoad<PlayerSave>.CheckOnExist(playerSave, name))
                        {
                            Console.Write($"Сохранение {name} уже существует. Введите другое название (или q, чтобы выйти): ");
                        }
                        else
                        {
                            isMenuConfirmed = true;
                        }
                    } while (SaveLoad<PlayerSave>.CheckOnExist(playerSave, name) || name.Length < 3);

                    if (isMenuConfirmed)
                    {
                        SaveLoad<PlayerSave>.Save(playerSave, name);
                        Scene = new NewGameScene(playerSave);
                        Scene = new HubScene(playerSave);
                    }
                }
                if (menu.GetSelectedIndex() == 1)
                {
                    Console.WriteLine();
                    LoadMenu loadMenu = new LoadMenu(SaveLoad<PlayerSave>.GetAllSaves());

                    isMenuConfirmed = false;
                    if (loadMenu.TryGetSelectedLoad(out string saveName))
                    {
                        Console.WriteLine($"Сохранение {saveName} готовится к загрузке!");
                        isMenuConfirmed = true;
                        Scene = new HubScene(SaveLoad<PlayerSave>.Load(saveName));
                    }
                }
                if (menu.GetSelectedIndex() == 2)
                {
                    isMenuConfirmed = true;
                    return;
                }

            }
            while (!isMenuConfirmed);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return;
        }
    }
}