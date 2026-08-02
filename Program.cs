using System.Security.Principal;

internal class Program
{
    public static Action<BattleState> LevelHasFinished;
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
                    }
                }
                if (menu.GetSelectedIndex() == 2)
                {
                    isMenuConfirmed = true;
                    return;
                }

            }
            while (!isMenuConfirmed);

            //GameplayLogPrinter gameplayLogPrinter = new GameplayLogPrinter();
            //UnitsPrinter unitsPrinter = new UnitsPrinter();
            //StatsPrinter statsPrinter = new StatsPrinter();
            //VitalsPrinter vitalsPrinter = new VitalsPrinter();
            //SkillsPrinter skillsPrinter = new SkillsPrinter();
            //TurnPrinter turnPrinter = new TurnPrinter();
            //SkillMenu skillMenu = new SkillMenu();

            //Task.Run(() =>
            //{
            //    Thread.Sleep(1500);
            //    Arena arena = new Arena(gameplayLogPrinter, unitsPrinter, statsPrinter, vitalsPrinter, turnPrinter, skillsPrinter, SaveLoad<ArenaModel>.Load("Title"), skillMenu);
            //    arena.Start();

            //    LevelHasFinished?.Invoke(arena.State);
            //});

            //GameplayRender render = new GameplayRender(gameplayLogPrinter, unitsPrinter, statsPrinter, vitalsPrinter, skillsPrinter, turnPrinter, skillMenu);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return;
        }
    }
}