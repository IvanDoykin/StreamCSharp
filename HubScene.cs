public class HubScene : IScene
{
    private HubMenu _menu;
    private PlayerSave _save;

    public HubScene(PlayerSave save)
    {
        _save = save;
        _menu = new HubMenu(save);
        _menu.Start();
    }
}
