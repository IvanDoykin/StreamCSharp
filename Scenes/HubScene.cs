public class HubScene : IScene
{
    private HubMenu _menu;
    private PlayerSave _save;

    public HubScene(PlayerSave save)
    {
        _save = save;
    }

    public void Start()
    {
        _menu = new HubMenu(_save);
        _menu.Start();
    }
}
