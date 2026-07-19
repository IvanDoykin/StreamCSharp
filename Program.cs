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

            var menu = new MainMenu();
            if (menu.GetSelectedIndex() == 0)
            {
                Console.Write("Введите название сохранения: ");

                string name = "";
                PlayerSave playerSave = new PlayerSave("");
                do
                {
                    name = Console.ReadLine();
                    playerSave = new PlayerSave(name);

                    if (SaveLoad<PlayerSave>.CheckOnExist(playerSave, name))
                    {
                        Console.Write($"Сохранение {name} уже существует. Введите другое название: ");
                    }
                } while (SaveLoad<PlayerSave>.CheckOnExist(playerSave, name));

                SaveLoad<PlayerSave>.Save(playerSave, name);
            }
            if (menu.GetSelectedIndex() == 1)
            {
                
            }
            if (menu.GetSelectedIndex() == 2)
            {
                return;
            }

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