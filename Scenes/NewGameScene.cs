public class NewGameScene : IScene
{
    private PlayerSave _save;

    public NewGameScene(PlayerSave save)
    {
        _save = save;
    }

    public void Start()
    {
        var menu = new NewGameMenu(_save);
        menu.Start();
    }
}
