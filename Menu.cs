using Spectre.Console;

public abstract class Menu
{
    private bool _destroyed = false;
    protected Layout _layout;

    protected abstract void InitializeLayout();

    public void Start()
    {
        OnBeforeInitialize();
        InitializeLayout();
        OnAfterInitialize();

        Task.Run(() =>
        {
            while (!_destroyed)
            {
                OnUpdate();
            }

            OnAfterStop();
        });
        AnsiConsole.Live(_layout)
            .Start(ctx =>
            {
                ctx.Refresh();
                while (!_destroyed) Thread.Sleep(1);
            });
    }

    public void Stop()
    {
        OnBeforeStop();
        _destroyed = true;
    }

    public virtual void OnBeforeInitialize() { }

    public virtual void OnAfterInitialize() {}

    public virtual void OnUpdate() {}

    public virtual void OnBeforeStop() {}

    public virtual void OnAfterStop() {}
}
