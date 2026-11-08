using Spectre.Console;

public abstract class Menu
{
    private bool _confirmed = true;
    public bool Confirmed => _confirmed;

    private bool _destroyed = false;
    protected Layout _layout;

    protected abstract void InitializeLayout();

    public void Start()
    {
        OnBeforeInitialize();
        InitializeLayout();
        OnAfterInitialize();

        AnsiConsole.Live(_layout)
            .Start(ctx =>
            {
                while (!_destroyed)
                {
                    OnUpdate();
                    ctx.Refresh();
                }
            });

        OnAfterStop();
    }

    public void Confirm()
    {
        _confirmed = true;
        OnBeforeStop();
        _destroyed = true;
    }

    public void Discard()
    {
        _confirmed = false;
        OnBeforeStop();
        _destroyed = true;
    }

    public virtual void OnBeforeInitialize() { }

    public virtual void OnAfterInitialize() {}

    public virtual void OnUpdate() {}

    public virtual void OnBeforeStop() {}

    public virtual void OnAfterStop() {}
}
