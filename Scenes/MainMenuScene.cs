public class MainMenuScene : IScene
{
    public void Start()
    {
        while (Program.Scene == this)
        {
            var menu = new MainMenu();
            menu.Start();

            if (menu.SelectedIndex == 0)
            {
                NewSaveMenu newSaveMenu = new NewSaveMenu();
                newSaveMenu.Start();

                if (newSaveMenu.Confirmed)
                {
                    JsonStorage<PlayerSave>.Save(newSaveMenu.NewSave, newSaveMenu.NewSave.Name);
                    Program.Scene = new NewGameScene(newSaveMenu.NewSave);
                    Program.Scene.Start();
                }
            }

            if (menu.SelectedIndex == 1)
            {
                Console.WriteLine();
                LoadMenu loadMenu = new LoadMenu();
                loadMenu.Start();

                if (loadMenu.Confirmed)
                {
                    Program.Scene = new HubScene(JsonStorage<PlayerSave>.Load(loadMenu.SelectedSaveName));
                    Program.Scene.Start();
                }
            }

            if (menu.SelectedIndex == 2)
            {
                Environment.Exit(0);
            }
        }
    }
}