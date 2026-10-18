public class GameplayScene : IScene
{
    public Action<BattleState> LevelHasFinished;

    private GameplayLogPrinter _gameplayLogPrinter = new GameplayLogPrinter();
    private UnitsPrinter _unitsPrinter = new UnitsPrinter();
    private StatsPrinter _statsPrinter = new StatsPrinter();
    private VitalsPrinter _vitalsPrinter = new VitalsPrinter();
    private SkillsPrinter _skillsPrinter = new SkillsPrinter();
    private TurnPrinter _turnPrinter = new TurnPrinter();
    private SkillMenu _skillMenu = new SkillMenu();

    private PlayerSave _save;
    private string _arenaName;

    public GameplayScene(PlayerSave save, string arenaName)
    {
        _save = save;
        _arenaName = arenaName;
    }

    public void Start()
    {
        Task.Run(() =>
        {
            Thread.Sleep(1500);
            Arena arena = new Arena(_gameplayLogPrinter, _unitsPrinter, _statsPrinter, _vitalsPrinter, _turnPrinter, _skillsPrinter, SaveLoad<ArenaModel>.Load(_arenaName), _skillMenu);
            arena.Start();

            LevelHasFinished?.Invoke(arena.State);
        });

        GameplayRender render = new GameplayRender(this, _gameplayLogPrinter, _unitsPrinter, _statsPrinter, _vitalsPrinter, _skillsPrinter, _turnPrinter, _skillMenu);

        Program.Scene = new HubScene(_save);
        Program.Scene.Start();
    }
}